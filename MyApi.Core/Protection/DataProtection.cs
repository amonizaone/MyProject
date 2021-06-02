using Microsoft.AspNetCore.DataProtection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApi.Core
{
    public class DataProtection
    {
        private readonly IDataProtectionProvider
          _dataProtectionProvider;
        public DataProtection(IDataProtectionProvider
            dataProtectionProvider)
        {
            _dataProtectionProvider = dataProtectionProvider;
        }
        public RepsonseProtection Encrypt(string textToEncrypt, string key)
        {
            try
            {
                return new RepsonseProtection
                {
                    IsSuccess = true,
                    Value = _dataProtectionProvider.CreateProtector(key).Protect(textToEncrypt)
                };
            }
            catch (Exception e)
            {
                return new RepsonseProtection
                {
                    IsSuccess = false,
                    Value = e.Message
                };
            }
        }
        public RepsonseProtection Decrypt(string cipherText, string key)
        {
            try
            {
                return new RepsonseProtection
                {
                    IsSuccess = true,
                    Value = _dataProtectionProvider.CreateProtector(key).Unprotect(cipherText)
                };
            }
            catch (Exception e)
            {

                return new RepsonseProtection
                {
                    IsSuccess = false,
                    Value = e.Message
                };
            }
        }
    }

    public class RepsonseProtection
    {
        public bool IsSuccess { get; set; }
        public string Value { get; set; }
    }
}
