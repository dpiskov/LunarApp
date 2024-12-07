using LunarApp.Data.Models;
using LunarApp.Services.Data.Interfaces;
using LunarApp.Web.ViewModels.Admin.UserManagement;
using Microsoft.AspNetCore.Identity;

namespace LunarApp.Services.Data
{
    public class UserService(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole<Guid>> roleManager)
        : BaseService, IUserService
    {
        public Task<IEnumerable<AllUsersViewModel>> GetAllUsersAsync()
        {
            throw new NotImplementedException();
        }

        public Task<bool> UserExistsByIdAsync(Guid userId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> AssignUserToRoleAsync(Guid userId, string roleName)
        {
            throw new NotImplementedException();
        }

        public Task<bool> RemoveUserRoleAsync(Guid userId, string roleName)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteUserAsync(Guid userId)
        {
            throw new NotImplementedException();
        }
    }
}
