using LunarApp.Data.Models;
using LunarApp.Services.Data.Interfaces;
using LunarApp.Web.Controllers;
using LunarApp.Web.ViewModels.Admin.UserManagement;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using static LunarApp.Common.ApplicationConstants;

namespace LunarApp.Web.Areas.Admin.Controllers
{
    /// <summary>
    /// Controller responsible for managing user roles and actions in the admin panel.
    /// </summary>
    /// <remarks>
    /// The <see cref="UserManagementController"/> handles the administrative functionalities related to user management.
    /// It allows for managing user roles, retrieving user details, and performing actions such as user creation, 
    /// deletion, and updating within the admin panel. The controller is protected by role-based authorization, 
    /// allowing only users with the role specified by <see cref="AdminRoleName"/> to access its actions.
    /// </remarks>
    /// <seealso cref="AdminRoleName"/>
    /// <param name="userService">An instance of <see cref="IUserService"/> to handle user-related operations such as retrieving details, creating, deleting, and updating users.</param>
    /// <param name="userManager">An instance of <see cref="UserManager{ApplicationUser}"/> to manage user-specific operations like creating, deleting, and updating users in the identity system.</param>
    /// <param name="signInManager">An instance of <see cref="SignInManager{ApplicationUser}"/> to handle user sign-in and authentication-related operations.</param>
    [Area(AdminRoleName)]
    [Authorize(Roles = AdminRoleName)]
    public class UserManagementController(
        IUserService userService,
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager)
        : BaseController
    {
        /// <summary>
        /// Displays a list of all users in the system.
        /// </summary>
        /// <returns>A view with the list of all users.</returns>
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            IEnumerable<AllUsersViewModel> allUsers = await userService.GetAllUsersAsync();
            return View(allUsers);
        }

        /// <summary>
        /// Assigns a role to a specific user.
        /// </summary>
        /// <param name="userId">The ID of the user to assign the role to.</param>
        /// <param name="role">The role to assign to the user.</param>
        /// <returns>Redirects to the Index action.</returns>
        [HttpPost]
        public async Task<IActionResult> AssignRole(string userId, string role)
        {
            Guid userGuid = Guid.Empty;
            if (IsGuidValid(userId, ref userGuid) == false)
            {
                return RedirectToAction(nameof(Index));
            }

            bool userExists = await userService.UserExistsByIdAsync(userGuid);

            if (userExists == false)
            {
                return RedirectToAction(nameof(Index));
            }

            bool assignResult = await userService.AssignUserToRoleAsync(userGuid, role);
            if (assignResult == false)
            {
                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// Removes a role from a specific user.
        /// </summary>
        /// <param name="userId">The ID of the user to remove the role from.</param>
        /// <param name="role">The role to remove from the user.</param>
        /// <returns>Redirects to the Index action.</returns>
        [HttpPost]
        public async Task<IActionResult> RemoveRole(string userId, string role)
        {
            if (role == null)
            {
                return RedirectToAction(nameof(Index));
            }

            Guid userGuid = Guid.Empty;
            if (IsGuidValid(userId, ref userGuid) == false)
            {
                return RedirectToAction(nameof(Index));
            }

            bool userExists = await userService.UserExistsByIdAsync(userGuid);

            if (userExists == false)
            {
                return RedirectToAction(nameof(Index));
            }

            bool removeResult = await userService.RemoveUserRoleAsync(userGuid, role);
            if (removeResult == false)
            {
                return RedirectToAction(nameof(Index));
            }

            ApplicationUser? currentUser = await userManager.GetUserAsync(User);
            IList<string> roles = await userManager.GetRolesAsync(currentUser);

            if (roles.Contains(AdminRoleName) == false)
            {
                await signInManager.SignOutAsync();

                return Redirect("/Identity/Account/Login");
            }

            await signInManager.RefreshSignInAsync(currentUser);

            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// Deletes a user from the system.
        /// </summary>
        /// <param name="userId">The ID of the user to delete.</param>
        /// <returns>Redirects to the Index action after deletion.</returns>
        public async Task<IActionResult> DeleteUser(Guid userId)
        {
            await userService.DeleteUserAsync(userId);

            return RedirectToAction("Index");
        }
    }
}
