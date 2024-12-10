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
    [Area(AdminRoleName)]
    [Authorize(Roles = AdminRoleName)]
    public class UserManagementController(
        IUserService userService,
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager)
        : BaseController
    {
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            IEnumerable<AllUsersViewModel> allUsers = await userService.GetAllUsersAsync();
            return View(allUsers);
        }

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

        public async Task<IActionResult> DeleteUser(Guid userId)
        {
            await userService.DeleteUserAsync(userId);

            return RedirectToAction("Index");
        }
    }
}
