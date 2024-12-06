using LunarApp.Services.Data.Interfaces;
using LunarApp.Web.ViewModels.Note;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LunarApp.Web.Controllers
{
    // TODO'S
    // TODO: Handle exceptions
    [Authorize]
    public class NoteController(INoteService noteService) : Controller
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
            NoteDeleteViewModel? model = await noteService.GetNoteForDeleteByIdAsync(notebookId, parentFolderId, folderId, noteId);

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
                await noteService.DeleteNoteAsync(model.Id);

                return RedirectToFolderIndexView(model.NotebookId, model.ParentFolderId, model.FolderId);
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid notebookId, Guid? parentFolderId, Guid? folderId, Guid noteId)
        {
            NoteEditViewModel? model = await noteService.GetNoteForEditByIdAsync(notebookId, parentFolderId, folderId, noteId);

            if (model == null)
            {
                return RedirectToAction(nameof(Index), "Notebook");
            }

            //TODO: Format LastSaved properly
            ViewData["DateCreated"] = model.DateCreated;
            ViewData["LastSaved"] = model.LastSaved;

            ViewData["NotebookId"] = notebookId;
            ViewData["ParentFolderId"] = parentFolderId;
            ViewData["FolderId"] = folderId;
            ViewData["NoteId"] = noteId;

            ViewData["Title"] = model.Title;

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(NoteEditViewModel model)
        {
            if (ModelState.IsValid)
            {
                await noteService.EditNoteAsync(model);

                return RedirectToFolderIndexView(model.NotebookId, model.ParentFolderId, model.FolderId);
            }

            model.Tags = await noteService.GetAllTagsAsync();

            return View(model);
        }

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