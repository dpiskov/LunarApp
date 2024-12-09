using LunarApp.Data.Models;
using LunarApp.Services.Data.Interfaces;
using LunarApp.Web.ViewModels;
using LunarApp.Web.ViewModels.Folder;
using LunarApp.Web.ViewModels.Notebook;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LunarApp.Web.Controllers
{
    [Authorize]
    public class NotebookController(INotebookService notebookService, IBaseService baseService)
        : Controller
    {
        // GET method to display the list of notebooks
        public async Task<IActionResult> Index(SearchFilterViewModel inputModel)
        {
            if (string.IsNullOrWhiteSpace(inputModel.SearchQuery) == false || string.IsNullOrWhiteSpace(inputModel.TagFilter) == false)
            {
                FolderNotesViewModel foldersAndNotes = await baseService.GetFilteredNotesAsync(inputModel.SearchQuery, inputModel.TagFilter);

                ViewData["Title"] = "Filtered Notes";
                return View("FilteredIndex", new SearchFilterViewModel
                {
                    FolderNotes = foldersAndNotes,
                    AllTags = await baseService.GetAllTagsAsync(),
                    SearchQuery = inputModel.SearchQuery,
                    TagFilter = inputModel.TagFilter
                });
            }

            IEnumerable<NotebookInfoViewModel> notebooks = await notebookService.IndexGetAllOrderedByTitleAsync();

            SearchFilterViewModel viewModel = new SearchFilterViewModel
            {
                Notebooks = notebooks,
                AllTags = await baseService.GetAllTagsAsync(),
                SearchQuery = inputModel.SearchQuery,
                TagFilter = inputModel.TagFilter
            };

            // Returns the view with the model containing the notebooks data
            return View(viewModel);
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
            if (ModelState.IsValid)
            {
                Notebook? existingNotebook = await notebookService.GetByTitleAsync(model.Title);

                if (existingNotebook != null)
                {
                    // Add a model state error if the notebook already exists
                    ModelState.AddModelError("Title", "A notebook with this title already exists.");
                    return View(model);  // Return to the form with the error message
                }

                // Checks if the submitted form data is valid

                await notebookService.AddNotebookAsync(model);

                // Redirects to the Index action to show the updated list of notebooks
                return RedirectToAction(nameof(Index));
            }

            return View(model);                                    // If not valid, returns the form view with validation errors
        }

        // GET method to render the confirmation page for deleting a notebook
        [HttpGet]
        public async Task<IActionResult> Remove(Guid notebookId)
        {
            NotebookDeleteViewModel? model =
                await notebookService.GetNotebookForDeleteByIdAsync(notebookId);

            // If the notebook doesn't exist, redirects to the Index view
            if (model == null)
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
            if (ModelState.IsValid)
            {
                await notebookService
                    .DeleteNotebookAsync(model.Id);

                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }

        // GET method to render the form for editing an existing notebook
        [HttpGet]
        public async Task<IActionResult> Edit(Guid notebookId)
        {
            NotebookEditViewModel? model = await notebookService.GetNotebookForEditByIdAsync(notebookId);

            if (model == null)
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
            if (ModelState.IsValid)
            {
                Notebook? existingNotebook = await notebookService.GetByTitleAsync(model.Title);

                if (existingNotebook != null)
                {
                    // Add a model state error if the notebook already exists
                    ModelState.AddModelError("Title", "A notebook with this title already exists.");
                    return View(model);  // Return to the form with the error message
                }

                bool isEdited = await notebookService.EditNotebookAsync(model);

                if (isEdited == false)
                {
                    return RedirectToAction(nameof(Index));
                }

                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Details(Guid notebookId)
        {
            NotebookDetailsViewModel? model = await notebookService.GetNotebookDetailsByIdAsync(notebookId);

            if (model == null)
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
            if (ModelState.IsValid)
            {
                bool isEdited = await notebookService.EditDetailsNotebookAsync(model);

                if (isEdited == false)
                {
                    return RedirectToAction(nameof(Index));
                }

                return RedirectToAction(nameof(Index));
            }

            return View(model);                             // If not valid, returns the form view with validation errors
        }
    }
}