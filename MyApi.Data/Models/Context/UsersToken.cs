using System;
using System.Collections.Generic;

#nullable disable

namespace MyApi.Data.Models.Context
{
    public partial class UsersToken
    {
        public int Id { get; set; }
        public string Token { get; set; }
        public DateTime? Expires { get; set; }
        public string ExpiredFlag { get; set; }
        public string IpAddress { get; set; }
        public DateTime? Revoked { get; set; }
        public string RevokedByIp { get; set; }
        public string ReplacedByToken { get; set; }
        public string ActiveStatus { get; set; }
        public DateTime? CreatedDt { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? ModifiedDt { get; set; }
        public string ModifiedBy { get; set; }
    }
}
