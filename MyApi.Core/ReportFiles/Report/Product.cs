using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApi.Core.Report
{
    public class ProductItem
    {
        public string ProductCode { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int Qty { get; set; }
        public decimal Total => Price * Qty;
    }
}
