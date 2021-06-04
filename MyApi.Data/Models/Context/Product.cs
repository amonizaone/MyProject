using System;
using System.Collections.Generic;

#nullable disable

namespace MyApi.Data.Models.Context
{
    public partial class Product
    {
        public int Id { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public string ProductDesc { get; set; }
        public string BarcodeNo { get; set; }
        public int? ProductCategoryId { get; set; }
        public double? ProductWeight { get; set; }
        public decimal ProductPrice { get; set; }
        public string UnlimitedFlag { get; set; }
        public DateTime CreatedDt { get; set; }
        public string CreatedBy { get; set; }
        public double? ProductStrock { get; set; }
        public double? ProductLive { get; set; }
    }
}
