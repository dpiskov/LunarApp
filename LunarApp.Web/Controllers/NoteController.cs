using LunarApp.Data.Models;
using LunarApp.Services.Data.Interfaces;
using LunarApp.Web.ViewModels.Note;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace LunarApp.Web.Controllers
{
    [Authorize]
    public class NoteController(
        INoteService noteService,
        UserManager<ApplicationUser> userManager
        ) : BaseController(userManager)
    {
        // GET method to render the form for creating a new note within a specific notebook and folder
        [HttpGet]
        public async Task<IActionResult> Create(Guid notebookId, Guid? parentFolderId, Guid? folderId)
        {
            Guid currentUserId = GetCurrentUserId();
            NoteCreateViewModel model = await noteService.GetCreateNoteAsync(notebookId, parentFolderId, folderId, currentUserId);

            ViewData["NotebookId"] = notebookId;
            ViewData["ParentFolderId"] = parentFolderId;
            ViewData["FolderId"] = folderId;

            return View(model);
        }

        // POST method to handle form submission for creating a new note
        [HttpPost]
        public async Task<IActionResult> Create(NoteCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                Guid currentUserId = GetCurrentUserId();

                (bool isSuccess, string? errorMessage) = await noteService.CreateNoteAsync(model, currentUserId);

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

            return View(model);
        }

        // GET method to render the confirmation page for deleting a note
        [HttpGet]
        public async Task<IActionResult> Remove(Guid notebookId, Guid? parentFolderId, Guid? folderId, Guid noteId)
        {
            Guid currentUserId = GetCurrentUserId();

            NoteDeleteViewModel? model = await noteService.GetNoteForDeleteByIdAsync(notebookId, parentFolderId, folderId, noteId, currentUserId);

            if (model == null)
            {
                return RedirectToFolderIndexView(notebookId, parentFolderId, folderId);
            }

            ViewData["NotebookId"] = notebookId;
            ViewData["ParentFolderId"] = parentFolderId;
            ViewData["FolderId"] = folderId;

            return View(model);
        }

        // POST method to delete a note from the database
        [HttpPost]
        public async Task<IActionResult> Remove(NoteDeleteViewModel model)
        {
            if (ModelState.IsValid)
            {
                Guid currentUserId = GetCurrentUserId();

                await noteService.DeleteNoteAsync(model.Id, currentUserId);

                return RedirectToFolderIndexView(model.NotebookId, model.ParentFolderId, model.FolderId);
            }

            return View(model);
        }

        // GET method to render the form for editing an existing note
        [HttpGet]
        public async Task<IActionResult> Edit(Guid notebookId, Guid? parentFolderId, Guid? folderId, Guid noteId)
        {
            Guid currentUserId = GetCurrentUserId();

            NoteEditViewModel? model = await noteService.GetNoteForEditByIdAsync(notebookId, parentFolderId, folderId, noteId, currentUserId);

            if (model == null)
            {
                return RedirectToAction(nameof(Index), "Notebook");
            }

            ViewData["DateCreated"] = model.DateCreated;
            ViewData["LastSaved"] = model.LastSaved;

            ViewData["NotebookId"] = notebookId;
            ViewData["ParentFolderId"] = parentFolderId;
            ViewData["FolderId"] = folderId;
            ViewData["NoteId"] = noteId;

            ViewData["Title"] = model.Title;

            return View(model);
        }

        // POST method to handle form submission for updating a note's information
        [HttpPost]
        public async Task<IActionResult> Edit(NoteEditViewModel model)
        {
            Guid currentUserId = GetCurrentUserId();

            if (ModelState.IsValid)
            {
                await noteService.EditNoteAsync(model, currentUserId);

                return RedirectToFolderIndexView(model.NotebookId, model.ParentFolderId, model.FolderId);
            }

            model.Tags = await noteService.GetAllTagsAsync(currentUserId);

            return View(model);
        }

        // Helper method to redirect to the appropriate folder index view based on the notebook and folder IDs
        private IActionResult RedirectToFolderIndexView(Guid notebookId, Guid? parentFolderId = null, Guid? folderId = null)
        {
            if (folderId.HasValue && folderId != Guid.Empty &&
                parentFolderId.HasValue && parentFolderId != Guid.Empty)
            {
                return Redirect($"~/Folder?notebookId={notebookId}&parentFolderId={parentFolderId}&folderId={folderId}");
            }
            else if (folderId.HasValue && folderId != Guid.Empty)
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