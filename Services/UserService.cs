using dotnetcondapackage.Entities;
using dotnetcondapackage.Helpers;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Linq;
using System.IdentityModel.Tokens.Jwt;
using MongoDB.Driver;
using dotnetcondapackage.Models;
using MongoDB.Driver.Core.Clusters;

namespace dotnetcondapackage.Services
{
    public interface IUserService
    {
        User Authenticate(string username, string password);
        IEnumerable<User> GetAllUser();
        User GetUserById(int id);

        public List<Dataset> getUserRoleDataset(int id);
        bool AuthenrticateAdmin(int id);
    }

    public class UserService : IUserService
    {

        private readonly IMongoCollection<User> _users;
        private readonly IMongoCollection<Role> _roles;
        private readonly IMongoCollection<UserRole> _userRoles;
        private readonly IMongoCollection<RoleDataset> _roleDataset;
        private readonly IMongoCollection<Dataset> _dataset;
        private readonly AppSettings _appSettings;

        public UserService(IDatabaseSettings settings, IOptions<AppSettings> appSettings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _users = database.GetCollection<User>("dbo.User");
            _roles = database.GetCollection<Role>("dbo.Role");
            _userRoles = database.GetCollection<UserRole>("dbo.UserRole");
            _roleDataset = database.GetCollection<RoleDataset>("dbo.RoleDataset");
            _dataset = database.GetCollection<Dataset>("dbo.Dataset");
            _appSettings = appSettings.Value;
        }

        // users hardcoded for simplicity, store in a db with hashed passwords in production applications
        /* private List<User> _users = new List<User>
         {
             new User { Id = 1, FirstName = "Admin", LastName = "User", Username = "admin", Password = "admin", Role = Role.Admin },
             new User { Id = 2, FirstName = "Normal", LastName = "User", Username = "user", Password = "user", Role = Role.User }
         }; */


        public User Authenticate(string username, string password)
        {

            // var user = _users.Find(x => x.Username == username && x.Password == password).SingleOrDefault();
            var user = _users.Find(x => x.Username == username).SingleOrDefault();


            // return null if user not found
            if (user == null)
                return null;

            // authentication successful so generate jwt token
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.UserId.ToString()),
                    //new Claim("numberORoles", ""),
                   //https://www.c-sharpcorner.com/article/jwt-json-web-token-authentication-in-asp-net-core/
                 //   var roleIdList =  getUserRoles(user.UserId)
             
        }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            user.Token = tokenHandler.WriteToken(token);

            return user.WithoutPassword();
        }

        bool IUserService.AuthenrticateAdmin(int id)
        {
            bool isAdmin = false;
            var result = from o in _users.AsQueryable()
                         join i in _userRoles.AsQueryable()
                         on o.UserId equals i.UserId
                         join r in _roles.AsQueryable()
                         on i.RoleId equals r.RoleId
                         where (o.UserId== id && r.RoleName=="Admin" )
                         select new
                         {
                             _Id = o.UserId
                         };

            int count = result.Count();

            if (count > 0)
            {
                isAdmin = true;
            }
            return isAdmin;
        }

        private List<UserRole> getUserRoles(int id)
        {

            var userroles = _userRoles.Find(x => x.UserId == id).ToList();
            return userroles;
        }

        public List<Dataset> getUserRoleDataset(int id)
        {
            var result = from ur in _userRoles.AsQueryable()
                         join rd in _roleDataset.AsQueryable()
                         on ur.RoleId equals rd.RoleId
                         join d in _dataset.AsQueryable()
                         on rd.DatasetId equals d.DatasetId
                         where (ur.UserId ==id)
                         select new Dataset
                         {
                             SchemaDatasetName = d.Schema+"."+d.DatasetName
                         };
          return result.ToList();
        }

        IEnumerable<User> IUserService.GetAllUser()
        {
            var user = _users.Find(Builders<User>.Filter.Empty).ToList();
            return user.WithoutPasswords();
        }

        User IUserService.GetUserById(int id)
        {
            var user = _users.Find(x => x.UserId == id).SingleOrDefault();
            return user.WithoutPassword();
        }
    }
}