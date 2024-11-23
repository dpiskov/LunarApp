﻿using Humanizer;
using LunarApp.Web.Data;
using LunarApp.Web.Data.Models;
using LunarApp.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LunarApp.Web.Controllers
{
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