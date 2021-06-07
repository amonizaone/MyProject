using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApi.Core.Helpers
{
    public class AppSettings
    {
        public string Secret { get; set; }
        public LineSetting LineSetting { get; set; }
    }

    public class LineSetting
    {
        public string RequestUrl { get; set; }
        public string ClientId { get; set; }
        public string RedirectUri { get; set; }
        public string ClientSecret { get; set; }
    }
}
