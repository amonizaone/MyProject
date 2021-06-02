using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApi.Core.Report
{
    public class RequestParameter
    {
        public string Name { get; set; }
        public object Value { get; set; }
    }

    public class RequestDataSet
    {
        public string Name { get; set; }
        public object Value { get; set; }
    }
}
