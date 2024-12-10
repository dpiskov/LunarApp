using LunarApp.Data.Models;
using LunarApp.Data.Repository.Interfaces;
using LunarApp.Services.Data.Interfaces;
using LunarApp.Web.ViewModels.Admin.UserManagement;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace LunarApp.Services.Data
{
    public class UserService(
        UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole<Guid>> roleManager,
        IRepository<Notebook, Guid> notebookRepository,
        IRepository<Folder, Guid> folderRepository,
        IRepository<Note, Guid> noteRepository,
        IRepository<Tag, Guid> tagRepository
        ) : IUserService
    {
        public async Task<IEnumerable<AllUsersViewModel>> GetAllUsersAsync()
        {
            IEnumerable<ApplicationUser> allUsers = await userManager.Users.ToArrayAsync();
            ICollection<AllUsersViewModel> allUsersViewModel = new List<AllUsersViewModel>();

            foreach (ApplicationUser user in allUsers)
            {
                IEnumerable<string> roles = await userManager.GetRolesAsync(user);

                allUsersViewModel.Add(new AllUsersViewModel()
                {
                    Id = user.Id.ToString(),
                    Email = user.Email,
                    Roles = roles
                });
            }

            return allUsersViewModel;
        }

        public async Task<bool> UserExistsByIdAsync(Guid userId)
        {
            ApplicationUser? user = await userManager
                .FindByIdAsync(userId.ToString());

            return user != null;
        }

        public async Task<bool> AssignUserToRoleAsync(Guid userId, string roleName)
        {
            ApplicationUser? user = await userManager
                .FindByIdAsync(userId.ToString());
            string normalizedRoleName = roleName.ToUpperInvariant();
            bool roleExists = await roleManager.RoleExistsAsync(normalizedRoleName);

            if (user == null || roleExists == false)
            {
                return false;
            }

            bool alreadyInRole = await userManager.IsInRoleAsync(user, roleName);
            if (alreadyInRole == false)
            {
                IdentityResult? result = await userManager
                    .AddToRoleAsync(user, roleName);

                if (result.Succeeded == false)
                {
                    return false;
                }
            }

            return true;
        }

        public async Task<bool> RemoveUserRoleAsync(Guid userId, string roleName)
        {
            ApplicationUser? user = await userManager
                .FindByIdAsync(userId.ToString());
            bool roleExists = await roleManager.RoleExistsAsync(roleName);

            if (user == null || roleExists == false)
            {
                return false;
            }

            bool alreadyInRole = await userManager.IsInRoleAsync(user, roleName);
            if (alreadyInRole)
            {
                IdentityResult? result = await userManager
                    .RemoveFromRoleAsync(user, roleName);

                if (result.Succeeded == false)
                {
                    return false;
                }
            }

            return true;
        }

        public async Task<bool> DeleteUserAsync(Guid userId)
        {
            ApplicationUser? user = await userManager.FindByIdAsync(userId.ToString());
            if (user == null)
            {
                return false;
            }

            IQueryable<Note> notes = noteRepository.GetAllAttached().Where(n => n.UserId == userId);
            await noteRepository.DeleteRangeAsync(notes);

            IQueryable<Folder> folders = folderRepository.GetAllAttached().Where(f => f.Notebook.UserId == userId);
            await folderRepository.DeleteRangeAsync(folders);

            IQueryable<Notebook> notebooks = notebookRepository.GetAllAttached().Where(n => n.UserId == userId);
            await notebookRepository.DeleteRangeAsync(notebooks);

            IQueryable<Tag> tags = tagRepository.GetAllAttached()
                .Where(t => t.UserId == userId);
            await tagRepository.DeleteRangeAsync(tags);

            IdentityResult result = await userManager.DeleteAsync(user);
            if (result.Succeeded)
            {
                await noteRepository.SaveChangesAsync();
                await folderRepository.SaveChangesAsync();
                await notebookRepository.SaveChangesAsync();
                await tagRepository.SaveChangesAsync();  

                return true;
            }

            return false;
        }
    }
}
