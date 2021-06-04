using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApi.Core.Authorization
{
    public class JwtMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly AppSetting _appSetting;


        public JwtMiddleware(RequestDelegate next, IOptions<AppSetting> appSetting)
        {
            _next = next;
            _appSetting = appSetting;
        }

        public async Task Invoke(HttpContext context,)
    }
}
