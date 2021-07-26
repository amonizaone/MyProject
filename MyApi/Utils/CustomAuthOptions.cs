using Microsoft.AspNetCore.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyApi.Utils
{
    public class CustomAuthOptions : AuthenticationSchemeOptions
    {
        public string UserInfoEndpoint { get; set; }
    }
}
