using System;
using System.Collections.Generic;

#nullable disable

namespace MyApi.Data.Models.Context
{
    public partial class Product
    {
        public int Id { get; set; }
        public string ProductCode { get; set; }
        public string ProductNameTh { get; set; }
        public string ProductNameEn { get; set; }
        public DateTime? CreatedDt { get; set; }
        public string CreatedBy { get; set; }
    }
}
