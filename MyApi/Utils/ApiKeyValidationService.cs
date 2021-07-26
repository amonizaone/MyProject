using JN.Authentication.HelperClasses;
using JN.Authentication.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace iel.imot.Helper
{
    public class ApiKeyValidationService : IApiKeyValidationService
    {
        private readonly string _errorKey = "1234";
        private readonly string _validKey = "123";
        private readonly string _errorMsg = "An error has occured";
        private readonly string _exceptionKey = "exception";

        public Task<ValidationResult> ValidateApiKey(string apiKey)
        {
            if (apiKey == _exceptionKey)
            {
                throw new ArgumentException("specified key causes an exception");
            }


            var claims = new[]
            {
                new Claim(ClaimTypes.GivenName, "Jose Teste"),
                new Claim(ClaimTypes.Name, "user1"),
                new Claim(ClaimTypes.Email, "email@email.com"),
                new Claim("IsAdmin", true.ToString()),
                new Claim(ClaimTypes.Role, "role1;role2")
            };

            var res = new ValidationResult
            {
                Success = (apiKey == _validKey),
                ErrorDescription = (apiKey == _errorKey) ? _errorMsg : "",
                ErrorCode = (apiKey == _errorKey) ? -1 : 0,
                Claims = claims
            };

            return Task.FromResult(res);
        }


    }
}
