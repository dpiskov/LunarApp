using Humanizer;
using LunarApp.Web.Data;
using LunarApp.Web.Data.Models;
using LunarApp.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LunarApp.Web.Controllers
{
    public class NoteController(ApplicationDbContext context) : Controller
    {
        public async Task<IActionResult> Index(Guid notebookId, Guid? parentFolderId)
        {
            IEnumerable<NoteViewModel> notes;

            notes = await context.Notes
                .Where(n => n.FolderId == parentFolderId)
                .Select(n => new NoteViewModel
                {
                    Title = n.Title,
                    Body = n.Body,
                    NotebookId = n.NotebookId,
                    FolderId = n.FolderId
                })
                .ToListAsync();

            notes = await context.Notes
                .Where(n => n.NotebookId == notebookId && n.FolderId == null)
                .Select(n => new NoteViewModel
                {
                    Title = n.Title,
                    Body = n.Body,
                    NotebookId = n.NotebookId
                })
                .ToListAsync();

            ViewData["NotebookId"] = notebookId;
            ViewData["Title"] = "Notes in Notebook";

            return RedirectToAction(nameof(Index), "Notebook");
        }

        [HttpGet]
        public async Task<IActionResult> Create(Guid notebookId, Guid? parentFolderId)
        {
            var model = new NoteViewModel
            {
                NotebookId = notebookId,
                FolderId = parentFolderId
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(NoteViewModel model)
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
                if (model.FolderId != Guid.Empty)
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
                    FolderId = model.FolderId != Guid.Empty ? model.FolderId : null // Set FolderId only if provided
                };

                await context.Notes.AddAsync(note);
                await context.SaveChangesAsync();

                // Redirect to the appropriate Index based on whether FolderId is set or not
                if (model.FolderId != Guid.Empty)
                {
                    return RedirectToAction(nameof(Index), "Folder", new { parentFolderId = model.FolderId, notebookId = model.NotebookId });
                    //return RedirectToAction(nameof(Index), "Folder", new { folderId = model.FolderId, notebookId = model.NotebookId });
                }

                return RedirectToAction(nameof(Index), "Folder", new { notebookId = model.NotebookId });
            }

            // If the model is invalid, return to the same view
            return View(model);
        }

        //public async Task<IActionResult> Index(Guid notebookId, Guid folderId)
        //{
        //    IEnumerable<NoteViewModel> notes;

        //    notes = await context.Notes
        //        .Where(n => n.FolderId == folderId)
        //        .Select(n => new NoteViewModel
        //        {
        //            Title = n.Title,
        //            Body = n.Body,
        //            NotebookId = n.NotebookId,
        //            FolderId = n.FolderId
        //        })
        //        .ToListAsync();

        //    notes = await context.Notes
        //        .Where(n => n.NotebookId == notebookId && n.FolderId == null)
        //        .Select(n => new NoteViewModel
        //        {
        //            Title = n.Title,
        //            Body = n.Body,
        //            NotebookId = n.NotebookId
        //        })
        //        .ToListAsync();

        //    ViewData["NotebookId"] = notebookId;
        //    ViewData["Title"] = "Notes in Notebook";

        //    return RedirectToAction(nameof(Index), "Notebook");
        //}
    }
}