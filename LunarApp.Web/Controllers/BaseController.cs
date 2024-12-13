using LunarApp.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace LunarApp.Web.Controllers
{
    /// <summary>
    /// A base controller that provides common functionality for all controllers.
    /// Includes methods for retrieving the current user ID and validating GUIDs.
    /// </summary>
    /// <remarks>
    /// The <see cref="BaseController"/> serves as a foundational class for other controllers in the application. 
    /// It includes utility methods such as retrieving the current user ID and validating GUIDs to ensure proper 
    /// operation across the application's controllers. Derived controllers can extend this functionality to perform 
    /// specific business logic while benefiting from these common operations.
    /// </remarks>
    public class BaseController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseController"/> class.
        /// </summary>
        public BaseController()
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseController"/> class with the specified <see cref="UserManager{ApplicationUser}"/>.
        /// </summary>
        /// <param name="userManager">The <see cref="UserManager{ApplicationUser}"/> used to manage users in the system.</param>
        public BaseController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        /// <summary>
        /// Retrieves the ID of the current authenticated user.
        /// </summary>
        /// <returns>The GUID of the current user.</returns>
        /// <exception cref="InvalidOperationException">Thrown if the user ID cannot be retrieved.</exception>
        protected Guid GetCurrentUserId()
        {
            string? userId = _userManager.GetUserId(User);

            if (string.IsNullOrEmpty(userId))
            {
                throw new InvalidOperationException("Unable to retrieve the current user ID.");
            }

            return Guid.Parse(userId);
        }

        /// <summary>
        /// Validates if a given string is a valid GUID and attempts to parse it.
        /// </summary>
        /// <param name="id">The string value to validate and parse.</param>
        /// <param name="parsedGuid">The resulting GUID if the validation succeeds.</param>
        /// <returns>True if the string is a valid GUID, otherwise false.</returns>
        protected bool IsGuidValid(string? id, ref Guid parsedGuid)
        {
            // Non-existing parameter in the URL
            if (String.IsNullOrWhiteSpace(id))
            {
                return false;
            }

            // Invalid parameter in the URL
            bool isGuidValid = Guid.TryParse(id, out parsedGuid);
            if (!isGuidValid)
            {
                return false;
            }

            return true;
        }
    }
}
