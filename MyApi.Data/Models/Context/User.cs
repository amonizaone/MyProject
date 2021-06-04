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
        public DateTime? CreatedDt { get; set; }
        public string CreatedBy { get; set; }
    }
}
