using System.Diagnostics;

using Microsoft.AspNetCore.Mvc;

using LunarApp.Web.ViewModels;

namespace LunarApp.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        //[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error(int? statusCode = null)
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

            //return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
