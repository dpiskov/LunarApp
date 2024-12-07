using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using static LunarApp.Common.ApplicationConstants;

namespace LunarApp.Web.Areas.Admin.Controllers
{
    [Area(AdminRoleName)]
    [Authorize(Roles = AdminRoleName)]
    public class HomeController : Controller
    {
        [Area(AdminRoleName)]
        [Authorize(Roles = AdminRoleName)]
        public IActionResult Index()
        {
            return View();
        }
    }
}
