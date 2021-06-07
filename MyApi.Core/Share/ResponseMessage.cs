using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApi.Core.Share
{
    public class ResponseMessageLocale
    {
        public string Code { get; set; }
        public LocaleViewModel Messages { get; set; }
        public object Results { get; set; }
    }

    public class LocaleViewModel
    {
        public string Th { get;set }
        public string En { get; set; }
    }
}
