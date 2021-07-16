using Microsoft.Extensions.Options;
using Microsoft.Reporting.NETCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MyApi.Core
{
    public class SSRSRender : ISSRSRender
    {
        private string _serverUrl;
        private readonly SSRS ssrsSetting;
        //private ICredentials _credentials;
        //private readonly IConfiguration configuration;
        private int timeout = 200000;
        private readonly IHttpClientFactory _httpClientFactory;
        /// <summary>
        /// Constructs a new instance of the SimpleSSRSRender class
        /// </summary>
        /// <param name="serverUrl">URL of the Report Server</param>
        public SSRSRender(SSRS _appSettings, string serverUrl = null)
        {
            ssrsSetting = _appSettings;
            _serverUrl = serverUrl;
        }

        public SSRS GetReportSetting() => ssrsSetting;

        /// <summary>
        /// Render an SSRS report the specified format
        /// </summary>
        /// <param name="reportPath">Path to the SSRS report</param>
        /// <param name="reportParameters">Collection of parameters for the report</param>
        /// <param name="outputFormat">Output format e.g PDF or EXCEL</param>
        /// <returns>A byte array of the output that can be saved to a file</returns>
        /// <remarks>The output bytes can be saved to file using code like File.WriteAllBytesAsync</remarks>
        public async Task<byte[]> RenderReport(string reportPath, NameValueCollection reportParameters, string outputFormat = "PDF")
        {
            string url = BuildFullRenderUrl(reportPath, reportParameters, outputFormat);
            string uriEscape = Uri.EscapeUriString(url);


            // EscapeUriString 
            HttpClient client = _httpClientFactory.CreateClient("SSRS");

            //var byteArray = Encoding.ASCII.GetBytes($"{initailAppSettings.ReportServer.Username}:{initailAppSettings.ReportServer.Password}");
            //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));
            // new HttpClient(new HttpClientHandler() { UseDefaultCredentials = true, Credentials = _credentials });

            return await client.GetByteArrayAsync(uriEscape);

        }

        public async Task<byte[]> Render(string reportPath, NameValueCollection reportParameters, string outputFormat = "PDF")
        {
            // string url = BuildFullRenderUrl(reportPath, reportParameters, outputFormat);
            // HttpClient client = _httpClientFactory.CreateClient("SSRS");
            try
            {
                Uri baseURL = new(string.IsNullOrEmpty(_serverUrl) ? ssrsSetting.Host : _serverUrl);
                ServerReport report = new();
                report.ReportServerCredentials.NetworkCredentials = new NetworkCredential(ssrsSetting.Username, ssrsSetting.Password, $"{baseURL.Scheme}://{baseURL.Host}");
                report.ReportServerUrl = baseURL;
                report.ReportPath = reportPath;
                //report.Timeout = Timeout;

                BuildParameter(reportParameters, report);


                var result = await Task.FromResult(report.Render(outputFormat));

                return result;
            }
            catch
            {
                throw;
            }
            //return await client.GetByteArrayAsync(url);

        }

        public async Task<byte[]> RenderLocal(string reportName, string reportPath, List<RequestDataSet> dataSource, NameValueCollection reportParameters, string outputFormat = "PDF")
        {
            try
            {

                string currentReport = reportPath.HasElement() ? $"{reportPath}.{reportName}.rdlc"
                     : $"{Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), "Reports")}\\{reportName}.rdlc";

                //using var reportDefinition = Assembly.GetExecutingAssembly().GetManifestResourceStream("Reports.RTP-Prouct-001.rdlc");

                LocalReport report = new();
                report.ReportPath = currentReport;
                dataSource.ForEach(ds => { report.DataSources.Add(new ReportDataSource { Name = ds.Name, Value = ds.Value }); });

                BuildParameterLocal(reportParameters, report);

                byte[] result = await Task.FromResult(report.Render(outputFormat));

                return result;
            }
            catch
            {
                throw;
            }
            //return await client.GetByteArrayAsync(url);

        }

        /// <summary>
        /// Build the URL for the report
        /// </summary>
        /// <param name="reportPath">Path to the SSRS report</param>
        /// <param name="reportParameters">Collection of parameters for the report</param>
        /// <param name="outputFormat">Output format e.g PDF or EXCEL</param>
        /// <returns>Returns the url for rendering the report</returns>
        private string BuildFullRenderUrl(string reportPath, NameValueCollection reportParameters, string outputFormat)
        {
            string parameterString = BuildParameterQueryString(reportParameters);
            string fullPath = $"{_serverUrl}?{reportPath}&rs:Command=Render{parameterString}&rs:Format={outputFormat}";

            return fullPath;
        }

        /// <summary>
        /// Builds the query string from the supplied parameters
        /// </summary>
        /// <param name="reportParameters">Collection of parameters for the report</param>
        /// <returns>A string of the parameters</returns>
        private string BuildParameterQueryString(NameValueCollection reportParameters)
        {
            string parameterString = string.Empty;

            var items = reportParameters.AllKeys.SelectMany(reportParameters.GetValues, (k, v) => (key: k, value: v));
            foreach (var item in items)
            {
                if (!string.IsNullOrEmpty(item.value)) parameterString += $"&{item.key}={item.value}";
                if (string.IsNullOrEmpty(item.value)) parameterString += $"&{item.key}:isnull=true";
            }

            return parameterString;
        }

        private static void BuildParameter(NameValueCollection reqParams, ServerReport report)
        {
            List<ReportParameter> parameters = new();

            List<ReportParameterInfo> parametersReport = report.GetParameters().ToList();

            var items = reqParams.AllKeys.SelectMany(reqParams.GetValues, (k, v) => (key: k, value: v));

            for (int i = 0; i < parametersReport.Count; i++)
            {
                ReportParameterInfo paramReport = parametersReport[i];
                var findReportRequest = items.FirstOrDefault(o => o.key?.ToUpper() == paramReport?.Name.ToUpper());
                parameters.Add(new ReportParameter(paramReport.Name, findReportRequest.value ?? null));

            }

            report.SetParameters(parameters);
            //return parameterString;
        }
        private static void BuildParameterLocal(NameValueCollection reqParams, LocalReport report)
        {
            List<ReportParameter> parameters = new();

            List<ReportParameterInfo> parametersReport = report.GetParameters().ToList();

            var items = reqParams.AllKeys.SelectMany(reqParams.GetValues, (k, v) => (key: k, value: v));

            for (int i = 0; i < parameters.Count; i++)
            {
                ReportParameterInfo paramReport = parametersReport[i];
                var findReportRequest = items.FirstOrDefault(o => o.key?.ToUpper() == paramReport?.Name.ToUpper());
                if (findReportRequest.value == null) continue;

                parameters.Add(new ReportParameter(paramReport.Name, findReportRequest.value ?? null));

            }

            report.SetParameters(parameters);
            //return parameterString;
        }
    }

    public class SSRS
    {
        public string Host { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Path { get; set; }
    }


}
