using LunarApp.Data;
using LunarApp.Data.Models;
using LunarApp.Services.Data.Interfaces;
using LunarApp.Web.ViewModels.Folder;
using LunarApp.Web.ViewModels.Note;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LunarApp.Web.Controllers
{
    // TODO: HANDLE EXCEPTIONS
    [Authorize]
    public class FolderController(ApplicationDbContext context, IFolderService folderService) : Controller
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
            if (ModelState.IsValid is false)
            {
                return View(model);
            }

            (bool isSuccess, string? errorMessage, Folder? folder) = await folderService.AddFolderAsync(model);

            if (isSuccess is false)
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
            bool isMadeDirectlyFromNotebook = parentFolderId == Guid.Empty || parentFolderId == null && folderId != Guid.Empty && folderId != null;

            // Initializes the view model with the provided notebookId and optional parentFolderId
            FolderCreateViewModel model = new FolderCreateViewModel
            {
                Title = string.Empty,
                NotebookId = notebookId,
                ParentFolderId = parentFolderId,
                FolderId = folderId,
                IsMadeDirectlyFromNotebook = isMadeDirectlyFromNotebook
            };

            Guid newParentFolderId = Guid.Empty;

            if (parentFolderId != Guid.Empty && parentFolderId != null)
            {
                newParentFolderId = await GetValue(parentFolderId, newParentFolderId);
            }

            // Passes notebookId and parentFolderId to the ViewData to be used in the view
            ViewData["NotebookId"] = notebookId;
            ViewData["ParentFolderId"] = parentFolderId;
            ViewData["FolderId"] = folderId;

            ViewData["NewParentFolderId"] = newParentFolderId;

            // Returns the form view with the model data
            return View(model);
        }

        // POST method to handle form submission for creating a new folder
        [HttpPost]
        public async Task<IActionResult> AddSubfolder(FolderCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                var notebook = await context.Notebooks.FindAsync(model.NotebookId);
                if (notebook is null)
                {
                    ModelState.AddModelError(string.Empty, "The selected notebook does not exist.");
                    return View(model);
                }

                Folder folder = new Folder
                {
                    Title = model.Title,
                    NotebookId = model.NotebookId,
                    Notebook = notebook,
                    ParentFolderId = model.FolderId
                };

                await context.Folders.AddAsync(folder);     // Adds the folder to the database
                await context.SaveChangesAsync();           // Saves changes to the database

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
            }

            return View(model);
        }

        // GET method to render the form for removing a folder
        [HttpGet]
        public async Task<IActionResult> Remove(Guid notebookId, Guid? parentFolderId, Guid folderId)
        {
            FolderDeleteViewModel? model = await context.Folders
                .Where(f => f.Id == folderId) // Filter by folder ID
                .AsNoTracking() // Do not track changes for efficiency
                .Select(f => new FolderDeleteViewModel()
                {
                    Title = f.Title,
                    NotebookId = f.NotebookId,
                    ParentFolderId = f.ParentFolderId,
                    FolderId = f.Id
                })
                .FirstOrDefaultAsync(); // Retrieve the first match or null if not found

            Guid newParentFolderId = Guid.Empty;

            if (parentFolderId != Guid.Empty && parentFolderId != null)
            {
                newParentFolderId = await GetValue(parentFolderId, newParentFolderId);
            }

            // Stores the data for the view to access
            ViewData["NotebookId"] = notebookId;
            ViewData["ParentFolderId"] = parentFolderId;
            ViewData["FolderId"] = folderId;

            ViewData["NewParentFolderId"] = newParentFolderId;

            // Return the view with the folder model
            return View(model);
        }

        // POST method to handle the removal of a folder
        [HttpPost]
        public async Task<IActionResult> Remove(FolderDeleteViewModel model)
        {
            if (ModelState.IsValid)
            {
                await DeleteFolderWithChildrenAsync(model.FolderId);

                var newParentFolderId = await context.Folders
                    .Include(f => f.ChildrenFolders)
                    .Select(f => f.ChildrenFolders
                        .Where(cf => cf.Id == model.ParentFolderId))
                    .FirstOrDefaultAsync();

                Folder? folder = await context.Folders
                    .Where(f => f.Id == model.FolderId)
                    .Select(f => new Folder()
                    {
                        Id = f.Id,
                        Title = f.Title,
                        ParentFolderId = f.ParentFolderId,
                        NotebookId = f.NotebookId,
                        Notebook = null,
                    })
                    .FirstOrDefaultAsync();

                if (folder == null)
                {
                    folder = await context.Folders
                        .Where(f => f.Id == model.ParentFolderId)
                        .Select(f => new Folder()
                        {
                            Id = f.Id,
                            Title = f.Title,
                            ParentFolderId = f.ParentFolderId,
                            NotebookId = f.NotebookId,
                            Notebook = null,
                        })
                        .FirstOrDefaultAsync();
                }

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

        // GET method to display the folder edit form
        [HttpGet]
        public async Task<IActionResult> Edit(Guid notebookId, Guid? parentFolderId, Guid folderId)
        {
            bool isEditedDirectlyFromNotebook = folderId == Guid.Empty || folderId == null &&
                parentFolderId == Guid.Empty || parentFolderId == null;

            // Fetches the folder from the database
            Folder? folder = await context.Folders
                .Where(f => f.Id == folderId)
                .AsNoTracking() // Do not track changes for efficiency
                .FirstOrDefaultAsync(); // Retrieve the first match or null if not found

            //TODO: SIMPLIFY IF POSSIBLE
            if (folder is null)
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

            // Creates a view model for the folder to be edited
            var model = new FolderEditViewModel()
            {
                Title = folder.Title,
                NotebookId = notebookId,
                ParentFolderId = parentFolderId,
                FolderId = folderId,
                IsEditedDirectlyFromNotebook = isEditedDirectlyFromNotebook
            };

            Guid newParentFolderId = Guid.Empty;

            if (parentFolderId != Guid.Empty && parentFolderId != null)
            {
                newParentFolderId = await GetValue(parentFolderId, newParentFolderId);
            }

            // Stores the data for the view to access
            ViewData["NotebookId"] = notebookId;
            ViewData["ParentFolderId"] = parentFolderId;
            ViewData["FolderId"] = folderId;

            ViewData["NewParentFolderId"] = newParentFolderId;

            // Returns the Edit view with the folder data
            return View(model);
        }

        // POST method to handle form submission for editing a folder
        [HttpPost]
        public async Task<IActionResult> Edit(FolderEditViewModel model)
        {
            // Check if the model is valid; if not, return the view with the current model data
            if (ModelState.IsValid)
            {
                // Fetch the folder from the database using the provided parentFolderId
                var folder = await context.Folders.FindAsync(model.FolderId);

                // If the folder is not found, redirect to the Index view
                if (folder is null)
                {
                    return RedirectToAction(nameof(Index));
                }

                // Update the folder's title with the new value from the form
                folder.Title = model.Title;

                // Save the changes to the database
                await context.SaveChangesAsync();

                var parentFolder = await context.Folders.FindAsync(model.ParentFolderId);

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
                    // Redirects to the main notebook view if no parent folder is specified
                    return Redirect($"~/Folder?notebookId={model.NotebookId}");
                }
            }

            return View(model);                 // If the form submission is invalid, return the form with errors
        }

        // GET method to display the details of a specific folder
        [HttpGet]
        public async Task<IActionResult> Details(Guid notebookId, Guid? parentFolderId, Guid folderId)
        {
            //bool isIdValid = Guid.TryParse(strNotebookId, out Guid notebookId);
            //if (isIdValid is false)
            //{
            //    return RedirectToAction(nameof(Index));
            //}
            bool isClickedDirectlyFromNotebook = folderId == Guid.Empty || folderId == null &&
                parentFolderId == Guid.Empty || parentFolderId == null;

            // Retrieve the folder from the database based on the provided parentFolderId
            Folder? folder = await context.Folders
                .Where(f => f.Id == folderId)
                .AsNoTracking()  // Do not track changes for efficiency, since this is a read-only operation
                .FirstOrDefaultAsync();

            // If the folder is not found, redirect to the Index view
            if (folder is null)
            {
                return RedirectToAction(nameof(Index));
            }

            // Create a view model for displaying folder details
            FolderDetailsViewModel? model = new FolderDetailsViewModel()
            {
                Description = folder.Description,  // Populate the description field from the retrieved folder
                NotebookId = notebookId,
                ParentFolderId = parentFolderId,
                FolderId = folderId,
                IsClickedDirectlyFromNotebook = isClickedDirectlyFromNotebook
            };

            Guid newParentFolderId = Guid.Empty;

            if (parentFolderId != Guid.Empty && parentFolderId != null)
            {
                newParentFolderId = await GetValue(parentFolderId, newParentFolderId);
            }

            // Store the folder's title in ViewData for use in the view
            ViewData["Title"] = folder.Title;

            // Stores the data for the view to access
            ViewData["NotebookId"] = notebookId;
            ViewData["ParentFolderId"] = parentFolderId;
            ViewData["FolderId"] = folderId;

            ViewData["NewParentFolderId"] = newParentFolderId;

            // Return the view with the folder details model
            return View(model);
        }

        // POST method to handle form submission for updating folder details
        [HttpPost]
        public async Task<IActionResult> Details(FolderDetailsViewModel model)
        {
            // Check if the form data is valid; if not, return the form view with the current model
            if (ModelState.IsValid)
            {
                // Retrieve the folder from the database based on the provided parentFolderId
                Folder? folder = await context.Folders.FindAsync(model.FolderId);

                // If the folder is not found, redirect to the Index view
                if (folder is null)
                {
                    return RedirectToAction(nameof(Index));
                }

                // Update the folder's description with the new value from the model
                folder.Description = model.Description;

                // Save changes to the database
                await context.SaveChangesAsync();

                Folder? parentFolder = await context.Folders.FindAsync(model.ParentFolderId);

                if (parentFolder != null && parentFolder.ParentFolderId != Guid.Empty && parentFolder.ParentFolderId != null &&
                    parentFolder.Id != Guid.Empty && parentFolder.Id != null)
                {
                    return Redirect(
                        $"~/Folder?notebookId={parentFolder.NotebookId}&parentFolderId={parentFolder.ParentFolderId}&folderId={parentFolder.Id}");
                }
                //else if (folder.Id != Guid.Empty && model.IsMadeDirectlyFromNotebook == false)
                else if (parentFolder != null && parentFolder.Id != Guid.Empty && parentFolder.Id != null &&
                         model.IsClickedDirectlyFromNotebook == false)
                {
                    return Redirect($"~/Folder?notebookId={parentFolder.NotebookId}&folderId={parentFolder.Id}");
                }
                else if (model.NotebookId != Guid.Empty && model.NotebookId != null)
                {
                    // Redirects to the main notebook view if no parent folder is specified
                    return Redirect($"~/Folder?notebookId={model.NotebookId}");
                }
            }

            return View(model);
        }

        private async Task<Guid> GetValue(Guid? parentFolderId, Guid newParentFolderId)
        {
            var folder = await context.Folders
                .SelectMany(f => f.ChildrenFolders)
                .Where(f => f.Id == parentFolderId)
                .FirstOrDefaultAsync();

            if (folder != null)
            {
                if (folder.ParentFolderId != Guid.Empty && folder.ParentFolderId != null)
                {
                    newParentFolderId = folder.ParentFolderId.Value;
                }
            }

            return newParentFolderId;
        }

        private async Task<(Guid newParentFolderId, Guid newFolderId)> GetValue(Guid? parentFolderId, Guid newParentFolderId, Guid newFolderId)
        {
            var folder = await context.Folders
                .SelectMany(f => f.ChildrenFolders)
                .Where(f => f.Id == parentFolderId)
                .FirstOrDefaultAsync();

            if (folder != null)
            {
                if (folder.ParentFolderId != Guid.Empty && folder.ParentFolderId != null)
                {
                    newParentFolderId = folder.ParentFolderId.Value;
                }
            }
            else if (folder is null && parentFolderId is not null)
            {
                newFolderId = (Guid)parentFolderId;
            }

            return (newParentFolderId, newFolderId);
        }

        public async Task DeleteFolderWithChildrenAsync(Guid folderId)
        {
            var folder = await context.Folders
                .Include(f => f.Notes) // Include Notes to delete them
                .Include(f => f.ChildrenFolders) // Include child folders to delete them later
                .FirstOrDefaultAsync(f => f.Id == folderId);

            if (folder == null)
            {
                throw new InvalidOperationException("Folder not found.");
            }

            // Create a copy of the ChildrenFolders collection to iterate safely
            var childFolders = folder.ChildrenFolders.ToList();

            // First delete all child folders (recursively)
            foreach (var childFolder in childFolders)
            {
                await DeleteFolderWithChildrenAsync(childFolder.Id); // Recursively delete child folders
            }

            // Then delete all Notes inside the current folder
            context.Notes.RemoveRange(folder.Notes);

            // Finally, delete the current folder
            context.Folders.Remove(folder);

            // Save changes to the database
            await context.SaveChangesAsync();
        }
    }
}