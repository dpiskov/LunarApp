using LunarApp.Data.Models;
using LunarApp.Services.Data.Interfaces;
using LunarApp.Web.ViewModels;
using LunarApp.Web.ViewModels.Folder;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace LunarApp.Web.Controllers
{
    /// <summary>
    /// Controller responsible for managing folders, subfolders, and folder-related operations in the application.
    /// Supports operations like creating, editing, deleting, and retrieving folders.
    /// </summary>
    /// <remarks>
    /// The <see cref="FolderController"/> handles folder-related actions within the application. 
    /// It supports creating, editing, deleting, and retrieving folders, along with managing subfolders. 
    /// The controller interacts with the <see cref="IFolderService"/> for business logic and performs user-specific 
    /// operations by utilizing the <see cref="IBaseService"/> and <see cref="UserManager{ApplicationUser}"/>.
    /// </remarks>
    /// <param name="folderService">An instance of <see cref="IFolderService"/> to manage folder operations such as create, edit, and delete.</param>
    /// <param name="baseService">An instance of <see cref="IBaseService"/> that provides common services for the controller.</param>
    /// <param name="userManager">An instance of <see cref="UserManager{ApplicationUser}"/> for managing user-related operations.</param>
    [Authorize]
    public class FolderController(
        IFolderService folderService,
        IBaseService baseService,
        UserManager<ApplicationUser> userManager) : BaseController(userManager)
    {
        /// <summary>
        /// Retrieves and displays all folders and notes within a specific notebook or folder,
        /// with optional search and tag filtering.
        /// </summary>
        /// <param name="inputModel">The filter model containing search query and tag filter.</param>
        /// <param name="notebookId">The ID of the notebook.</param>
        /// <param name="parentFolderId">The ID of the parent folder.</param>
        /// <param name="folderId">The ID of the folder to display.</param>
        /// <returns>The view displaying folders and notes.</returns>
        public async Task<IActionResult> Index(SearchFilterViewModel inputModel, Guid notebookId, Guid? parentFolderId, Guid? folderId)
        {
            FolderNotesViewModel foldersAndNotes;

            Guid currentUserId = GetCurrentUserId();

            if (string.IsNullOrWhiteSpace(inputModel.SearchQuery) == false || string.IsNullOrWhiteSpace(inputModel.TagFilter) == false)
            {
                foldersAndNotes = await baseService.GetFilteredNotesAsyncByNotebookId(notebookId, parentFolderId, folderId, currentUserId, inputModel.SearchQuery, inputModel.TagFilter);

                ViewData["Title"] = "Filtered Notes";
                ViewData["NotebookId"] = notebookId;
                ViewData["ParentFolderId"] = parentFolderId;
                ViewData["FolderId"] = folderId;

                return View("FilteredIndex", new SearchFilterViewModel
                {
                    FolderNotes = foldersAndNotes,
                    AllTags = await baseService.GetAllTagsAsync(currentUserId),
                    SearchQuery = inputModel.SearchQuery,
                    TagFilter = inputModel.TagFilter
                });
            }
            else
            {
                foldersAndNotes = await folderService.IndexGetAllFoldersAsync(notebookId, parentFolderId, folderId, currentUserId);
            }

            SearchFilterViewModel viewModel = new SearchFilterViewModel
            {
                FolderNotes = foldersAndNotes,
                AllTags = await baseService.GetAllTagsAsync(currentUserId),
                SearchQuery = inputModel.SearchQuery,
                TagFilter = inputModel.TagFilter
            };

            if (folderId != Guid.Empty && folderId != null && notebookId != Guid.Empty && notebookId != null)
            {
                string? parentFolderTitle = await folderService.GetFolderTitleAsync(folderId.Value);
                (Guid newParentFolderId, Guid newFolderId) =
                    await folderService.GetFolderAndParentIdsAsync(parentFolderId, Guid.Empty, Guid.Empty, currentUserId);

                ViewData["Title"] = parentFolderTitle;
                ViewData["NotebookId"] = notebookId;
                ViewData["ParentFolderId"] = parentFolderId;
                ViewData["FolderId"] = folderId;
                ViewData["NewParentFolderId"] = newParentFolderId;
                ViewData["NewFolderId"] = newFolderId;
            }
            else if (notebookId != Guid.Empty && notebookId != null)
            {
                string? notebookTitle = await folderService.GetNotebookTitleAsync(notebookId, currentUserId);

                if (notebookTitle == null)
                {
                    return RedirectToAction(nameof(Index), "Home");
                }

                ViewData["NotebookId"] = notebookId;
                ViewData["Title"] = notebookTitle;
            }
            else
            {
                return RedirectToAction(nameof(Index), "Notebook");
            }

            return View(viewModel);
        }

        /// <summary>
        /// Displays the form to add a new folder.
        /// </summary>
        /// <param name="notebookId">The ID of the notebook.</param>
        /// <param name="parentFolderId">The ID of the parent folder.</param>
        /// <param name="folderId">The ID of the folder.</param>
        /// <returns>The view displaying the folder creation form.</returns>
        [HttpGet]
        public async Task<IActionResult> AddFolder(Guid notebookId, Guid? parentFolderId, Guid? folderId)
        {
            FolderCreateViewModel? model = await folderService.GetAddFolderModelAsync(notebookId, parentFolderId, folderId);

            ViewData["NotebookId"] = notebookId;
            ViewData["ParentFolderId"] = parentFolderId;
            ViewData["FolderId"] = folderId;

            return View(model);
        }

        /// <summary>
        /// Handles the POST request to add a new folder.
        /// </summary>
        /// <param name="model">The folder creation model containing folder details.</param>
        /// <returns>A redirect to the appropriate folder or notebook view.</returns>
        [HttpPost]
        public async Task<IActionResult> AddFolder(FolderCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                Folder? existingFolder = await folderService.GetByTitleAsync(model.Title, model.NotebookId, model.ParentFolderId, model.FolderId);

                Folder? existingFolderByNotebookId = null;

                Guid currentUserId = GetCurrentUserId();

                if (model.FolderId == null || model.FolderId == Guid.Empty)
                {
                    existingFolderByNotebookId = await folderService.GetByTitleInNotebookAsync(model.Title, model.NotebookId, currentUserId);
                }

                if (existingFolder != null || existingFolderByNotebookId != null)
                {
                    ModelState.AddModelError("Title", "A folder with this title already exists.");

                    ViewData["NotebookId"] = model.NotebookId;
                    ViewData["ParentFolderId"] = model.ParentFolderId;
                    ViewData["FolderId"] = model.FolderId;

                    return View(model);
                }

                (bool isSuccess, string? errorMessage) = await folderService.AddFolderAsync(model, currentUserId);

                if (isSuccess == false)
                {
                    ModelState.AddModelError(string.Empty, errorMessage ?? "An unknown error occurred.");
                    return View(model);
                }

                return RedirectToFolderOrNotebookIndex(model.NotebookId, model.ParentFolderId, model.FolderId);
            }

            return View(model);
        }

        /// <summary>
        /// Displays the form to add a new subfolder within an existing folder.
        /// </summary>
        /// <param name="notebookId">The ID of the notebook.</param>
        /// <param name="parentFolderId">The ID of the parent folder.</param>
        /// <param name="folderId">The ID of the folder.</param>
        /// <returns>The view displaying the subfolder creation form.</returns>
        [HttpGet]
        public async Task<IActionResult> AddSubfolder(Guid notebookId, Guid? parentFolderId, Guid? folderId)
        {
            Guid currentUserId = GetCurrentUserId();

            (FolderCreateViewModel model, Guid newParentFolderId) = await folderService.GetAddSubfolderModelAsync(notebookId, parentFolderId, folderId, currentUserId);

            ViewData["NotebookId"] = notebookId;
            ViewData["ParentFolderId"] = parentFolderId;
            ViewData["FolderId"] = folderId;

            ViewData["NewParentFolderId"] = newParentFolderId;

            return View(model);
        }

        /// <summary>
        /// Handles the POST request to add a new subfolder.
        /// </summary>
        /// <param name="model">The subfolder creation model containing subfolder details.</param>
        /// <returns>A redirect to the appropriate folder or notebook view.</returns>
        [HttpPost]
        public async Task<IActionResult> AddSubfolder(FolderCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                Guid currentUserId = GetCurrentUserId();

                Folder? existingFolder = await folderService.GetByTitleAsync(model.Title, model.NotebookId, model.ParentFolderId, model.FolderId);

                Folder? existingFolderByNotebookId = await folderService.GetByTitleInNotebookAsync(model.Title, model.NotebookId, currentUserId);

                if (model.FolderId == null || model.FolderId == Guid.Empty)
                {
                    if (existingFolder != null || existingFolderByNotebookId != null)
                    {
                        ModelState.AddModelError("Title", "A folder with this title already exists.");

                        ViewData["NotebookId"] = model.NotebookId;
                        ViewData["ParentFolderId"] = model.ParentFolderId;
                        ViewData["FolderId"] = model.FolderId;

                        return View(model);
                    }
                }

                (bool isSuccess, string? errorMessage) = await folderService.AddFolderAsync(model, currentUserId);

                if (isSuccess == false)
                {
                    ModelState.AddModelError(string.Empty, errorMessage ?? "An unknown error occurred.");
                    return View(model);
                }

                return RedirectToFolderOrNotebookIndex(model.NotebookId, model.ParentFolderId, model.FolderId);
            }

            return View(model);
        }

        /// <summary>
        /// Displays the form to remove a folder.
        /// </summary>
        /// <param name="notebookId">The ID of the notebook.</param>
        /// <param name="parentFolderId">The ID of the parent folder.</param>
        /// <param name="folderId">The ID of the folder to delete.</param>
        /// <returns>The view displaying the folder removal confirmation form.</returns>
        [HttpGet]
        public async Task<IActionResult> Remove(Guid notebookId, Guid? parentFolderId, Guid folderId)
        {
            Guid currentUserId = GetCurrentUserId();

            (FolderDeleteViewModel? model, Guid newParentFolderId) =
                await folderService.GetFolderForDeleteByIdAsync(notebookId, parentFolderId, folderId, currentUserId);

            ViewData["NotebookId"] = notebookId;
            ViewData["ParentFolderId"] = parentFolderId;
            ViewData["FolderId"] = folderId;

            ViewData["NewParentFolderId"] = newParentFolderId;

            return View(model);
        }

        /// <summary>
        /// Handles the POST request to remove a folder and its children.
        /// </summary>
        /// <param name="model">The folder deletion model containing folder details.</param>
        /// <returns>A redirect to the appropriate folder or notebook view.</returns>
        [HttpPost]
        public async Task<IActionResult> Remove(FolderDeleteViewModel model)
        {
            if (ModelState.IsValid)
            {
                Guid currentUserId = GetCurrentUserId();

                await folderService.DeleteFolderWithChildrenAsync(model.FolderId, currentUserId);

                Folder? folder = await folderService.GetFolderForRedirectionAsync(model.FolderId, model.ParentFolderId);

                if (folder != null)
                {
                    return RedirectToFolderOrNotebookIndex(folder.NotebookId, folder.ParentFolderId, folder.Id);
                }

                return RedirectToFolderOrNotebookIndex(model.NotebookId, Guid.Empty, Guid.Empty);
            }

            return View(model);
        }

        /// <summary>
        /// Displays the form to edit an existing folder's details.
        /// </summary>
        /// <param name="notebookId">The ID of the notebook.</param>
        /// <param name="parentFolderId">The ID of the parent folder.</param>
        /// <param name="folderId">The ID of the folder to edit.</param>
        /// <returns>The view displaying the folder edit form.</returns>
        [HttpGet]
        public async Task<IActionResult> Edit(Guid notebookId, Guid? parentFolderId, Guid folderId)
        {
            Guid currentUserId = GetCurrentUserId();

            (FolderEditViewModel? model, Guid newParentFolderId) = await folderService.GetFolderForEditByIdAsync(notebookId, parentFolderId, folderId, currentUserId);

            if (model == null)
            {
                return RedirectToFolderOrNotebookIndexForEdit(notebookId, parentFolderId, folderId, newParentFolderId);
            }

            ViewData["NotebookId"] = notebookId;
            ViewData["ParentFolderId"] = parentFolderId;
            ViewData["FolderId"] = folderId;

            ViewData["NewParentFolderId"] = newParentFolderId;

            return View(model);
        }

        /// <summary>
        /// Handles the POST request to edit an existing folder.
        /// </summary>
        /// <param name="model">The folder edit model containing updated folder details.</param>
        /// <returns>A redirect to the updated folder or notebook view.</returns>
        [HttpPost]
        public async Task<IActionResult> Edit(FolderEditViewModel model)
        {
            if (ModelState.IsValid)
            {
                Guid currentUserId = GetCurrentUserId();

                Folder? existingFolderForEdit = await folderService.GetByTitleForEditAsync(model.Title, model.ParentFolderId, model.FolderId, currentUserId);

                if (existingFolderForEdit != null)
                {
                    ModelState.AddModelError("Title", "A folder with this title already exists.");

                    ViewData["NotebookId"] = model.NotebookId;
                    ViewData["ParentFolderId"] = model.ParentFolderId;
                    ViewData["FolderId"] = model.FolderId;

                    return View(model);
                }

                (bool isEdited, Folder? parentFolder) = await folderService.EditFolderAsync(model);

                if (isEdited == false)
                {
                    return RedirectToAction(nameof(Index));
                }

                if (parentFolder != null)
                {
                    return RedirectToFolderForEditOrDetails(parentFolder.NotebookId, parentFolder.ParentFolderId, parentFolder.Id, model.IsAccessedDirectlyFromNotebook);
                }
                else if (model.NotebookId != Guid.Empty && model.NotebookId != null)
                {
                    return RedirectToFolderForEditOrDetails(model.NotebookId, Guid.Empty, Guid.Empty, model.IsAccessedDirectlyFromNotebook);
                }

                return RedirectToAction("Index", "Notebook");
            }

            return View(model);
        }

        /// <summary>
        /// Displays the details of a specific folder.
        /// </summary>
        /// <param name="notebookId">The ID of the notebook.</param>
        /// <param name="parentFolderId">The ID of the parent folder.</param>
        /// <param name="folderId">The ID of the folder to view.</param>
        /// <returns>The view displaying the folder's details.</returns>
        [HttpGet]
        public async Task<IActionResult> Details(Guid notebookId, Guid? parentFolderId, Guid folderId)
        {
            Guid currentUserId = GetCurrentUserId();

            (FolderDetailsViewModel? model, Guid newParentFolderId) = await folderService.GetFolderDetailsByIdAsync(notebookId, parentFolderId, folderId, currentUserId);

            ViewData["Title"] = model.Title;

            ViewData["NotebookId"] = notebookId;
            ViewData["ParentFolderId"] = parentFolderId;
            ViewData["FolderId"] = folderId;

            ViewData["NewParentFolderId"] = newParentFolderId;

            return View(model);
        }

        /// <summary>
        /// Handles the POST request to edit folder details.
        /// </summary>
        /// <param name="model">The folder details model containing updated information.</param>
        /// <returns>A redirect to the updated folder or notebook view.</returns>
        [HttpPost]
        public async Task<IActionResult> Details(FolderDetailsViewModel model)
        {
            if (ModelState.IsValid)
            {
                (bool isEdited, Folder? parentFolder) = await folderService.EditDetailsFolderAsync(model);

                if (isEdited == false)
                {
                    return RedirectToAction(nameof(Index));
                }

                if (parentFolder != null)
                {
                    return RedirectToFolderForEditOrDetails(parentFolder.NotebookId, parentFolder.ParentFolderId, parentFolder.Id, model.IsAccessedDirectlyFromNotebook);
                }
                else if (model.NotebookId != Guid.Empty && model.NotebookId != null)
                {
                    return RedirectToFolderForEditOrDetails(model.NotebookId, Guid.Empty, Guid.Empty, model.IsAccessedDirectlyFromNotebook);
                }

                return RedirectToAction("Index", "Notebook");
            }

            return View(model);
        }

        /// <summary>
        /// Helper method for redirecting to the correct folder or notebook index view after adding/editing/removing folders.
        /// </summary>
        /// <param name="notebookId">The ID of the notebook.</param>
        /// <param name="parentFolderId">The ID of the parent folder.</param>
        /// <param name="folderId">The ID of the folder.</param>
        /// <returns>A redirect to the appropriate folder or notebook index view.</returns>
        private IActionResult RedirectToFolderOrNotebookIndex(Guid? notebookId, Guid? parentFolderId, Guid? folderId)
        {
            if (parentFolderId.HasValue && parentFolderId != Guid.Empty &&
                folderId.HasValue && folderId != Guid.Empty)
            {
                return Redirect(
                    $"~/Folder?notebookId={notebookId}&parentFolderId={parentFolderId}&folderId={folderId}");
            }
            else if (folderId.HasValue && folderId != Guid.Empty)
            {
                return Redirect($"~/Folder?notebookId={notebookId}&folderId={folderId}");
            }
            else if (notebookId.HasValue && notebookId != Guid.Empty)
            {
                return Redirect($"~/Folder?notebookId={notebookId}");
            }

            return RedirectToAction("Index", "Notebook");
        }

        /// <summary>
        /// Redirects to the appropriate folder or notebook index view after editing a folder,
        /// based on the parent folder and new parent folder IDs provided.
        /// </summary>
        /// <param name="notebookId">The ID of the notebook that the folder belongs to.</param>
        /// <param name="parentFolderId">The ID of the folder's current parent folder.</param>
        /// <param name="folderId">The ID of the folder being edited.</param>
        /// <param name="newParentFolderId">The ID of the folder's new parent folder, if the parent folder changes.</param>
        /// <returns>A redirect to the updated folder or notebook index view.</returns>
        private IActionResult RedirectToFolderOrNotebookIndexForEdit(Guid notebookId, Guid? parentFolderId, Guid folderId,
            Guid newParentFolderId)
        {
            if (newParentFolderId != Guid.Empty && newParentFolderId != null &&
                parentFolderId != Guid.Empty && parentFolderId != null)
            {
                return Redirect(
                    $"~/Folder?notebookId={notebookId}&parentFolderId={newParentFolderId}&folderId={parentFolderId}");
            }
            if (folderId != Guid.Empty && folderId != null &&
                parentFolderId != Guid.Empty && parentFolderId != null)
            {
                return Redirect($"~/Folder?notebookId={notebookId}&folderId={parentFolderId}");
            }
            if (folderId != Guid.Empty && folderId != null)
            {
                return Redirect($"~/Folder?notebookId={notebookId}");
            }

            return RedirectToAction("Index", "Notebook");
        }

        /// <summary>
        /// Redirects to the appropriate folder view after editing or accessing the details of a folder.
        /// The redirection depends on whether the folder was accessed directly from the notebook or through another folder.
        /// </summary>
        /// <param name="notebookId">The ID of the notebook to which the folder belongs.</param>
        /// <param name="parentFolderId">The ID of the folder's parent folder, if applicable.</param>
        /// <param name="folderId">The ID of the folder being edited or viewed.</param>
        /// <param name="isAccessedDirectlyFromNotebook">Indicates whether the folder was accessed directly from the notebook (true) or through another folder (false).</param>
        /// <returns>A redirect to the appropriate folder or notebook view after edit or details changes.</returns>
        private IActionResult RedirectToFolderForEditOrDetails(Guid notebookId, Guid? parentFolderId, Guid? folderId, bool isAccessedDirectlyFromNotebook)
        {
            if (parentFolderId.HasValue && parentFolderId != Guid.Empty && folderId.HasValue && folderId != Guid.Empty)
            {
                return Redirect($"~/Folder?notebookId={notebookId}&parentFolderId={parentFolderId}&folderId={folderId}");
            }
            else if (folderId.HasValue && folderId != Guid.Empty && !isAccessedDirectlyFromNotebook)
            {
                return Redirect($"~/Folder?notebookId={notebookId}&folderId={folderId}");
            }
            else if (notebookId != Guid.Empty)
            {
                return Redirect($"~/Folder?notebookId={notebookId}");
            }

            return RedirectToAction("Index", "Notebook");
        }
    }
}