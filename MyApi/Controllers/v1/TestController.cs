using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MyApi.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace MyApi.Controllers.v1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("v{version:apiVersion}/api/test")]
    public class TestController : ControllerBase
    {

        private readonly ILogger<TestController> _logger;
        private readonly IHttpClientFactory _httpClientFactory;
        public TestController(ILogger<TestController> logger, IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _httpClientFactory = httpClientFactory;


        }
        // POST api/<UserController>
        [HttpPost("query/type")]
        public IActionResult PostQueryStorageType(object mode)
        {
            RestClientHelper restClient = new RestClientHelper(_httpClientFactory);

            //logistics_interface ={ "weight":"1","destAreaCode":"KALIDERES","sendSiteCode":"JAKARTA","feeType":"CHARGE","productType":"EZ"}

            //key = 74e88159b508593ffebf3ff392cf2a50

            //  data_digest =Base64.encodeBase64(  MD5Util.code32(logistics_interface + key, "UTF-8").getBytes("UTF-8")  );

            var interfaceLogistic = new
            {
                eccompanyid = "INTEREXPRESS",
                msg_type = "SYSQUERYSTORAGETYPE",
                logistics_interface = new
                {
                    customerid = "VIP111122022"
                }
            };

            string keyx = "bde12510ec749188a65da066db8f47ef";
            var xjon = mode.SerializeObject();
            string toHash = xjon + keyx;
            string data_digest;

            using (MD5 md5 = MD5.Create())
            {
                data_digest = Extention.Encode(string.Join(string.Empty, md5.ComputeHash(Encoding.UTF8.GetBytes(toHash)).Select(b => b.ToString("x2"))));
            }

            var rx = GetStrMd5_32X(toHash);
            var url = "https://jtpay-uat.jtexpress.co.th/thailand-ifd-web/normal/track/tracking.do";
            var requestForm = new
            {
                eccompanyid = "INTEREXPRESS",
                data_digest = data_digest,
                msg_type = "TRACKQUERY",
                logistics_interface = Extention.DeserializeObject<object>(xjon)
            };

            var result = restClient.PostAsync(url, requestForm);
            return Ok(result);
        }

        public static string GetStrMd5_32X(string ConvertString)
        {
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            string t2 = BitConverter.ToString(md5.ComputeHash(Encoding.UTF8.GetBytes(ConvertString)));
            t2 = t2.Replace("-", "");
            return t2.ToLower();
        }
    }
}
