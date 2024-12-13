using Microsoft.AspNetCore.Mvc;

namespace LunarApp.Web.Controllers
{
    /// <summary>
    /// Controller responsible for handling requests to the home page, error pages, and access denial scenarios.
    /// </summary>
    /// <remarks>
    /// The <see cref="HomeController"/> handles requests related to the home page, error handling, and access denial. 
    /// It is responsible for logging messages for debugging or monitoring purposes. The controller interacts with the 
    /// <see cref="ILogger{HomeController}"/> for logging and provides users with appropriate views based on the request.
    /// </remarks>
    /// <param name="logger">An instance of <see cref="ILogger{HomeController}"/> used to log messages for this controller.</param>
    public class HomeController(ILogger<HomeController> logger) : Controller
    {
        private readonly ILogger<HomeController> _logger = logger;

        /// <summary>
        /// Handles the request to the home page.
        /// </summary>
        /// <returns>The view for the home page.</returns>
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Redirects to an error page when access is denied.
        /// </summary>
        /// <param name="returnUrl">The URL that the user attempted to access before being denied.</param>
        /// <returns>A redirect to the error page with a 403 status code.</returns>
        public IActionResult AccessDenied(string returnUrl)
        {
            return RedirectToAction("Error", "Home", new { statusCode = 403, returnUrl });
        }

        /// <summary>
        /// Displays an error page based on the status code.
        /// </summary>
        /// <param name="statusCode">The HTTP status code that indicates the error.</param>
        /// <returns>A view displaying the error page corresponding to the provided status code.</returns>
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
