using LunarApp.Services.Data.Interfaces;
using LunarApp.Web.ViewModels.Notebook;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LunarApp.Web.Controllers
{
    [Authorize]
    public class NotebookController(INotebookService notebookService)
        : Controller
    {
        // GET method to display the list of notebooks
        public async Task<IActionResult> Index()
        {
            IEnumerable<NotebookInfoViewModel> notebooks = await notebookService.IndexGetAllOrderedByTitleAsync();

            // Returns the view with the model containing the notebooks data
            return View(notebooks);
        }

        // GET method to render the form for creating a new notebook
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View();                                             // Returns the create notebook form view
        }

        // POST method to handle form submission for creating a new notebook
        [HttpPost]
        public async Task<IActionResult> Create(NotebookCreateViewModel model)
        {
            // Checks if the submitted form data is valid
            if (ModelState.IsValid is false)
            {
                return View(model);                                    // If not valid, returns the form view with validation errors
            }

            await notebookService.AddNotebookAsync(model);

            // Redirects to the Index action to show the updated list of notebooks
            return RedirectToAction(nameof(Index));
        }

        // GET method to render the confirmation page for deleting a notebook
        [HttpGet]
        public async Task<IActionResult> Remove(Guid notebookId)
        {
            NotebookDeleteViewModel? model =
                await notebookService.GetNotebookForDeleteByIdAsync(notebookId);

            // If the notebook doesn't exist, redirects to the Index view
            if (model is null)
            {
                return RedirectToAction(nameof(Index));
            }

            // Returns the confirmation view with the notebook data
            return View(model);
        }

        // POST method to actually remove a notebook from the database
        [HttpPost]
        public async Task<IActionResult> Remove(NotebookDeleteViewModel model)
        {
            bool isDeleted = await notebookService
                .DeleteNotebookAsync(model.Id);

            if (isDeleted is false)
            {
                TempData["ErrorMessage"] =
                    "Unexpected error occurred while trying to delete the cinema! Please contact system administrator!";
                return this.RedirectToAction(nameof(Remove), new { notebookId = model.Id });
            }

            // Redirects to the Index view to show the updated list of notebooks
            return RedirectToAction(nameof(Index));
        }

        // GET method to render the form for editing an existing notebook
        [HttpGet]
        public async Task<IActionResult> Edit(Guid notebookId)
        {
            NotebookEditViewModel? model = await notebookService.GetNotebookForEditByIdAsync(notebookId);

            if (model is null)
            {
                return RedirectToAction(nameof(Index));
            }

            // Returns the edit view with the notebook data in the view model
            return View(model);
        }

        // POST method to handle form submission for updating a notebook's information
        [HttpPost]
        public async Task<IActionResult> Edit(NotebookEditViewModel model)
        {
            // Checks if the submitted form data is valid
            if (ModelState.IsValid is false)
            {
                return View(model);                             // If not valid, returns the form view with validation errors
            }

            bool isEdited = await notebookService.EditNotebookAsync(model);

            if (isEdited is false)
            {
                return RedirectToAction(nameof(Index));
            }

            // Redirects to the Index view to show the updated list of notebooks
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Details(Guid notebookId)
        {
            NotebookDetailsViewModel? model = await notebookService.GetNotebookDetailsByIdAsync(notebookId);

            if (model is null)
            {
                return RedirectToAction(nameof(Index));
            }

            ViewData["Title"] = model.Title;

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Details(NotebookDetailsViewModel model)
        {
            // Checks if the submitted form data is valid
            if (ModelState.IsValid is false)
            {
                return View(model);                             // If not valid, returns the form view with validation errors
            }

            bool isEdited = await notebookService.EditDetailsNotebookAsync(model);

            if (isEdited is false)
            {
                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction(nameof(Index));
        }
    }
}