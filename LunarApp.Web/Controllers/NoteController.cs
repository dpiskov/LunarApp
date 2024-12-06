﻿using LunarApp.Data;
using LunarApp.Data.Models;
using LunarApp.Services.Data.Interfaces;
using LunarApp.Web.ViewModels.Note;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LunarApp.Web.Controllers
{
    // TODO'S
    // TODO: Handle exceptions

    [Authorize]
    public class NoteController(ApplicationDbContext context) : Controller
    public class NoteController(ApplicationDbContext context, 
        INoteService noteService) : Controller
    {
        [HttpGet]
        public async Task<IActionResult> Create(Guid notebookId, Guid? parentFolderId, Guid? folderId)
        {
            NoteCreateViewModel model = await noteService.GetCreateNoteAsync(notebookId, parentFolderId, folderId);

            ViewData["NotebookId"] = notebookId;
            ViewData["ParentFolderId"] = parentFolderId;
            ViewData["FolderId"] = folderId;

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(NoteCreateViewModel model)
        {
            if (ModelState.IsValid == false)
            {
                return View(model);
            }

            (bool isSuccess, string? errorMessage) = await noteService.CreateNoteAsync(model);

            if (isSuccess == false)
            {
                ModelState.AddModelError(string.Empty, errorMessage ?? "An unknown error occurred.");
                return View(model);
            }

            ViewData["NotebookId"] = model.NotebookId;
            ViewData["ParentFolderId"] = model.ParentFolderId;
            ViewData["FolderId"] = model.FolderId;

            return RedirectToFolderIndexView(model.NotebookId, model.ParentFolderId, model.FolderId);
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