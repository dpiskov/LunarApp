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
    /// <summary>
    /// Controller responsible for managing notebooks, including creating, editing, deleting, and retrieving notebooks.
    /// </summary>
    /// <remarks>
    /// The <see cref="NotebookController"/> uses the <see cref="INotebookService"/> to perform notebook-related operations. 
    /// It also leverages the <see cref="IBaseService"/> for common tasks like filtering notes and managing tags, 
    /// and utilizes <see cref="UserManager{ApplicationUser}"/> to access user-specific information and manage user-related actions.
    /// </remarks>
    /// <param name="notebookService">An instance of <see cref="INotebookService"/> to handle notebook-related operations.</param>
    /// <param name="baseService">An instance of <see cref="IBaseService"/> for common services such as note filtering and tag retrieval.</param>
    /// <param name="userManager">An instance of <see cref="UserManager{ApplicationUser}"/> to manage user-related tasks and access current user information.</param>
    [Authorize]
    public class NotebookController(
        INotebookService notebookService,
        IBaseService baseService,
        UserManager<ApplicationUser> userManager) : BaseController(userManager)
    {
        /// <summary>
        /// Displays the list of notebooks with optional search and filter functionality.
        /// </summary>
        /// <param name="inputModel">The model containing search and filter criteria.</param>
        /// <returns>A view with the list of notebooks, optionally filtered by search query and tag filter.</returns>
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

        /// <summary>
        /// Renders the form for creating a new notebook.
        /// </summary>
        /// <returns>The view for creating a new notebook.</returns>
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View();
        }

        /// <summary>
        /// Handles the form submission for creating a new notebook.
        /// </summary>
        /// <param name="model">The model containing data for the new notebook.</param>
        /// <returns>A redirect to the notebook index page or a re-rendering of the create form if there are validation errors.</returns>
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

        /// <summary>
        /// Renders the confirmation page for deleting a notebook.
        /// </summary>
        /// <param name="notebookId">The ID of the notebook to be deleted.</param>
        /// <returns>A view to confirm the deletion of the notebook.</returns>
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

        /// <summary>
        /// Handles the POST request to delete a notebook from the database.
        /// </summary>
        /// <param name="model">The model containing the notebook data to be deleted.</param>
        /// <returns>A redirect to the notebook index page after successful deletion.</returns>
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

        /// <summary>
        /// Renders the form for editing an existing notebook.
        /// </summary>
        /// <param name="notebookId">The ID of the notebook to be edited.</param>
        /// <returns>A view to edit the notebook.</returns>
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

        /// <summary>
        /// Handles the form submission for updating an existing notebook's information.
        /// </summary>
        /// <param name="model">The model containing updated notebook data.</param>
        /// <returns>A redirect to the notebook index page if successful or a re-rendering of the edit form if there are validation errors.</returns>
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

        /// <summary>
        /// Displays the details of a specific notebook.
        /// </summary>
        /// <param name="notebookId">The ID of the notebook whose details are to be displayed.</param>
        /// <returns>A view displaying the details of the specified notebook.</returns>
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

        /// <summary>
        /// Handles updating the details of a specific notebook.
        /// </summary>
        /// <param name="model">The model containing the updated details for the notebook.</param>
        /// <returns>A redirect to the notebook index page if successful, or a re-rendering of the details form if there are validation errors.</returns>
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