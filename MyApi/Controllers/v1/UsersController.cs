using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MyApi.Core.Services.Users;
using MyApi.Data.Models.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
 
namespace MyApi.Controllers.v1
{ 
    [ApiController]
    [ApiVersion("1.0")]
    [Route("v{version:apiVersion}/users")]
    public class UsersController : ControllerBase
    {
        private readonly ILogger<UsersController> _logger;
        private readonly IUserService _userService;
        public UsersController(ILogger<UsersController> logger, IUserService userService)
        {
            _logger = logger;
            _userService = userService;
        }
        // GET: api/<UserController>
        [HttpGet]
        //[Authorize]
        [Authorize]
        public IActionResult Get()
        {
            return Ok(new string[] { "value1", "value2" });
        }

        // POST api/<UserController>
        [HttpPost("auth/login")]
        public IActionResult PostUserLogin([FromBody] UserAuthModel userAuth)
        {
            var user =_userService.Authenticate(userAuth.Username, userAuth.Password);
            return Ok(user);
        }

        // POST api/<UserController>
        [HttpGet("auth/me")]
        public IActionResult GetUserLogin()
        {
            var user = _userService.Authenticate("amonizaone@gmail.com", "123456");
            return Ok(user);
        }

        // POST api/<UserController>
        [HttpPost("auth/refresh/token")]
        public IActionResult PostRefreshToken([FromBody] UserAuthModel userAuth)
        {
            var user = _userService.Authenticate(userAuth.Username, userAuth.Password);
            if (user == null)
                return BadRequest(new { message = "Username or password is incorrect" });

            return Ok(user);
        }

        // GET api/<UserController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<UserController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<UserController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<UserController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }

        //[AllowAnonymous]
        //[HttpPost("refresh-token")]
        //public IActionResult RefreshToken()
        //{
        //    var refreshToken = Request.Cookies["refreshToken"];
        //    var response = _userService.RefreshToken(refreshToken, ipAddress());

        //    if (response == null)
        //        return Unauthorized(new { message = "Invalid token" });

        //    setTokenCookie(response.RefreshToken);

        //    return Ok(response);
        //}

        //[HttpPost("revoke-token")]
        //public IActionResult RevokeToken([FromBody] RevokeTokenRequest model)
        //{
        //    // accept token from request body or cookie
        //    var token = model.Token ?? Request.Cookies["refreshToken"];

        //    if (string.IsNullOrEmpty(token))
        //        return BadRequest(new { message = "Token is required" });

        //    var response = _userService.RevokeToken(token, ipAddress());

        //    if (!response)
        //        return NotFound(new { message = "Token not found" });

        //    return Ok(new { message = "Token revoked" });
        //}

        // helper methods

        private void setTokenCookie(string token)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = DateTime.UtcNow.AddDays(7)
            };
            Response.Cookies.Append("refreshToken", token, cookieOptions);
        }

        private string ipAddress()
        {
            if (Request.Headers.ContainsKey("X-Forwarded-For"))
                return Request.Headers["X-Forwarded-For"];
            else
                return HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
        }
    }
}
