using System;
using System.Collections.Generic;

#nullable disable

namespace MyApi.Data.Models.Context
{
    public partial class User
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string Platform { get; set; }
        public string MobileNo { get; set; }
        public string PhoneNo { get; set; }
        public string FaxNo { get; set; }
        public string Address { get; set; }
        public DateTime? CreatedDt { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? ModifiedDt { get; set; }
        public string ModifiedBy { get; set; }
    }
}
