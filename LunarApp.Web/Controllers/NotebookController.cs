using LunarApp.Data.Models;
using LunarApp.Services.Data.Interfaces;
using LunarApp.Web.ViewModels;
using LunarApp.Web.ViewModels.Folder;
using LunarApp.Web.ViewModels.Notebook;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace LunarApp.Web.Controllers
{
    [Authorize]
    public class NotebookController(
        INotebookService notebookService,
        IBaseService baseService,
        UserManager<ApplicationUser> userManager) : BaseController(userManager)
    {
        // GET method to display the list of notebooks, with optional search and filter functionality
        public async Task<IActionResult> Index(SearchFilterViewModel inputModel)
        {
            Guid currentUserId = GetCurrentUserId();

            if (string.IsNullOrWhiteSpace(inputModel.SearchQuery) == false || string.IsNullOrWhiteSpace(inputModel.TagFilter) == false)
            {
                FolderNotesViewModel foldersAndNotes = await baseService.GetFilteredNotesAsync(currentUserId, inputModel.SearchQuery, inputModel.TagFilter);

                ViewData["Title"] = "Filtered Notes";
                return View("FilteredIndex", new SearchFilterViewModel
                {
                    FolderNotes = foldersAndNotes,
                    AllTags = await baseService.GetAllTagsAsync(currentUserId),
                    SearchQuery = inputModel.SearchQuery,
                    TagFilter = inputModel.TagFilter
                });
            }

            IEnumerable<NotebookInfoViewModel> notebooks = await notebookService.IndexGetAllOrderedByTitleAsync(currentUserId);

            SearchFilterViewModel viewModel = new SearchFilterViewModel
            {
                Notebooks = notebooks,
                AllTags = await baseService.GetAllTagsAsync(currentUserId),
                SearchQuery = inputModel.SearchQuery,
                TagFilter = inputModel.TagFilter
            };

            return View(viewModel);
        }

        // GET method to render the form for creating a new notebook
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View();
        }

        // POST method to handle form submission for creating a new notebook
        [HttpPost]
        public async Task<IActionResult> Create(NotebookCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                Guid currentUserId = GetCurrentUserId();

                Notebook? existingNotebook = await notebookService.GetByTitleAsync(model.Title, currentUserId);

                if (existingNotebook != null)
                {
                    ModelState.AddModelError("Title", "A notebook with this title already exists.");
                    return View(model);
                }

                await notebookService.AddNotebookAsync(model, currentUserId);

                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }

        // GET method to render the confirmation page for deleting a notebook
        [HttpGet]
        public async Task<IActionResult> Remove(Guid notebookId)
        {
            Guid currentUserId = GetCurrentUserId();

            NotebookDeleteViewModel? model =
                await notebookService.GetNotebookForDeleteByIdAsync(notebookId, currentUserId);

            if (model == null)
            {
                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }

        // POST method to remove a notebook from the database
        [HttpPost]
        public async Task<IActionResult> Remove(NotebookDeleteViewModel model)
        {
            if (ModelState.IsValid)
            {
                Guid currentUserId = GetCurrentUserId();

                await notebookService
                    .DeleteNotebookAsync(model.Id, currentUserId);

                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }

        // GET method to render the form for editing an existing notebook
        [HttpGet]
        public async Task<IActionResult> Edit(Guid notebookId)
        {
            Guid currentUserId = GetCurrentUserId();

            NotebookEditViewModel? model = await notebookService.GetNotebookForEditByIdAsync(notebookId, currentUserId);

            if (model == null)
            {
                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }

        // POST method to handle form submission for updating a notebook's information
        [HttpPost]
        public async Task<IActionResult> Edit(NotebookEditViewModel model)
        {
            if (ModelState.IsValid)
            {
                Guid currentUserId = GetCurrentUserId();

                Notebook? existingNotebook = await notebookService.GetByTitleAsync(model.Title, currentUserId);

                if (existingNotebook != null)
                {
                    ModelState.AddModelError("Title", "A notebook with this title already exists.");
                    return View(model);
                }

                bool isEdited = await notebookService.EditNotebookAsync(model, currentUserId);

                if (isEdited == false)
                {
                    return RedirectToAction(nameof(Index));
                }

                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }

        // GET method to display the details of a specific notebook
        [HttpGet]
        public async Task<IActionResult> Details(Guid notebookId)
        {
            Guid currentUserId = GetCurrentUserId();

            NotebookDetailsViewModel? model = await notebookService.GetNotebookDetailsByIdAsync(notebookId, currentUserId);

            if (model == null)
            {
                return RedirectToAction(nameof(Index));
            }

            ViewData["Title"] = model.Title;

            return View(model);
        }

        // POST method to handle updating notebook details
        [HttpPost]
        public async Task<IActionResult> Details(NotebookDetailsViewModel model)
        {
            if (ModelState.IsValid)
            {
                Guid currentUserId = GetCurrentUserId();

                bool isEdited = await notebookService.EditDetailsNotebookAsync(model, currentUserId);

                if (isEdited == false)
                {
                    return RedirectToAction(nameof(Index));
                }

                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }
    }
}