using LunarApp.Data.Models;
using LunarApp.Data.Repository.Interfaces;
using LunarApp.Services.Data.Interfaces;
using LunarApp.Web.ViewModels.Admin.UserManagement;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace LunarApp.Services.Data
{
    /// <summary>
    /// Service for managing users, including retrieving user details, assigning roles, and deleting users.
    /// </summary>
    /// <remarks>
    /// The <see cref="UserService"/> class provides operations for managing user-related activities such as retrieving 
    /// user information, assigning roles, and managing users' associated entities like notebooks, folders, notes, and tags.
    /// It interacts with the <see cref="UserManager{ApplicationUser}"/>, <see cref="RoleManager{IdentityRole{Guid}}"/>, 
    /// and various repositories for CRUD operations on <see cref="Notebook"/>, <see cref="Folder"/>, <see cref="Note"/>, and 
    /// <see cref="Tag"/> entities.
    /// </remarks>
    /// <param name="userManager">The <see cref="UserManager{ApplicationUser}"/> for managing users.</param>
    /// <param name="roleManager">The <see cref="RoleManager{IdentityRole{Guid}}"/> for managing user roles.</param>
    /// <param name="notebookRepository">The repository for interacting with <see cref="Notebook"/> entities.</param>
    /// <param name="folderRepository">The repository for interacting with <see cref="Folder"/> entities.</param>
    /// <param name="noteRepository">The repository for interacting with <see cref="Note"/> entities.</param>
    /// <param name="tagRepository">The repository for interacting with <see cref="Tag"/> entities.</param>
    public class UserService(
        UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole<Guid>> roleManager,
        IRepository<Notebook, Guid> notebookRepository,
        IRepository<Folder, Guid> folderRepository,
        IRepository<Note, Guid> noteRepository,
        IRepository<Tag, Guid> tagRepository
        ) : IUserService
    {
        /// <summary>
        /// Retrieves all users, including their email addresses and assigned roles.
        /// </summary>
        /// <returns>A task representing the asynchronous operation, with a collection of all users.</returns>
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

        /// <summary>
        /// Checks if a user exists by their ID.
        /// </summary>
        /// <param name="userId">The ID of the user to check.</param>
        /// <returns>A task representing the asynchronous operation, with a result indicating whether the user exists.</returns>
        public async Task<bool> UserExistsByIdAsync(Guid userId)
        {
            ApplicationUser? user = await userManager
                .FindByIdAsync(userId.ToString());

            return user != null;
        }

        /// <summary>
        /// Assigns a role to a user.
        /// </summary>
        /// <param name="userId">The ID of the user to assign the role to.</param>
        /// <param name="roleName">The name of the role to assign.</param>
        /// <returns>A task representing the asynchronous operation, with a result indicating whether the assignment was successful.</returns>
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

        /// <summary>
        /// Removes a role from a user.
        /// </summary>
        /// <param name="userId">The ID of the user to remove the role from.</param>
        /// <param name="roleName">The name of the role to remove.</param>
        /// <returns>A task representing the asynchronous operation, with a result indicating whether the removal was successful.</returns>
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

        /// <summary>
        /// Deletes a user and all related data (notes, folders, notebooks, and tags).
        /// </summary>
        /// <param name="userId">The ID of the user to delete.</param>
        /// <returns>A task representing the asynchronous operation, with a result indicating whether the deletion was successful.</returns>
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
