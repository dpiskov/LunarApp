using LunarApp.Services.Data.Interfaces;
using LunarApp.Web.Controllers;
using LunarApp.Web.ViewModels.Admin.UserManagement;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static LunarApp.Common.ApplicationConstants;

namespace LunarApp.Web.Areas.Admin.Controllers
{
    [Area(AdminRoleName)]
    [Authorize(Roles = AdminRoleName)]
    public class UserManagementController(IUserService userService) : BaseController
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

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> DeleteUser(string userId)
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

            bool removeResult = await userService.DeleteUserAsync(userGuid);
            if (removeResult == false)
            {
                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
