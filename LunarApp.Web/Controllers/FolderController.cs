using LunarApp.Data.Models;
using LunarApp.Services.Data;
using LunarApp.Services.Data.Interfaces;
using LunarApp.Web.ViewModels;
using LunarApp.Web.ViewModels.Folder;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LunarApp.Web.Controllers
{
    // TODO: HANDLE EXCEPTIONS
    [Authorize]
    public class FolderController(IFolderService folderService, IBaseService baseService) : Controller
    {
        public async Task<IActionResult> Index(SearchFilterViewModel inputModel, Guid notebookId, Guid? parentFolderId, Guid? folderId)
        {
            FolderNotesViewModel foldersAndNotes;

            if (string.IsNullOrWhiteSpace(inputModel.SearchQuery) == false || string.IsNullOrWhiteSpace(inputModel.TagFilter) == false)
            {
                foldersAndNotes = await baseService.GetFilteredNotesAsyncByNotebookId(notebookId, parentFolderId, folderId, inputModel.SearchQuery, inputModel.TagFilter);

                ViewData["Title"] = "Filtered Notes";
                ViewData["NotebookId"] = notebookId;
                ViewData["ParentFolderId"] = parentFolderId;
                ViewData["FolderId"] = folderId;

                // Redirect with query parameters for filtered results
                return View("FilteredIndex", new SearchFilterViewModel
                {
                    FolderNotes = foldersAndNotes,
                    AllTags = await baseService.GetAllTagsAsync(),
                    SearchQuery = inputModel.SearchQuery,
                    TagFilter = inputModel.TagFilter
                });
            }
            else
            {
                // Default behavior
                foldersAndNotes = await folderService.IndexGetAllFoldersAsync(notebookId, parentFolderId, folderId);
            }

            SearchFilterViewModel viewModel = new SearchFilterViewModel
            {
                FolderNotes = foldersAndNotes,
                AllTags = await baseService.GetAllTagsAsync(),
                SearchQuery = inputModel.SearchQuery,
                TagFilter = inputModel.TagFilter
            };

            // Check if you need to return folder or notebook data
            if (folderId != Guid.Empty && folderId != null && notebookId != Guid.Empty && notebookId != null)
            {
                string? parentFolderTitle = await folderService.GetFolderTitleAsync(folderId.Value);
                (Guid newParentFolderId, Guid newFolderId) =
                    await folderService.GetFolderAndParentIdsAsync(parentFolderId, Guid.Empty, Guid.Empty);

                ViewData["Title"] = parentFolderTitle;
                ViewData["NotebookId"] = notebookId;
                ViewData["ParentFolderId"] = parentFolderId;
                ViewData["FolderId"] = folderId;
                ViewData["NewParentFolderId"] = newParentFolderId;
                ViewData["NewFolderId"] = newFolderId;
            }
            else if (notebookId != Guid.Empty && notebookId != null)
            {
                string? notebookTitle = await folderService.GetNotebookTitleAsync(notebookId);
                ViewData["NotebookId"] = notebookId;
                ViewData["Title"] = notebookTitle;
            }
            else
            {
                return RedirectToAction(nameof(Index), "Notebook");
            }

            // Return the default view with updated view model
            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> AddFolder(Guid notebookId, Guid? parentFolderId, Guid? folderId)
        {
            FolderCreateViewModel? model = await folderService.GetAddFolderModelAsync(notebookId, parentFolderId, folderId);

            ViewData["NotebookId"] = notebookId;
            ViewData["ParentFolderId"] = parentFolderId;
            ViewData["FolderId"] = folderId;

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddFolder(FolderCreateViewModel model)
        {
            if (ModelState.IsValid == false)
            {
                return View(model);
            }

            (bool isSuccess, string? errorMessage) = await folderService.AddFolderAsync(model);

            if (isSuccess == false)
            {
                ModelState.AddModelError(string.Empty, errorMessage ?? "An unknown error occurred.");
                return View(model);
            }

            return RedirectToFolderOrNotebookIndex(model.NotebookId, model.ParentFolderId, model.FolderId);
        }

        [HttpGet]
        public async Task<IActionResult> AddSubfolder(Guid notebookId, Guid? parentFolderId, Guid? folderId)
        {

            (FolderCreateViewModel model, Guid newParentFolderId) = await folderService.GetAddSubfolderModelAsync(notebookId, parentFolderId, folderId);

            ViewData["NotebookId"] = notebookId;
            ViewData["ParentFolderId"] = parentFolderId;
            ViewData["FolderId"] = folderId;

            ViewData["NewParentFolderId"] = newParentFolderId;

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddSubfolder(FolderCreateViewModel model)
        {
            if (ModelState.IsValid == false)
            {
                return View(model);
            }

            (bool isSuccess, string? errorMessage) = await folderService.AddFolderAsync(model);

            if (isSuccess == false)
            {
                ModelState.AddModelError(string.Empty, errorMessage ?? "An unknown error occurred.");
                return View(model);
            }

            return RedirectToFolderOrNotebookIndex(model.NotebookId, model.ParentFolderId, model.FolderId);
        }

        [HttpGet]
        public async Task<IActionResult> Remove(Guid notebookId, Guid? parentFolderId, Guid folderId)
        {
            (FolderDeleteViewModel? model, Guid newParentFolderId) =
                await folderService.GetFolderForDeleteByIdAsync(notebookId, parentFolderId, folderId);

            // Stores the data for the view to access
            ViewData["NotebookId"] = notebookId;
            ViewData["ParentFolderId"] = parentFolderId;
            ViewData["FolderId"] = folderId;

            ViewData["NewParentFolderId"] = newParentFolderId;

            // Return the view with the folder model
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Remove(FolderDeleteViewModel model)
        {
            if (ModelState.IsValid == false)
            {
                return View(model);
            }

            await folderService.DeleteFolderWithChildrenAsync(model.FolderId);

            Folder? folder = await folderService.GetFolderForRedirectionAsync(model.FolderId, model.ParentFolderId);

            if (folder != null)
            {
                return RedirectToFolderOrNotebookIndex(folder.NotebookId, folder.ParentFolderId, folder.Id);
            }

            return RedirectToFolderOrNotebookIndex(model.NotebookId, Guid.Empty, Guid.Empty);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid notebookId, Guid? parentFolderId, Guid folderId)
        {
            (FolderEditViewModel? model, Guid newParentFolderId) = await folderService.GetFolderForEditByIdAsync(notebookId, parentFolderId, folderId);

            //TODO: SIMPLIFY IF POSSIBLE
            if (model == null)
            {
                return RedirectToFolderOrNotebookIndexForEdit(notebookId, parentFolderId, folderId, newParentFolderId);
            }

            // Stores the data for the view to access
            ViewData["NotebookId"] = notebookId;
            ViewData["ParentFolderId"] = parentFolderId;
            ViewData["FolderId"] = folderId;

            ViewData["NewParentFolderId"] = newParentFolderId;

            // Returns the Edit view with the folder data
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(FolderEditViewModel model)
        {
            if (ModelState.IsValid)
            {
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

                return RedirectToAction("Index", "Notebook"); // Fallback
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Details(Guid notebookId, Guid? parentFolderId, Guid folderId)
        {
            (FolderDetailsViewModel? model, Guid newParentFolderId) = await folderService.GetFolderDetailsByIdAsync(notebookId, parentFolderId, folderId);

            ViewData["Title"] = model.Title;

            ViewData["NotebookId"] = notebookId;
            ViewData["ParentFolderId"] = parentFolderId;
            ViewData["FolderId"] = folderId;

            ViewData["NewParentFolderId"] = newParentFolderId;

            return View(model);
        }

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

                return RedirectToAction("Index", "Notebook"); // Fallback
            }

            return View(model);
        }

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

            return RedirectToAction("Index", "Notebook"); // Fallback
        }

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

            return RedirectToAction("Index", "Notebook"); // Fallback
        }

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

            return RedirectToAction("Index", "Notebook"); // Fallback
        }

    }
}