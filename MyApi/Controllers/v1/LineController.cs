using iel.line.Models;
using LineMessagingAPI;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MyApi.Core;
using MyApi.Core.Helpers;
using MyApi.Core.Share;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace MyApi.Controllers.v1
{
    [ApiVersion("1.0")]
    [Route("v{version:apiVersion}/line")]
    public class LineController : ControllerBase
    {
        #region field
        private readonly string apiLineMe = "https://notify-api.line.me";
        private static LineMessagingClient lineMessagingClient;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly ILogger<LineController> logger;
        //private readonly IMapper mapper;
        //private readonly InspDbContext inspDbContext;
        private readonly IOptions<AppSettings> options;
        private readonly IHttpClientFactory _httpClientFactory;
        #endregion field

        #region constructor

        public LineController(
            IHttpContextAccessor _httpContextAccessor,
            IHttpClientFactory httpClientFactory,
            ILogger<LineController> _logger,
            //IMapper _mapper,
            //InspDbContext _inspDbContext,
            IOptions<AppSettings> _options
        )
        {
            options = _options;
            //inspDbContext = _inspDbContext;
            _httpClientFactory = httpClientFactory;
            httpContextAccessor = _httpContextAccessor;
            logger = _logger;
            //mapper = _mapper;
            lineMessagingClient = new LineMessagingClient(null);
        }

        #endregion constructor

        [HttpPost("request/token")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> PostRequestToken(RequestLineToken requestModel)
        {

            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new ResponseMessageLocale()
                    {
                        Messages = new LocaleViewModel
                        {
                            Th = "ไม่พบข้อมูลคำขอ",
                            En = "Please input data"
                        }
                    });
                }
                RestClientHelper restClientHelper = new RestClientHelper(_httpClientFactory);
                var requestUrl = Extention.GetUri(Request);
                var lineSetting = options.Value.LineSetting;
                string redirectUrl = lineSetting.RedirectUri;

#if DEBUG
                Console.WriteLine("Mode=Debug");

                Request.Headers.TryGetValue("Origin", out var OrginUrl);
                Uri uri = new Uri(OrginUrl.FirstOrDefault());
                redirectUrl = $"{uri.Scheme}://{uri.Authority}{new Uri(redirectUrl).AbsolutePath}";
                //redirectUrl = $"{HttpRequest Request.Request.Url.Authority}";
#endif
                var result = await restClientHelper.PostAsync($"{lineSetting.RequestUrl}/oauth/token", new
                {
                    client_id = lineSetting.ClientId,
                    client_secret = lineSetting.ClientSecret,
                    code = requestModel.Code,
                    grant_type = requestModel.GrantType,
                    redirect_uri = redirectUrl,
                }, null, "application/x-www-form-urlencoded");
                var response = result.DeserializeObject<object>();

                return Ok(response);
            }
            catch (Exception e)
            {
                logger.LogDebug($"Error: {(e.InnerException != null ? e.InnerException.Message : e.Message)}");
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseMessageLocale()
                {
                    Messages = new LocaleViewModel
                    {
                        Th = "เกิดข้อผิดพลาดจากระบบ กรุณาลองใหม่อีกครั้ง",
                        En = "Something went wrong, please try again."
                    }
                });

            }

        }


        [HttpPost("message/push")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> PostMessage(RequestLineMessage requestModel)
        {

            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new ResponseMessageLocale()
                    {
                        Messages = new LocaleViewModel
                        {
                            Th = "ไม่พบข้อมูลคำขอ",
                            En = "Please input data"
                        }
                    });
                }
                RestClientHelper restClientHelper = new RestClientHelper(_httpClientFactory);

                var lineSetting = options.Value.LineSetting;
                Dictionary<string, string> additionalHeaders = new Dictionary<string, string>
                {
                    { "Authorization", $"Bearer {requestModel.Token}" }
                };
                requestModel.Token = null;
                var result = await restClientHelper.PostAsync($"{apiLineMe}/api/notify", requestModel, additionalHeaders, "application/x-www-form-urlencoded");
                var response = result.DeserializeObject<object>();

                return Ok(response);
            }
            catch (Exception e)
            {
                logger.LogDebug($"Error: {(e.InnerException != null ? e.InnerException.Message : e.Message)}");
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseMessageLocale()
                {
                    Messages = new LocaleViewModel
                    {
                        Th = "เกิดข้อผิดพลาดจากระบบ กรุณาลองใหม่อีกครั้ง",
                        En = "Something went wrong, please try again."
                    }
                });

            }

        }
    }
}
