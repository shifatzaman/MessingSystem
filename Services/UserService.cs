using MessingSystem.Data;
using MessingSystem.Domain;
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
    }
}
