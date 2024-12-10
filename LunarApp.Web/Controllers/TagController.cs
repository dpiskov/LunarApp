using LunarApp.Data.Models;
using LunarApp.Services.Data.Interfaces;
using LunarApp.Web.ViewModels.Tag;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace LunarApp.Web.Controllers
{
    [Authorize]
    public class TagController(ITagService tagService, UserManager<ApplicationUser> userManager) : BaseController(userManager)
    {
        // GET method to fetch and display all tags
        public async Task<IActionResult> Index(Guid? notebookId, Guid? parentFolderId, Guid? folderId, Guid? noteId)
        {
            Guid currentUserId = GetCurrentUserId();
            IEnumerable<TagViewModel> tags = await tagService.IndexGetAllTagsOrderedByNameAsync(currentUserId);

            ViewData["NotebookId"] = notebookId;
            ViewData["ParentFolderId"] = parentFolderId;
            ViewData["FolderId"] = folderId;
            ViewData["NoteId"] = noteId;

            return View(tags);
        }

        // GET method to render the form for creating a new tag
        [HttpGet]
        public async Task<IActionResult> Create(Guid? notebookId, Guid? parentFolderId, Guid? folderId, Guid? noteId)
        {
            TagCreateViewModel model = await tagService.GetCreateTagAsync(notebookId, parentFolderId, folderId, noteId);

            ViewData["NotebookId"] = notebookId;
            ViewData["ParentFolderId"] = parentFolderId;
            ViewData["FolderId"] = folderId;
            ViewData["NoteId"] = noteId;

            return View(model);
        }

        // POST method to handle the form submission for creating a new tag
        [HttpPost]
        public async Task<IActionResult> Create(TagCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                Guid currentUserId = GetCurrentUserId();
                Tag? existingNotebook = await tagService.GetByTitleAsync(model.Name, currentUserId);

                if (existingNotebook != null)
                {
                    ModelState.AddModelError("Name", "A tag with this name already exists.");

                    ViewData["NotebookId"] = model.NotebookId;
                    ViewData["ParentFolderId"] = model.ParentFolderId;
                    ViewData["FolderId"] = model.FolderId;
                    ViewData["NoteId"] = model.NoteId;

                    return View(model); 
                }

                await tagService.CreateTagAsync(model, currentUserId);

                return RedirectToTagIndexView(model.NotebookId, model.ParentFolderId, model.FolderId, model.NoteId);
            }

            return View(model);
        }

        // GET method to render the form for editing an existing tag
        [HttpGet]
        public async Task<IActionResult> Edit(Guid? notebookId, Guid? parentFolderId, Guid? folderId, Guid? noteId, Guid tagId)
        {
            Guid currentUserId = GetCurrentUserId();
            TagEditViewModel? model = await tagService.GetTagForEditByIdAsync(notebookId, parentFolderId, folderId, noteId, tagId, currentUserId);

            if (model == null)
            {
                return RedirectToAction(nameof(Index), "Tag");
            }

            ViewData["NotebookId"] = notebookId;
            ViewData["ParentFolderId"] = parentFolderId;
            ViewData["FolderId"] = folderId;
            ViewData["NoteId"] = noteId;
            ViewData["TagId"] = tagId;

            ViewData["Title"] = "Edit Tag";

            return View(model);
        }

        // POST method to handle form submission for updating an existing tag's details
        [HttpPost]
        public async Task<IActionResult> Edit(TagEditViewModel model, string name)
        {
            if (ModelState.IsValid)
            {
                Guid currentUserId = GetCurrentUserId();
                Tag? existingTag = await tagService.GetByTitleAsync(model.Name, currentUserId);

                if (existingTag != null)
                {
                    ModelState.AddModelError("Name", "A tag with this name already exists.");

                    ViewData["NotebookId"] = model.NotebookId;
                    ViewData["ParentFolderId"] = model.ParentFolderId;
                    ViewData["FolderId"] = model.FolderId;
                    ViewData["NoteId"] = model.NoteId;
                    ViewData["TagId"] = model.Id;

                    ViewData["Title"] = "Edit Tag";

                    return View(model); 
                }

                await tagService.EditTagAsync(model);

                return RedirectToTagIndexView(model.NotebookId, model.ParentFolderId, model.FolderId, model.NoteId);
            }

            return View(model);
        }

        // GET method to confirm the deletion of a tag
        [HttpGet]
        public async Task<IActionResult> Remove(Guid notebookId, Guid? parentFolderId, Guid? folderId, Guid? noteId, Guid tagId)
        {
            TagRemoveViewModel? model = await tagService.GetTagForDeleteByIdAsync(notebookId, parentFolderId, folderId, noteId, tagId);

            ViewData["NotebookId"] = notebookId;
            ViewData["ParentFolderId"] = parentFolderId;
            ViewData["FolderId"] = folderId;
            ViewData["NoteId"] = noteId;
            ViewData["TagId"] = tagId;

            return View(model);
        }

        // POST method to delete a tag
        [HttpPost]
        public async Task<IActionResult> Remove(TagRemoveViewModel model)
        {
            if (ModelState.IsValid)
            {
                await tagService.DeleteTagAsync(model.Id);

                return RedirectToTagIndexView(model.NotebookId, model.ParentFolderId, model.FolderId, model.NoteId);
            }

            return View(model);
        }

        // Helper method to redirect to the appropriate tag index view based on the parameters
        private IActionResult RedirectToTagIndexView(Guid? notebookId, Guid? parentFolderId, Guid? folderId, Guid? noteId)
        {
            string redirectUrl = string.Empty;

            if (folderId != Guid.Empty && folderId != null &&
                parentFolderId != Guid.Empty && parentFolderId != null)
            {
                redirectUrl += $"~/Tag?notebookId={notebookId}&parentFolderId={parentFolderId}&folderId={folderId}";
            }
            else if (folderId != Guid.Empty && folderId != null)
            {
                redirectUrl += $"~/Tag?notebookId={notebookId}&folderId={folderId}";
            }
            else if (notebookId != Guid.Empty && notebookId != null)
            {
                redirectUrl += $"~/Tag?notebookId={notebookId}";
            }

            if (noteId != Guid.Empty && noteId != null)
            {
                redirectUrl += $"&noteId={noteId}";
            }

            return Redirect(redirectUrl);
        }
    }
}
