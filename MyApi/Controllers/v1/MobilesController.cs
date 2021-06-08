using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyApi.Core.Share;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyApi.Controllers.v1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("v{version:apiVersion}/mobiles")]
    public class MobilesController : ControllerBase
    {

        public MobilesController()
        {

        }

        [HttpPost("register/device")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> PostRegisterMobile([FromBody]string requestModel)
        {

            try
            { 
                return Ok(requestModel);
            }
            catch (Exception e)
            {
                // logger.LogDebug($"Error: {(e.InnerException != null ? e.InnerException.Message : e.Message)}");
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
