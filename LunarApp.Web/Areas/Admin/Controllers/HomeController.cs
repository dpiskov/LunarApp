using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using static LunarApp.Common.ApplicationConstants;

namespace LunarApp.Web.Areas.Admin.Controllers
{
    [Area(AdminRoleName)]
    [Authorize(Roles = AdminRoleName)]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
