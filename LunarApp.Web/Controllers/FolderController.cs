﻿using LunarApp.Web.Data;
using LunarApp.Web.Data.Models;
using LunarApp.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LunarApp.Web.Controllers
{
    public class FolderController(ApplicationDbContext context) : Controller
    {
        // GET method to display a list of folders
        public async Task<IActionResult> Index(Guid notebookId, Guid? parentFolderId, Guid? folderId)
        {
            // Holds the list of folders to be displayed
            IEnumerable<FolderInfoViewModel> folders;
            // Holds the list of notes to be displayed, initially empty
            IEnumerable<NoteInfoViewModel> notes;

            if (folderId != Guid.Empty && folderId != null)
            {
                // Fetches folders that are inside the specified parent folder
                folders = await context.Folders
                    .Where(f => f.ParentFolderId == folderId)       // Filters folders by parentFolderId
                    .Select(f => new FolderInfoViewModel                        // Maps folders to FolderInfoViewModel
                    {
                        Id = f.Id,
                        Title = f.Title,
                        NotebookId = f.NotebookId,
                        ParentFolderId = f.ParentFolderId
                    })
                    .ToListAsync();

                // Fetches notes that belong to the specified parent folder
                notes = await context.Notes
                .Where(n => n.FolderId == folderId)                  // Filters notes by parentFolderId
                .Select(n => new NoteInfoViewModel                               // Maps notes to NoteInfoViewModel
                {
                    Id = n.Id,
                    Title = n.Title,
                    NotebookId = n.NotebookId,
                    FolderId = n.FolderId
                })
                .ToListAsync();

                Guid newFolderId = Guid.Empty;
                Guid newParentFolderId = Guid.Empty;

                (newParentFolderId, newFolderId) = await GetValue(parentFolderId, newParentFolderId, newFolderId);

                // Fetches the title of the parent folder
                var parentFolderTitle = await context.Folders
                    .Where(f => f.Id == folderId)
                    .Select(f => f.Title)
                    .FirstOrDefaultAsync();

                // Stores the data for the view to access (for navigation and display)
                ViewData["Title"] = parentFolderTitle;

                ViewData["NotebookId"] = notebookId;
                ViewData["ParentFolderId"] = parentFolderId;
                ViewData["FolderId"] = folderId;

                ViewData["NewParentFolderId"] = newParentFolderId;
                ViewData["NewFolderId"] = newFolderId;
            }
            // If no parent folder is specified, checks if a valid notebook ID is provided
            else if (notebookId != Guid.Empty && notebookId != null)
            {
                // Fetches folders that belong to the specified notebook and do not have a parent folder
                folders = await context.Folders
                    .Where(f => f.NotebookId == notebookId && f.ParentFolderId == null)  // Filters folders by notebookId and no parent folder
                    .Select(f => new FolderInfoViewModel                                 // Projects folders to FolderInfoViewModel
                    {
                        Id = f.Id,
                        Title = f.Title,
                        NotebookId = f.NotebookId,
                        ParentFolderId = parentFolderId
                    })
                    .ToListAsync();

                // Fetches notes that do not belong to any folder
                notes = await context.Notes
                .Where(n => n.FolderId == null)                                  // Filters notes without a folder
                .Where(nb => nb.NotebookId == notebookId)
                    .Select(n => new NoteInfoViewModel
                    {
                        Id = n.Id,
                        Title = n.Title,
                        NotebookId = n.NotebookId,
                        FolderId = n.FolderId
                    })
                    .ToListAsync();

                // Fetches the title of the notebook
                var notebookTitle = await context.Notebooks
                    .Where(nb => nb.Id == notebookId)
                    .Select(nb => nb.Title)
                    .FirstOrDefaultAsync();

                // Stores the data for the view to access
                ViewData["NotebookId"] = notebookId;
                ViewData["Title"] = notebookTitle;
            }
            else
            {
                // Returns an error if neither notebookId nor parentFolderId is provided
                return BadRequest("NotebookId or ParentFolderId must be provided.");
            }

            // Returns the view with the list of folders and notes
            return View(new FolderNotesViewModel
            {
                Folders = folders,                    // List of folders to display
                Notes = notes                         // List of notes to display
            });
        }

        // GET method to render the form for creating a new folder
        [HttpGet]
        public async Task<IActionResult> AddFolder(Guid notebookId, Guid? parentFolderId, Guid? folderId)
        {
            bool isMadeDirectlyFromNotebook = folderId == Guid.Empty || folderId == null &&
                parentFolderId == Guid.Empty || parentFolderId == null;

            FolderCreateViewModel model = new FolderCreateViewModel
            {
                Title = string.Empty,
                NotebookId = notebookId,
                ParentFolderId = parentFolderId,
                FolderId = folderId,
                IsMadeDirectlyFromNotebook = isMadeDirectlyFromNotebook
            };

            // Passes notebookId and parentFolderId to the ViewData to be used in the view
            ViewData["NotebookId"] = notebookId;
            ViewData["ParentFolderId"] = parentFolderId;
            ViewData["FolderId"] = folderId;

            // Returns the form view with the model data
            return View(model);
        }

        // POST method to handle form submission for creating a new folder
        [HttpPost]
        public async Task<IActionResult> AddFolder(FolderCreateViewModel model)
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

                await context.Folders.AddAsync(folder); // Adds the folder to the database
                await context.SaveChangesAsync(); // Saves changes to the database

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

        [HttpPost]
        public async Task<IActionResult> Remove(FolderDeleteViewModel model)
        {
            // Check if the model is null or invalid
            if (model == null)
            {
                // Return bad request if the model is invalid
                return BadRequest("Invalid model.");
            }

            Folder? folder = null;

            // If a parent folder ID is provided, try to find the folder and its related entities in the database
            if (model.ParentFolderId != null)
            {
                folder = await context.Folders
                    .Include(f => f.Notes)                                    // Include related notes for the folder
                    .Include(f => f.ChildrenFolders)                          // Include related child folders
                    .FirstOrDefaultAsync(f => f.Id == model.ParentFolderId);
            }
            // If no parent folder is provided but a notebook ID is provided, find the folder for that notebook
            else if (model.NotebookId != Guid.Empty)
            {
                folder = await context.Folders
                    .Include(f => f.Notes)                                    // Include related notes for the folder
                    .Include(f => f.ChildrenFolders)                          // Include related child folders
                    .Where(f => f.NotebookId == model.NotebookId && f.ParentFolderId == null)    // Filter by notebook ID
                    .FirstOrDefaultAsync();                                                            // Retrieve the first match or null if not found
            }

            // If the folder is found, remove it from the database
            if (folder != null)
            {
                // Remove any child folders if they exist
                if (folder.ChildrenFolders != null && folder.ChildrenFolders.Any())
                {
                    context.Folders.RemoveRange(folder.ChildrenFolders.ToList()); // Remove all child folders
                }

                // Remove any notes if they exist within the folder
                if (folder.Notes != null && folder.Notes.Any())
                {
                    context.Notes.RemoveRange(folder.Notes);                      // Remove all notes associated with the folder
                }

                context.Folders.Remove(folder);                 // Remove the folder from the context

                await context.SaveChangesAsync();               // Save changes to the database

                // Redirect to the parent folder view if the folder has a parent folder ID
                if (folder.ParentFolderId.HasValue)
                {
                    return Redirect($"~/Folder?parentFolderId={folder.ParentFolderId.Value}&notebookId={model.NotebookId}");
                }
            }

            // If no parent folder is specified, redirect to the main notebook view
            return Redirect($"~/Folder?notebookId={model.NotebookId}");
        }

        // GET method to display the folder edit form
        [HttpGet]
        public async Task<IActionResult> Edit(Guid notebookId, Guid parentFolderId)
        {
            // Fetches the folder from the database
            var folder = await context.Folders
                .Where(f => f.Id == parentFolderId)
                .AsNoTracking()                                 // Do not track changes for efficiency
                .FirstOrDefaultAsync();                         // Retrieve the first match or null if not found

            // If the folder does not exist, redirect to the Index view
            if (folder is null)
            {
                return RedirectToAction(nameof(Index));
            }

            // Creates a view model for the folder to be edited
            var model = new FolderViewModel
            {
                Title = folder.Title
            };

            // Stores the data for the view to access
            //ViewData["NotebookId"] = notebookId;
            //ViewData["ParentFolderId"] = parentFolderId;

            // Returns the Edit view with the folder data
            return View(model);
        }

        // POST method to handle form submission for editing a folder
        [HttpPost]
        public async Task<IActionResult> Edit(FolderViewModel model, Guid notebookId, Guid parentFolderId)
        {
            // Check if the model is valid; if not, return the view with the current model data
            if (ModelState.IsValid is false)
            {
                return View(model);                 // If the form submission is invalid, return the form with errors
            }

            // Fetch the folder from the database using the provided parentFolderId
            var folder = await context.Folders.FindAsync(parentFolderId);

            // If the folder is not found, redirect to the Index view
            if (folder is null)
            {
                return RedirectToAction(nameof(Index));
            }

            // Update the folder's title with the new value from the form
            folder.Title = model.Title;

            // Save the changes to the database
            await context.SaveChangesAsync();

            // If the folder has a parent folder, redirect to the parent folder's view
            if (folder.ParentFolderId.HasValue)
            {
                return RedirectToAction(nameof(Index), "Folder", new { parentFolderId = folder.ParentFolderId, notebookId = folder.NotebookId });
                //return RedirectToAction(nameof(Index), "Folder", new { folderId = model.FolderId, notebookId = model.NotebookId });
            }

            // If no parent folder exists, redirect to the main notebook view
            return RedirectToAction(nameof(Index), "Folder", new { notebookId = folder.NotebookId });
        }

        // GET method to display the details of a specific folder
        [HttpGet]
        public async Task<IActionResult> Details(Guid notebookId, Guid parentFolderId)
        {
            // Retrieve the folder from the database based on the provided parentFolderId
            var folder = await context.Folders
                .Where(f => f.Id == parentFolderId)
                .AsNoTracking()  // Do not track changes for efficiency, since this is a read-only operation
                .FirstOrDefaultAsync();

            // If the folder is not found, redirect to the Index view
            if (folder is null)
            {
                return RedirectToAction(nameof(Index));
            }

            // Create a view model for displaying folder details
            var model = new FolderDetailsViewModel()
            {
                Description = folder.Description  // Populate the description field from the retrieved folder
            };

            // Retrieve the folder's title for display purposes
            var folderTitle = await context.Folders
                .Where(f => f.Id == parentFolderId)
                .Select(f => f.Title)
                .FirstOrDefaultAsync();

            // Store the folder's title in ViewData for use in the view
            ViewData["Title"] = folderTitle;

            // Return the view with the folder details model
            return View(model);
        }

        // POST method to handle form submission for updating folder details
        [HttpPost]
        public async Task<IActionResult> Details(FolderDetailsViewModel model, Guid notebookId, Guid parentFolderId)
        {
            // Check if the form data is valid; if not, return the form view with the current model
            if (ModelState.IsValid is false)
            {
                return View(model);
            }

            // Retrieve the folder from the database based on the provided parentFolderId
            var folder = await context.Folders.FindAsync(parentFolderId);

            // If the folder is not found, redirect to the Index view
            if (folder is null)
            {
                return RedirectToAction(nameof(Index));
            }

            // Update the folder's description with the new value from the model
            folder.Description = model.Description;

            // Save changes to the database
            await context.SaveChangesAsync();

            // If the folder has a parent folder, redirect to the parent folder view
            if (folder.ParentFolderId.HasValue)
            {
                return Redirect($"~/Folder?parentFolderId={folder.ParentFolderId.Value}&notebookId={folder.NotebookId}");
            }

            // If no parent folder is specified, redirect to the main notebook view
            return Redirect($"~/Folder?notebookId={folder.NotebookId}");
        }
    }
}