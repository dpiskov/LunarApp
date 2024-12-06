using LunarApp.Services.Data.Interfaces;
using LunarApp.Web.ViewModels.Tag;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LunarApp.Web.Controllers
{
    [Authorize]
    public class TagController(ITagService tagService) : Controller
    {
        public async Task<IActionResult> Index(Guid? notebookId, Guid? parentFolderId, Guid? folderId, Guid? noteId)
        {
            IEnumerable<TagViewModel> tags = await tagService.IndexGetAllTagsOrderedByNameAsync();

            ViewData["NotebookId"] = notebookId;
            ViewData["ParentFolderId"] = parentFolderId;
            ViewData["FolderId"] = folderId;
            ViewData["NoteId"] = noteId;

            return View(tags);
        }

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

        [HttpPost]
        public async Task<IActionResult> Create(TagCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                await tagService.CreateTagAsync(model);

                return RedirectToTagIndexView(model.NotebookId, model.ParentFolderId, model.FolderId, model.NoteId);
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid? notebookId, Guid? parentFolderId, Guid? folderId, Guid? noteId, Guid tagId)
        {
            TagEditViewModel? model = await tagService.GetTagForEditByIdAsync(notebookId, parentFolderId, folderId, noteId, tagId);

            if (model == null)
            {
                return RedirectToAction(nameof(Index), "Tag");
            }

            ViewData["NotebookId"] = notebookId;
            ViewData["ParentFolderId"] = parentFolderId;
            ViewData["FolderId"] = folderId;
            ViewData["NoteId"] = noteId;
            ViewData["TagId"] = tagId;

            ViewData["Title"] = model.Name;

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(TagEditViewModel model)
        {
            if (ModelState.IsValid)
            {
                await tagService.EditTagAsync(model);

                return RedirectToTagIndexView(model.NotebookId, model.ParentFolderId, model.FolderId, model.NoteId);
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Remove(Guid notebookId, Guid? parentFolderId, Guid? folderId, Guid noteId, Guid tagId)
        {
            TagRemoveViewModel? model = await tagService.GetTagForDeleteByIdAsync(notebookId, parentFolderId, folderId, noteId, tagId);

            ViewData["NotebookId"] = notebookId;
            ViewData["ParentFolderId"] = parentFolderId;
            ViewData["FolderId"] = folderId;
            ViewData["NoteId"] = noteId;
            ViewData["TagId"] = tagId;

            return View(model);
        }

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
