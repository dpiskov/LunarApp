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
        public async Task<IActionResult> Index(Guid notebookId, Guid? parentFolderId)
        {
            // Holds the list of folders to be displayed
            IEnumerable<FolderInfoViewModel> folders;
            IEnumerable<NoteInfoViewModel> notes = new List<NoteInfoViewModel>();

            // Checks if a parent folder ID is provided and is valid
            if (parentFolderId.HasValue && parentFolderId.Value != Guid.Empty)
            {
                // Fetches folders inside the specified parent folder
                folders = await context.Folders
                    .Where(f => f.ParentFolderId == parentFolderId.Value)       // Filters folders by parentFolderId
                    .Select(f => new FolderInfoViewModel                        // Selects needed folder details for the view model
                    {
                        Id = f.Id,
                        Title = f.Title,
                        NotebookId = f.NotebookId,
                        ParentFolderId = f.ParentFolderId
                    })
                    .ToListAsync();

                notes = await context.Notes
                    .Where(n => n.FolderId == parentFolderId.Value)
                    .Select(n => new NoteInfoViewModel
                    {
                        Id = n.Id,
                        Title = n.Title,
                        FolderId = n.FolderId
                    })
                    .ToListAsync();

                // Sets data for the view to access
                ViewData["NotebookId"] = notebookId;
                ViewData["ParentFolderId"] = parentFolderId;
                ViewData["Title"] = "Folders in Folder";
            }
            // If no parent folder, checks for a valid notebook ID
            else if (notebookId != Guid.Empty)
            {
                // Fetches folders that belong to the notebook and have no parent folder
                folders = await context.Folders
                    .Where(f => f.NotebookId == notebookId && f.ParentFolderId == null)
                    .Select(f => new FolderInfoViewModel
                    {
                        Id = f.Id,
                        Title = f.Title,
                        NotebookId = f.NotebookId
                    })
                    .ToListAsync();

                notes = await context.Notes
                    .Where(n => n.FolderId == null)
                    .Select(n => new NoteInfoViewModel
                    {
                        Id = n.Id,
                        Title = n.Title,
                        FolderId = n.FolderId
                    })
                    .ToListAsync();

                // Sets additional data for the view
                ViewData["NotebookId"] = notebookId;
                ViewData["Title"] = "Folders in Notebook";
            }
            else
            {
                // Returns an error if neither notebookId nor parentFolderId is provided
                return BadRequest("NotebookId or ParentFolderId must be provided.");
            }

            // Returns the view with the list of folders
            return View(new FolderNotesViewModel
            {
                Folders = folders,
                Notes = notes
            });
        }

        // GET method to render the form for creating a new folder
        [HttpGet]
        public async Task<IActionResult> Create(Guid notebookId, Guid? parentFolderId)
        {
            // Creates a view model with notebookId and optional parentFolderId
            var model = new FolderViewModel
            {
                NotebookId = notebookId,
                ParentFolderId = parentFolderId
            };

            // Returns the form view with the model data
            return View(model);
        }

        // POST method to handle form submission for creating a new folder
        [HttpPost]
        public async Task<IActionResult> Create(FolderViewModel model)
        {
            // Checks if the specified notebook exists
            var notebookExists = await context.Notebooks.AnyAsync(n => n.Id == model.NotebookId);

            if (!notebookExists)                        // If the notebook doesn't exist
            {
                ModelState.AddModelError(string.Empty, "The specified notebook does not exist.");
                return View(model);                     // Returns the form view with an error message
            }

            // Creates a new Folder object with unique ID and data from the form
            Folder folder = new Folder
            {
                Title = model.Title,
                NotebookId = model.NotebookId,
                ParentFolderId = model.ParentFolderId
            };

            await context.Folders.AddAsync(folder);     // Adds the folder to the database
            await context.SaveChangesAsync();           // Saves changes to the database

            // Redirects to the parent folder view if a parent folder is specified
            if (folder.ParentFolderId.HasValue)
            {
                return Redirect($"~/Folder?parentFolderId={folder.ParentFolderId.Value}&notebookId={model.NotebookId}");
            }

            // Redirects to the main notebook view if no parent folder is specified
            return Redirect($"~/Folder?notebookId={model.NotebookId}");

            //return RedirectToAction("Index", "Notebook");


        }

        [HttpGet]
        public async Task<IActionResult> Remove(Guid notebookId, Guid? parentFolderId)
        {
            FolderDeleteViewModel? model;

            // Check if a parent folder ID is provided and is valid
            if (parentFolderId.HasValue && parentFolderId.Value != Guid.Empty)
            {
                //var id = await context.Folders.FirstOrDefaultAsync(f => f.Id == parentFolderId.Value);
                // Retrieve folder details for the specified parent folder
                model = await context.Folders
                    .Where(f => f.Id == parentFolderId.Value)                  // Filter by parent folder ID
                    .AsNoTracking()                                                              // Do not track changes for efficiency
                    .Select(f => new FolderDeleteViewModel()
                    {
                        Title = f.Title,
                        NotebookId = f.NotebookId,
                        ParentFolderId = f.Id
                    })
                    .FirstOrDefaultAsync();                                                      // Get the first match or null if none

                // Store notebook and parent folder IDs in ViewData for use in the view
                ViewData["NotebookId"] = notebookId;
                ViewData["ParentFolderId"] = parentFolderId;
            }
            // Check if a notebook ID is provided and is valid
            else if (notebookId != Guid.Empty)
            {
                // Retrieve folder details for the specified notebook (without parent folder)
                model = await context.Folders
                    .Where(f => f.NotebookId == notebookId && f.ParentFolderId == null)    // Filter by notebook ID
                    .Select(f => new FolderDeleteViewModel()
                    {
                        Title = f.Title,
                        NotebookId = f.NotebookId
                    })
                    .FirstOrDefaultAsync();                                                      // Get the first match or null if none

                // Store notebook ID in ViewData for use in the view
                ViewData["NotebookId"] = notebookId;
            }
            else
            {
                // Return a bad request if neither notebookId nor parentFolderId is provided
                return BadRequest("NotebookId or ParentFolderId must be provided.");
            }

            // Return the view with the folder model
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Remove(FolderDeleteViewModel model)
        {
            if (model == null)
            {
                // Return bad request if the model is invalid
                return BadRequest("Invalid model.");
            }

            Folder? folder = null;

            // If a parent folder ID is provided, try to find the folder in the database
            if (model.ParentFolderId != null)
            {
                folder = await context.Folders.FirstOrDefaultAsync(f => f.Id == model.ParentFolderId);

                //folder = await context.Folders
                //    .Where(pf => pf.Id == model.ParentFolderId)               // Filter by parent folder ID
                //    .FirstOrDefaultAsync();                                                     // Get the first match or null if none
            }
            // If a notebook ID is provided, try to find the folder for the notebook (without parent folder)
            else if (model.NotebookId != Guid.Empty)
            {
                folder = await context.Folders
                    .Where(f => f.NotebookId == model.NotebookId && f.ParentFolderId == null)  // Filter by notebook ID
                    .FirstOrDefaultAsync();                                                          // Get the first match or null if none
            }

            // If a folder is found, remove it from the database
            if (folder != null)
            {
                context.Folders.Remove(folder);                 // Remove the folder from the context

                await context.SaveChangesAsync();               // Save changes to the database

                // Redirects to the parent folder view if a parent folder is specified
                if (folder.ParentFolderId.HasValue)
                {
                    return Redirect($"~/Folder?parentFolderId={folder.ParentFolderId.Value}&notebookId={model.NotebookId}");
                }
            }

            // Redirect to the Index action, passing the notebook ID and parent folder ID
            //return RedirectToAction(nameof(Index), new { notebookId = model.NotebookId, parentFolderId = model.ParentFolderId });

            // Redirects to the main notebook view if no parent folder is specified
            return Redirect($"~/Folder?notebookId={model.NotebookId}");
        }
    }
}