using System.Collections.Generic;
using System.Collections.Specialized;
using System.Threading.Tasks;

namespace MyApi.Core
{
    public interface ISSRSRender
    {
        Task<byte[]> RenderReport(string reportPath, NameValueCollection reportParameters, string outputFormat = "PDF");
        Task<byte[]> Render(string reportPath, NameValueCollection reportParameters, string outputFormat = "PDF");

        Task<byte[]> RenderLocal(string reportName, string reportPath, List<RequestDataSet> dataSource, NameValueCollection reportParameters, string outputFormat = "PDF");
        SSRS GetReportSetting();
    }
}