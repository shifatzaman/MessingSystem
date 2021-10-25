using MessingSystem.Data;
using MessingSystem.Domain;
using MessingSystem.Enums;
using MessingSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MessingSystem.Services
{
    public partial class UserService : IUserService
    {
        private readonly AppDbContext dbContext;
        public UserService(AppDbContext appDbContext)
        {
            dbContext = appDbContext;
        }
        public User GetUserByEmail(string email)
        {
            return dbContext.Users.Where(u => u.Email.Trim().ToLower() == email.Trim().ToLower()).FirstOrDefault();

        }

        public User GetUserById(int userId )
        {
            return dbContext.Users.Where(u => u.UserId == userId).FirstOrDefault();

        }

        public int GetMemberId(int userId)
        {
            return dbContext.MessMembers.Where(m => m.UserId == userId).Select(m => m.Id).FirstOrDefault();
        }

        public int AddUser(UserViewModel model)
        {
            if (model == null)
                return 0;

            var user = new User();
            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.Email = model.Email;
            user.Role = model.Role;
            byte[] passwordHash, passwordSalt;

            CommonUtilities.CreatePasswordHash(model.Password, out passwordHash, out passwordSalt);

            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            dbContext.Users.Add(user);
            dbContext.SaveChanges();

            return user.UserId;
        }


        public void UpdateUser(User user)
        {
            dbContext.Users.Update(user);
            dbContext.SaveChanges();
        }
        public (bool, string) HasPermission(int userId, string permissionType)
        {
            var user = dbContext.Users.Where(u => u.UserId == userId).FirstOrDefault();

            if (user == null)
                return (false, "User not found");

            if (user.Role == (int)UserRoles.Manager || user.Role == (int)UserRoles.Admin)
                return (true, "Access Granted");
            else
            {
                return Constants.AllowedPermissonsForMembers.Contains(permissionType) ?
                    (true, "Access Granted") :
                    (false, "Access Denied");
            }

        }
    }
}
