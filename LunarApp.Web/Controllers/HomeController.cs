using Microsoft.AspNetCore.Mvc;

namespace LunarApp.Web.Controllers
{
    public class HomeController(ILogger<HomeController> logger) : Controller
    {
        private readonly ILogger<HomeController> _logger = logger;

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult AccessDenied(string returnUrl)
        {
            return RedirectToAction("Error", "Home", new { statusCode = 403, returnUrl });
        }

        public IActionResult Error(int? statusCode)
        {
            if (statusCode.HasValue == false)
            {
                return View("Error");
            }

            if (statusCode == 404)
            {
                return View("Error404");
            }
            else if (statusCode == 401 || statusCode == 403)
            {
                return View("Error403");
            }

            return View("Error500");
        }
    }
}
