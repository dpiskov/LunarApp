using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using static LunarApp.Common.ApplicationConstants;

namespace LunarApp.Web.Areas.Admin.Controllers
{
    /// <summary>
    /// Controller responsible for handling the home page for admin users.
    /// </summary>
    /// <remarks>
    /// The <see cref="HomeController"/> is responsible for handling requests to the home page for users with admin roles. 
    /// It provides actions to display the home page and other admin-specific functionalities.
    /// The controller is protected by role-based authorization, allowing only users with the role specified by <see cref="AdminRoleName"/>.
    /// </remarks>
    /// <seealso cref="AdminRoleName"/>
    [Area(AdminRoleName)]
    [Authorize(Roles = AdminRoleName)]
    public class HomeController : Controller
    {
        /// <summary>
        /// Displays the home page for admin users.
        /// </summary>
        /// <returns>The view representing the home page.</returns>
        public IActionResult Index()
        {
            return View();
        }
    }
}
