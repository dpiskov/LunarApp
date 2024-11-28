using System.Globalization;
using LunarApp.Data;
using LunarApp.Data.Models;
using LunarApp.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using static LunarApp.Common.ValidationConstants.Note;

namespace LunarApp.Web.Controllers
{
    // TODO'S
    // TODO: Handle exceptions

    [Authorize]
    public class NoteController(ApplicationDbContext context) : Controller
    {
        [HttpGet]
        public IActionResult Create(Guid notebookId, Guid? parentFolderId, Guid? folderId)
        {
            NoteCreateViewModel? model = null;

            if (folderId != Guid.Empty && folderId != null &&
                parentFolderId != Guid.Empty && parentFolderId != null)
            {
                model = new NoteCreateViewModel
                {
                    Title = string.Empty,
                    NotebookId = notebookId,
                    ParentFolderId = parentFolderId,
                    FolderId = folderId,
                };
            }
            else if (folderId != Guid.Empty && folderId != null)
            {
                model = new NoteCreateViewModel
                {
                    Title = string.Empty,
                    NotebookId = notebookId,
                    FolderId = folderId,
                };
            }
            else if (parentFolderId != Guid.Empty && parentFolderId != null)
            {
                model = new NoteCreateViewModel
                {
                    Title = string.Empty,
                    NotebookId = notebookId,
                    FolderId = parentFolderId,
                };
            }
            else if (notebookId != Guid.Empty && notebookId != null)
            {
                model = new NoteCreateViewModel
                {
                    Title = string.Empty,
                    NotebookId = notebookId
                };
            }

            ViewData["NotebookId"] = notebookId;
            ViewData["ParentFolderId"] = parentFolderId;
            ViewData["FolderId"] = folderId;

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(NoteCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Check if the notebook exists
                var notebook = await context.Notebooks.FindAsync(model.NotebookId);
                if (notebook is null)
                {
                    ModelState.AddModelError(string.Empty, "The selected notebook does not exist.");
                    return View(model);
                }

                // Check if the folder exists, but only if a folder ID is provided
                if (model.FolderId != Guid.Empty && model.FolderId != null)
                {
                    var folder = await context.Folders.FindAsync(model.FolderId);
                    if (folder is null)
                    {
                        ModelState.AddModelError(string.Empty, "The selected folder does not exist.");
                        return View(model);
                    }
                }

                // Create the new note
                var note = new Note
                {
                    Title = model.Title,
                    Body = model.Body,
                    NotebookId = model.NotebookId,
                    Notebook = notebook,
                    FolderId = model.FolderId,
                    DateCreated = model.DateCreated,
                    LastSaved = model.LastSaved
                };

                await context.Notes.AddAsync(note);
                await context.SaveChangesAsync();

                if (model.FolderId != Guid.Empty && model.FolderId != null &&
                    model.ParentFolderId != Guid.Empty && model.FolderId != null)
                {
                    return RedirectToAction(nameof(Index), "Folder", new { notebookId = model.NotebookId, parentFolderId = model.ParentFolderId, folderId = model.FolderId });
                }
                // Redirect to the appropriate Index based on whether FolderId is set or not
                if (model.FolderId != Guid.Empty && model.FolderId != null)
                {
                    return RedirectToAction(nameof(Index), "Folder", new { notebookId = model.NotebookId, folderId = model.FolderId });
                    //return RedirectToAction(nameof(Index), "Folder", new { folderId = model.FolderId, notebookId = model.NotebookId });
                }

                return RedirectToAction(nameof(Index), "Folder", new { notebookId = model.NotebookId });
            }

            ViewData["NotebookId"] = model.NotebookId;
            ViewData["ParentFolderId"] = model.ParentFolderId;
            ViewData["FolderId"] = model.FolderId;

            // If the model is invalid, return to the same view
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Remove(Guid notebookId, Guid? parentFolderId, Guid? folderId, Guid noteId)
        {
            NoteDeleteViewModel? model = await context.Notes
                .Where(n => n.Id == noteId)
                .AsNoTracking()
                .Select(n => new NoteDeleteViewModel()
                {
                    Id = n.Id,
                    Title = n.Title,
                    NotebookId = n.NotebookId,
                    ParentFolderId = parentFolderId,
                    FolderId = n.FolderId
                })
                .FirstOrDefaultAsync();

            ViewData["NotebookId"] = notebookId;
            ViewData["ParentFolderId"] = parentFolderId;
            ViewData["FolderId"] = folderId;

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Remove(NoteDeleteViewModel model)
        {
            if (ModelState.IsValid)
            {
                Note? note = await context.Notes
                    .Where(n => n.Id == model.Id)
                    .FirstOrDefaultAsync();

                if (note != null)
                {
                    context.Notes.Remove(note);

                    await context.SaveChangesAsync();
                }

                if (note.FolderId != Guid.Empty && note.FolderId != null &&
                    model.ParentFolderId != Guid.Empty && model.ParentFolderId != null)
                {
                    return Redirect($"~/Folder?notebookId={model.NotebookId}&parentFolderId={model.ParentFolderId}&folderId={note.FolderId}");
                }
                else if (note.FolderId != Guid.Empty && note.FolderId != null)
                {
                    return Redirect($"~/Folder?notebookId={model.NotebookId}&folderId={note.FolderId}");
                }
                else if (model.NotebookId != Guid.Empty && model.NotebookId != null)
                {
                    return Redirect($"~/Folder?notebookId={model.NotebookId}");
                }
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid notebookId, Guid? parentFolderId, Guid? folderId, Guid noteId)
        {
            var note = await context.Notes
                .Where(n => n.Id == noteId)
                .AsNoTracking()
                .FirstOrDefaultAsync();

            if (note == null)
            {
                // TODO: FIX ROUTING
                return RedirectToAction(nameof(Index), "Notebook");
            }

            var model = new NoteEditViewModel
            {
                Id = noteId,
                Title = note.Title,
                Body = note.Body,
                NotebookId = notebookId,
                ParentFolderId = parentFolderId,
                FolderId = folderId,
                DateCreated = note.DateCreated,
                LastSaved = note.LastSaved
            };

            //TODO: Format LastSaved properly
            ViewData["DateCreated"] = note.DateCreated;
            ViewData["LastSaved"] = note.LastSaved;

            ViewData["NotebookId"] = notebookId;
            ViewData["ParentFolderId"] = parentFolderId;
            ViewData["FolderId"] = folderId;

            ViewData["Title"] = note.Title;

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(NoteEditViewModel model)
        {
            if (ModelState.IsValid)
            {
                var note = await context.Notes.FindAsync(model.Id);

                if (note == null)
                {
                    // TODO: TEMPORARY PATCH?
                    if (model.FolderId != Guid.Empty && model.FolderId != null &&
                        model.ParentFolderId != Guid.Empty && model.ParentFolderId != null)
                    {
                        return Redirect($"~/Folder?notebookId={model.NotebookId}&parentFolderId={model.ParentFolderId}&folderId={model.FolderId}");
                    }
                    else if (model.FolderId != Guid.Empty && model.FolderId != null)
                    {
                        return Redirect($"~/Folder?notebookId={model.NotebookId}&folderId={model.FolderId}");
                    }
                    else if (model.NotebookId != Guid.Empty && model.NotebookId != null)
                    {
                        // If no parent folder is specified, redirect to the main notebook view
                        return Redirect($"~/Folder?notebookId={model.NotebookId}");
                    }
                }
                else
                {
                    note.Title = model.Title;
                    note.Body = model.Body;
                    note.LastSaved = DateTime.Now;
                }

                await context.SaveChangesAsync();

                if (model.FolderId != Guid.Empty && model.FolderId != null &&
                    model.ParentFolderId != Guid.Empty && model.ParentFolderId != null)
                {
                    return Redirect($"~/Folder?notebookId={model.NotebookId}&parentFolderId={model.ParentFolderId}&folderId={model.FolderId}");
                }
                else if (model.FolderId != Guid.Empty && model.FolderId != null)
                {
                    return Redirect($"~/Folder?notebookId={model.NotebookId}&folderId={model.FolderId}");
                }
                else if (model.NotebookId != Guid.Empty && model.NotebookId != null)
                {
                    // If no parent folder is specified, redirect to the main notebook view
                    return Redirect($"~/Folder?notebookId={model.NotebookId}");
                }
            }

            return View(model);
        }
    }
}