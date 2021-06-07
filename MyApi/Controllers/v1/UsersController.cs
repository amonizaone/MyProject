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
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // POST api/<UserController>
        [HttpPost("auth/login")]
        public IActionResult PostUserLogin([FromBody] UserAuthModel userAuth)
        {
            var user =_userService.Authenticate(userAuth.Username, userAuth.Password);
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
    }
}
