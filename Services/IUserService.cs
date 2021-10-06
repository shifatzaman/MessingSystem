using MessingSystem.Domain;
using MessingSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MessingSystem.Services
{
    public partial interface IUserService
    {
        User GetUserByEmail(string email);

        (bool, string) HasPermission(int userId, string permissionType);

        int AddUser(UserViewModel model);

        User GetUserById(int userId);

        void UpdateUser(User user);

        int GetMemberId(int userId);
    }
}
