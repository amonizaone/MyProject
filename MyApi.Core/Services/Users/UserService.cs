﻿using BCrypt.Net;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MyApi.Core.Helpers;
using MyApi.Data.Models.Context;
using MyApi.Data.Models.View;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MyApi.Core.Services.Users
{
    public class UserService : IUserService
    {
        // users hardcoded for simplicity, store in a db with hashed passwords in production applications
        private List<User> _users = new List<User>
        {
            //new User { Id = 1, FirstName = "Admin", LastName = "User", Username = "admin", Password = "admin", Role = Role.Admin },
            //new User { Id = 2, FirstName = "Normal", LastName = "User", Username = "user", Password = "user", Role = Role.User }
        };

        private readonly AppSettings _appSettings;
        private readonly MyDbContext _dbContext;
        public UserService(IOptions<AppSettings> appSettings, MyDbContext dbContext)
        {
            _appSettings = appSettings.Value;
            _dbContext = dbContext;
        }

        public bool VerifyPassword(string password, string hashPassword)
        {
            return BCrypt.Net.BCrypt.EnhancedVerify(password, hashPassword, hashType: BCrypt.Net.HashType.SHA384);
        }

        public string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.EnhancedHashPassword(password, hashType: BCrypt.Net.HashType.SHA384);
        }

        public User Authenticate(string username, string password)
        {
            var user = _dbContext.Users.FirstOrDefault(x => x.Email == username);
            string hash = HashPassword(password);
            // return null if user not found
            if (user == null)
                return null;

            if (!VerifyPassword(password, user?.Password))
                return null;
            var userView= new UsersViewModel()
            {
                Id = user.Id,
                Email = user.Email,
                MobileNo = user.MobileNo,
                Roles = new List<RoleViewModel> { },
                Platform = user.Platform,
                Name = user.Name,
                Address = user.Address
            };

            // authentication successful so generate jwt token
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Id.ToString()),
                    new Claim(ClaimTypes.Role, "")
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            string refreshKey = Guid.NewGuid().ToString("N");
            var token = tokenHandler.CreateToken(tokenDescriptor);
            userView.UserToken = new TokenViewModel
            {
                Id = DateTime.Now.Second,
                ExpiredDt = tokenDescriptor.Expires,
                UserId= user.Id,
                RefreshToken = refreshKey,
                AccessToken = tokenHandler.WriteToken(token)
            };
            userView.Password = null;
            return userView;
        }

        public IEnumerable<User> GetAll()
        {
            return _users.WithoutPasswords();
        }

        public User GetById(int id)
        {
            var user = _users.FirstOrDefault(x => x.Id == id);
            return user.WithoutPassword();
        }
    }

    public interface IUserService
    {
        User Authenticate(string username, string password);
        IEnumerable<User> GetAll();
        User GetById(int id);
    }
}
