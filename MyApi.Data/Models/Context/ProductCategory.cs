using System;
using System.Collections.Generic;

#nullable disable

namespace MyApi.Data.Models.Context
{
    public partial class ProductCategory
    {
        public int Id { get; set; }
        public string CategoryNameTh { get; set; }
        public string CategoryNameEn { get; set; }
        public int ParentCategoryId { get; set; }
        public DateTime CreatedDt { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? ModifiedDt { get; set; }
        public string ModifiedBy { get; set; }
    }
}
