using LunarApp.Data.Models;
using LunarApp.Services.Data.Interfaces;
using LunarApp.Web.ViewModels.Note;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace LunarApp.Web.Controllers
{
    /// <summary>
    /// Controller for managing notes within a notebook and folder. Provides actions to create, edit, delete, and view notes.
    /// </summary>
    /// <remarks>
    /// The controller ensures that only authorized users can perform actions, and all actions are tied to specific notebooks and folders.
    /// It interacts with the <see cref="INoteService"/> for note-related operations such as creating, updating, and deleting notes.
    /// </remarks>
    /// <param name="noteService">The service responsible for handling note operations such as creation, editing, and deletion.</param>
    /// <param name="userManager">The user manager used to manage user-related operations and authentication.</param>
    [Authorize]
    public class NoteController(
        INoteService noteService,
        UserManager<ApplicationUser> userManager
        ) : BaseController(userManager)
    {

        /// <summary>
        /// Renders the form for creating a new note within a specific notebook and folder.
        /// </summary>
        /// <param name="notebookId">The ID of the notebook the note belongs to.</param>
        /// <param name="parentFolderId">The ID of the parent folder containing the note.</param>
        /// <param name="folderId">The ID of the folder containing the note.</param>
        /// <returns>A view for creating a new note.</returns>
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

        /// <summary>
        /// Handles form submission for creating a new note.
        /// </summary>
        /// <param name="model">The model containing the note's data.</param>
        /// <returns>A redirect to the folder index view after the note is created.</returns>
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

        /// <summary>
        /// Renders the confirmation page for deleting a note.
        /// </summary>
        /// <param name="notebookId">The ID of the notebook containing the note to delete.</param>
        /// <param name="parentFolderId">The ID of the parent folder containing the note to delete.</param>
        /// <param name="folderId">The ID of the folder containing the note to delete.</param>
        /// <param name="noteId">The ID of the note to delete.</param>
        /// <returns>A confirmation view for deleting a note.</returns>
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

        /// <summary>
        /// Handles form submission for deleting a note from the database.
        /// </summary>
        /// <param name="model">The model containing the note's data.</param>
        /// <returns>A redirect to the folder index view after the note is deleted.</returns>
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

        /// <summary>
        /// Renders the form for editing an existing note.
        /// </summary>
        /// <param name="notebookId">The ID of the notebook containing the note to edit.</param>
        /// <param name="parentFolderId">The ID of the parent folder containing the note to edit.</param>
        /// <param name="folderId">The ID of the folder containing the note to edit.</param>
        /// <param name="noteId">The ID of the note to edit.</param>
        /// <returns>A view for editing the specified note.</returns>
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

        /// <summary>
        /// Handles form submission for updating a note's information.
        /// </summary>
        /// <param name="model">The model containing the updated note's data.</param>
        /// <returns>A redirect to the folder index view after the note is updated.</returns>
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

        /// <summary>
        /// Helper method to redirect to the appropriate folder index view based on the notebook and folder IDs.
        /// </summary>
        /// <param name="notebookId">The ID of the notebook to redirect to.</param>
        /// <param name="parentFolderId">The ID of the parent folder to redirect to (optional).</param>
        /// <param name="folderId">The ID of the folder to redirect to (optional).</param>
        /// <returns>A redirect to the folder index view.</returns>
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