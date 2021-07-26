//using System;
//using System.Collections.Generic;
//using System.Text;

//namespace Microsoft.ReportViewer.NETCore.Microsoft.Reporting
//{
//    public static class ReportServiceHelpers
//    {
//        public static ReportExportResult ExportReportToFormat(ReportViewerModel model, ReportFormats format, int? startPage = 0, int? endPage = 0)
//        {
//            return ExportReportToFormat(model, format.GetName(), startPage, endPage);
//        }
//		public static ReportExportResult ExportReportToFormat(ReportViewerModel model, string format, int? startPage = 0, int? endPage = 0)
//		{
//			var definedReportParameters = GetReportParameters(model, true);

//			var url = model.ServerUrl + ((model.ServerUrl.ToSafeString().EndsWith("/")) ? "" : "/") + "ReportExecution2005.asmx";

//			var basicHttpBinding = _initializeHttpBinding(url, model);
//			var service = new ReportServiceExecution.ReportExecutionServiceSoapClient(basicHttpBinding, new System.ServiceModel.EndpointAddress(url));
//			service.ClientCredentials.Windows.AllowedImpersonationLevel = System.Security.Principal.TokenImpersonationLevel.Impersonation;
//			service.ClientCredentials.Windows.ClientCredential = (System.Net.NetworkCredential)(model.Credentials ?? System.Net.CredentialCache.DefaultCredentials);

//			var exportResult = new ReportExportResult();
//			exportResult.CurrentPage = (startPage.ToInt32() <= 0 ? 1 : startPage.ToInt32());
//			exportResult.SetParameters(definedReportParameters, model.Parameters);

//			if (startPage == 0)
//			{
//				startPage = 1;
//			}

//			if (endPage == 0)
//			{
//				endPage = startPage;
//			}

//			var outputFormat = $"<OutputFormat>{format}</OutputFormat>";
//			var encodingFormat = $"<Encoding>{model.Encoding.EncodingName}</Encoding>";
//			var htmlFragment = ((format.ToUpper() == "HTML4.0" && model.UseCustomReportImagePath == false && model.ViewMode == ReportViewModes.View) ? "<HTMLFragment>true</HTMLFragment>" : "");
//			var deviceInfo = $"<DeviceInfo>{outputFormat}{encodingFormat}<Toolbar>False</Toolbar>{htmlFragment}</DeviceInfo>";
//			if (model.ViewMode == ReportViewModes.View && startPage.HasValue && startPage > 0)
//			{
//				if (model.EnablePaging)
//				{
//					deviceInfo = $"<DeviceInfo>{outputFormat}<Toolbar>False</Toolbar>{htmlFragment}<Section>{startPage}</Section></DeviceInfo>";
//				}
//				else
//				{
//					deviceInfo = $"<DeviceInfo>{outputFormat}<Toolbar>False</Toolbar>{htmlFragment}</DeviceInfo>";
//				}
//			}

//			var reportParameters = new List<ReportServiceExecution.ParameterValue>();
//			foreach (var parameter in exportResult.Parameters)
//			{
//				bool addedParameter = false;
//				foreach (var value in parameter.SelectedValues)
//				{
//					var reportParameter = new ReportServiceExecution.ParameterValue();
//					reportParameter.Name = parameter.Name;
//					reportParameter.Value = value;
//					reportParameters.Add(reportParameter);

//					addedParameter = true;
//				}

//				if (!addedParameter)
//				{
//					var reportParameter = new ReportServiceExecution.ParameterValue();
//					reportParameter.Name = parameter.Name;
//					reportParameters.Add(reportParameter);
//				}
//			}

//			var executionHeader = new ReportServiceExecution.ExecutionHeader();

//			ReportServiceExecution.ExecutionInfo executionInfo = null;
//			string extension = null;
//			string encoding = null;
//			string mimeType = null;
//			string[] streamIDs = null;
//			ReportServiceExecution.Warning[] warnings = null;

//			try
//			{
//				string historyID = null;
//				executionInfo = service.LoadReportAsync(model.ReportPath, historyID).Result;
//				executionHeader.ExecutionID = executionInfo.ExecutionID;

//				var executionParameterResult = service.SetReportParameters(executionInfo.ExecutionID, reportParameters.ToArray(), "en-us").Result;

//				if (model.EnablePaging)
//				{
//					var renderRequest = new ReportServiceExecution.Render2Request(format, deviceInfo, ReportServiceExecution.PageCountMode.Actual);
//					var result = service.Render2(executionInfo.ExecutionID, renderRequest).Result;

//					extension = result.Extension;
//					mimeType = result.MimeType;
//					encoding = result.Encoding;
//					warnings = result.Warnings;
//					streamIDs = result.StreamIds;

//					exportResult.ReportData = result.Result;
//				}
//				else
//				{
//					var renderRequest = new ReportServiceExecution.RenderRequest(format, deviceInfo);
//					var result = service.Render(executionInfo.ExecutionID, renderRequest).Result;

//					extension = result.Extension;
//					mimeType = result.MimeType;
//					encoding = result.Encoding;
//					warnings = result.Warnings;
//					streamIDs = result.StreamIds;

//					exportResult.ReportData = result.Result;
//				}

//				executionInfo = service.GetExecutionInfo(executionHeader.ExecutionID).Result;
//			}
//			catch (Exception ex)
//			{
//				Console.WriteLine(ex.Message);
//			}

//			exportResult.ExecutionInfo = executionInfo;
//			exportResult.Format = format;
//			exportResult.MimeType = mimeType;
//			exportResult.StreamIDs = (streamIDs == null ? new List<string>() : streamIDs.ToList());
//			exportResult.Warnings = (warnings == null ? new List<ReportServiceExecution.Warning>() : warnings.ToList());

//			if (executionInfo != null)
//			{
//				exportResult.TotalPages = executionInfo.NumPages;
//			}

//			return exportResult;
//		}
//	}
//}
