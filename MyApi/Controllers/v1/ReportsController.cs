using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Logging;
using MyApi.Core;
using MyApi.Data.Models.Report;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace MyApi.Controllers.v1
{
    //[Route("api/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    [Route("v{version:apiVersion}/report")]
    public class ReportsController : ControllerBase
    {
        #region field
        private readonly ISSRSRender _ssrs;


        #endregion field
        public ReportsController()
        {
            _ssrs = new SSRSRender(new SSRS()
            {
                Host = "http://localhost/reportserver",
                Username = "",
                Password = "",
                Path = "/MyReport",
            });
        }

        [HttpGet("export")]
        //[Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> ExportAttachmentFile(string reportName, string format = "pdf")
        {
            try
            {

                var reportServer = _ssrs.GetReportSetting();

                NameValueCollection paramsColection = HttpUtility.ParseQueryString(HttpContext.Request.QueryString.ToString());
                paramsColection.Remove("reportName");
                paramsColection.Remove("format");

                string fileName = $"Export-{reportName}{DateTime.Now:yyyyMMddHHmm}.{format}";
                var IsfileContent = new FileExtensionContentTypeProvider().TryGetContentType(fileName, out string contentType);
                var result = await _ssrs.Render($"{reportServer.Path}/{reportName}", paramsColection, format);
                //UTF8Encoding utf8 = new UTF8Encoding(true, true);

                //Byte[] bytes = new Byte[utf8.GetByteCount(s) + utf8.GetPreamble().Length];


                // Encoding.GetString(result, 0,result.Length);

                // RPT-Product-0001.rdl

                List<string> listType = new List<string> { "HTML4.0", "HTML5", "MHTML" };
                if (listType.Contains(format))
                {
                    var content = Encoding.UTF8.GetString(result);
                    return Ok(new
                    {

                        Content = content

                    });
                }
                else
                {
                    return File(result, $"{contentType}", fileName);
                }



                /// HTML4.0 / HTML5 / MHTML
            }
            catch (Exception e)
            {
                throw e;
            }

        }

        [HttpGet("internal/export")]
        //[Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        //[SwaggerResponseExample(200, typeof(File))]
        public async Task<ActionResult> ExportLocalReport(string reportName, string format = "pdf")
        {
            try
            {
                var reportServer = _ssrs.GetReportSetting();

                NameValueCollection paramsColection = HttpUtility.ParseQueryString(HttpContext.Request.QueryString.ToString());
                paramsColection.Remove("reportName");
                paramsColection.Remove("format");

                string fileName = $"Export-{reportName}{DateTime.Now:yyyyMMddHHmm}.{format}";
                var IsfileContent = new FileExtensionContentTypeProvider().TryGetContentType(fileName, out string contentType);


                List<ProductItem> products = new()
                {
                    new ProductItem { Description = "Widget 6000", Price = 100, Qty = 1 },
                    new ProductItem { Description = "Gizmo MAX", Price = 100, Qty = 25 }
                };


                var result = await _ssrs.RenderLocal(reportName, null, new List<RequestDataSet> {
                    new RequestDataSet{
                        Name ="Product",
                        Value= Extention.CreateDataTable(products)
                    },
                }, paramsColection, format);

                return File(result, $"{contentType}", fileName);

            }
            catch (Exception e)
            {
                throw;
            }

        }


    }
}
