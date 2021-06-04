using System;
using System.Collections.Generic;

#nullable disable

namespace MyApi.Data.Models.Context
{
    public partial class ProductImage
    {
        public int Id { get; set; }
        public string ImageName { get; set; }
        public string ProductImage1 { get; set; }
        public string ProductImageThumb { get; set; }
        public int? ProductId { get; set; }
        public DateTime CreatedDt { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? ModifiedDt { get; set; }
        public string ModifiedBy { get; set; }
    }
}
