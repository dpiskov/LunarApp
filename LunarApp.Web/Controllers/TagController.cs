using LunarApp.Data.Models;
using LunarApp.Services.Data.Interfaces;
using LunarApp.Web.ViewModels.Tag;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace LunarApp.Web.Controllers
{
    /// <summary>
    /// Controller for managing tags within a notebook, folder, or note. Provides actions to create, edit, delete, and view tags.
    /// </summary>
    /// <remarks>
    /// This controller ensures that only authorized users can manage tags. It interacts with the <see cref="ITagService"/> for tag-related operations such as creation, editing, and deletion.
    /// </remarks>
    /// <param name="tagService">The service responsible for handling tag operations such as creating, updating, and deleting tags.</param>
    /// <param name="userManager">The user manager used to manage user-related operations and authentication.</param>
    [Authorize]
    public class TagController(ITagService tagService, UserManager<ApplicationUser> userManager) : BaseController(userManager)
    {
        /// <summary>
        /// Retrieves and displays all tags, ordered by name.
        /// </summary>
        /// <param name="notebookId">The ID of the notebook to filter the tags (optional).</param>
        /// <param name="parentFolderId">The ID of the parent folder to filter the tags (optional).</param>
        /// <param name="folderId">The ID of the folder to filter the tags (optional).</param>
        /// <param name="noteId">The ID of the note to filter the tags (optional).</param>
        /// <returns>A view displaying all tags.</returns>
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

        /// <summary>
        /// Renders the form for creating a new tag.
        /// </summary>
        /// <param name="notebookId">The ID of the notebook the tag is associated with (optional).</param>
        /// <param name="parentFolderId">The ID of the parent folder the tag is associated with (optional).</param>
        /// <param name="folderId">The ID of the folder the tag is associated with (optional).</param>
        /// <param name="noteId">The ID of the note the tag is associated with (optional).</param>
        /// <returns>A view for creating a new tag.</returns>
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

        /// <summary>
        /// Handles form submission for creating a new tag.
        /// </summary>
        /// <param name="model">The model containing the tag's data.</param>
        /// <returns>A redirect to the tag index view after the tag is created.</returns>
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

        /// <summary>
        /// Renders the form for editing an existing tag.
        /// </summary>
        /// <param name="notebookId">The ID of the notebook containing the tag (optional).</param>
        /// <param name="parentFolderId">The ID of the parent folder containing the tag (optional).</param>
        /// <param name="folderId">The ID of the folder containing the tag (optional).</param>
        /// <param name="noteId">The ID of the note containing the tag (optional).</param>
        /// <param name="tagId">The ID of the tag to edit.</param>
        /// <returns>A view for editing the specified tag.</returns>
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

        /// <summary>
        /// Handles form submission for updating an existing tag's details.
        /// </summary>
        /// <param name="model">The model containing the updated tag's data.</param>
        /// <param name="name">The name of the tag being edited.</param>
        /// <returns>A redirect to the tag index view after the tag is updated.</returns>
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

        /// <summary>
        /// Renders the confirmation page for deleting a tag.
        /// </summary>
        /// <param name="notebookId">The ID of the notebook containing the tag (optional).</param>
        /// <param name="parentFolderId">The ID of the parent folder containing the tag (optional).</param>
        /// <param name="folderId">The ID of the folder containing the tag (optional).</param>
        /// <param name="noteId">The ID of the note containing the tag (optional).</param>
        /// <param name="tagId">The ID of the tag to delete.</param>
        /// <returns>A confirmation view for deleting the specified tag.</returns>
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

        /// <summary>
        /// Handles form submission for deleting a tag from the system.
        /// </summary>
        /// <param name="model">The model containing the tag's data to be deleted.</param>
        /// <returns>A redirect to the tag index view after the tag is deleted.</returns>
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

        /// <summary>
        /// Helper method to redirect to the appropriate tag index view based on the parameters.
        /// </summary>
        /// <param name="notebookId">The ID of the notebook to redirect to (optional).</param>
        /// <param name="parentFolderId">The ID of the parent folder to redirect to (optional).</param>
        /// <param name="folderId">The ID of the folder to redirect to (optional).</param>
        /// <param name="noteId">The ID of the note to redirect to (optional).</param>
        /// <returns>A redirect to the tag index view.</returns>
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
