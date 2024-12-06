using LunarApp.Data.Models;
using LunarApp.Services.Data.Interfaces;
using LunarApp.Web.ViewModels.Folder;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LunarApp.Web.Controllers
{
    // TODO: HANDLE EXCEPTIONS
    [Authorize]
    public class FolderController(IFolderService folderService) : Controller
    {
        public async Task<IActionResult> Index(Guid notebookId, Guid? parentFolderId, Guid? folderId)
        {
            FolderNotesViewModel foldersAndNotes = await folderService.IndexGetAllFoldersAsync(notebookId, parentFolderId, folderId);

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

            return View(foldersAndNotes);
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

            if (model.ParentFolderId != Guid.Empty && model.ParentFolderId != null &&
                model.FolderId != Guid.Empty && model.FolderId != null)
            {
                return Redirect(
                    $"~/Folder?notebookId={model.NotebookId}&parentFolderId={model.ParentFolderId}&folderId={model.FolderId}");
            }
            else if (model.FolderId != Guid.Empty && model.FolderId != null)
            {
                return Redirect($"~/Folder?notebookId={model.NotebookId}&folderId={model.FolderId}");
            }
            else if (model.NotebookId != Guid.Empty && model.NotebookId != null)
            {
                return Redirect($"~/Folder?notebookId={model.NotebookId}");
            }

            return View(model);
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

            if (model.ParentFolderId != Guid.Empty && model.ParentFolderId != null &&
                model.FolderId != Guid.Empty && model.FolderId != null)
            {
                return Redirect(
                    $"~/Folder?notebookId={model.NotebookId}&parentFolderId={model.ParentFolderId}&folderId={model.FolderId}");
            }
            //else if (folder.Id != Guid.Empty && model.IsMadeDirectlyFromNotebook == false)
            else if (model.FolderId != Guid.Empty && model.FolderId != null)
            {
                return Redirect($"~/Folder?notebookId={model.NotebookId}&folderId={model.FolderId}");
            }
            else if (model.NotebookId != Guid.Empty && model.NotebookId != null)
            {
                // Redirects to the main notebook view if no parent folder is specified
                return Redirect($"~/Folder?notebookId={model.NotebookId}");
            }

            return View(model);
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
            if (ModelState.IsValid)
            {
                await folderService.DeleteFolderWithChildrenAsync(model.FolderId);

                Folder? folder = await folderService.GetFolderForRedirectionAsync(model.FolderId, model.ParentFolderId);

                if (folder != null && folder.ParentFolderId != Guid.Empty && folder.ParentFolderId != null &&
                    folder.Id != Guid.Empty && folder.Id != null)
                {
                    return Redirect(
                        $"~/Folder?notebookId={folder.NotebookId}&parentFolderId={folder.ParentFolderId}&folderId={folder.Id}");
                }
                else if (folder != null && folder.Id != Guid.Empty && folder.Id != null)
                {
                    return Redirect($"~/Folder?notebookId={folder.NotebookId}&folderId={folder.Id}");
                }
                else if (model.NotebookId != Guid.Empty && model.NotebookId != null)
                {
                    // Redirects to the main notebook view if no parent folder is specified
                    return Redirect($"~/Folder?notebookId={model.NotebookId}");
                }
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid notebookId, Guid? parentFolderId, Guid folderId)
        {
            (FolderEditViewModel? model, Guid newParentFolderId) = await folderService.GetFolderForEditByIdAsync(notebookId, parentFolderId, folderId);

            //TODO: SIMPLIFY IF POSSIBLE
            if (model == null)
            {
                if (folderId != Guid.Empty && folderId != null &&
                    parentFolderId != Guid.Empty && parentFolderId != null)
                {
                    return RedirectToAction(nameof(Index), "Folder", new { notebookId = notebookId, parentFolderId = parentFolderId, folderId = folderId });
                }
                if (folderId != Guid.Empty && folderId != null)
                {
                    return RedirectToAction(nameof(Index), "Folder", new { notebookId = notebookId, folderId = folderId });
                }

                return RedirectToAction(nameof(Index), "Folder", new { notebookId = notebookId });
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

                if (parentFolder != null && parentFolder.ParentFolderId != Guid.Empty && parentFolder.ParentFolderId != null &&
                    parentFolder.Id != Guid.Empty && parentFolder.Id != null)
                {
                    return Redirect(
                        $"~/Folder?notebookId={parentFolder.NotebookId}&parentFolderId={parentFolder.ParentFolderId}&folderId={parentFolder.Id}");
                }
                else if (parentFolder != null && parentFolder.Id != Guid.Empty && parentFolder.Id != null &&
                         model.IsEditedDirectlyFromNotebook == false)
                {
                    return Redirect($"~/Folder?notebookId={parentFolder.NotebookId}&folderId={parentFolder.Id}");
                }
                else if (model.NotebookId != Guid.Empty && model.NotebookId != null)
                {
                    return Redirect($"~/Folder?notebookId={model.NotebookId}");
                }
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

                if (parentFolder != null && parentFolder.ParentFolderId != Guid.Empty && parentFolder.ParentFolderId != null &&
                    parentFolder.Id != Guid.Empty && parentFolder.Id != null)
                {
                    return Redirect(
                        $"~/Folder?notebookId={parentFolder.NotebookId}&parentFolderId={parentFolder.ParentFolderId}&folderId={parentFolder.Id}");
                }
                else if (parentFolder != null && parentFolder.Id != Guid.Empty && parentFolder.Id != null &&
                         model.IsClickedDirectlyFromNotebook == false)
                {
                    return Redirect($"~/Folder?notebookId={parentFolder.NotebookId}&folderId={parentFolder.Id}");
                }
                else if (model.NotebookId != Guid.Empty && model.NotebookId != null)
                {
                    return Redirect($"~/Folder?notebookId={model.NotebookId}");
                }
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
    }
}