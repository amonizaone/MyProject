using MyApi.Data.Models.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApi.Data.Models.View
{
    public class UsersViewModel : User
    {
        public TokenViewModel UserToken { get; set; }
         public List<RoleViewModel> Roles { get; set; }
    }

    public class RoleViewModel
    {
        public string Id { get; set; }
        public string RoleName { get; set; }
    }

    public class TokenViewModel
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public DateTime? ExpiredDt { get; set; }
        public DateTime CreatedBy { get; set; }
        public string CreatedByIp { get; set; }
    }
}
