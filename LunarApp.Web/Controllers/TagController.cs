using LunarApp.Data;
using LunarApp.Data.Models;
using LunarApp.Services.Data.Interfaces;
using LunarApp.Web.ViewModels.Tag;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LunarApp.Web.Controllers
{
    [Authorize]
    public class TagController(ApplicationDbContext context) : Controller
    public class TagController(ApplicationDbContext context, ITagService tagService) : Controller
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
                Tag tag = new Tag
                {
                    Name = model.Name
                };

                context.Tags.Add(tag);
                await context.SaveChangesAsync();

                if (model.FolderId != Guid.Empty && model.FolderId != null &&
                    model.ParentFolderId != Guid.Empty && model.ParentFolderId != null)
                {
                    return Redirect($"~/Tag?notebookId={model.NotebookId}&parentFolderId={model.ParentFolderId}&folderId={model.FolderId}&noteId={model.NoteId}");
                }
                else if (model.FolderId != Guid.Empty && model.FolderId != null)
                {
                    return Redirect($"~/Tag?notebookId={model.NotebookId}&folderId={model.FolderId}&noteId={model.NoteId}");
                }
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid? notebookId, Guid? parentFolderId, Guid? folderId, Guid? noteId, Guid tagId)
        {
            Tag? tag = await context.Tags
                .Where(t => t.Id == tagId)
                .AsNoTracking()
                .FirstOrDefaultAsync();

            if (tag == null)
            {
                return RedirectToAction(nameof(Index), "Tag");
            }

            TagEditViewModel model = new TagEditViewModel
            {
                Id = tagId,
                Name = tag.Name,
                NotebookId = notebookId,
                ParentFolderId = parentFolderId,
                FolderId = folderId,
                NoteId = noteId
            };

            ViewData["NotebookId"] = notebookId;
            ViewData["ParentFolderId"] = parentFolderId;
            ViewData["FolderId"] = folderId;
            ViewData["NoteId"] = noteId;
            ViewData["TagId"] = tagId;

            ViewData["Title"] = tag.Name;

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(TagEditViewModel model)
        {
            if (ModelState.IsValid)
            {
                var tag = await context.Tags.FindAsync(model.Id);

                if (tag == null)
                {
                    if (model.FolderId != Guid.Empty && model.FolderId != null &&
                        model.ParentFolderId != Guid.Empty && model.ParentFolderId != null)
                    {
                        return Redirect($"~/Tag?notebookId={model.NotebookId}&parentFolderId={model.ParentFolderId}&folderId={model.FolderId}&noteId={model.NoteId}");
                    }
                    else if (model.FolderId != Guid.Empty && model.FolderId != null)
                    {
                        return Redirect($"~/Tag?notebookId={model.NotebookId}&folderId={model.FolderId}&noteId={model.NoteId}");
                    }
                }
                else
                {
                    tag.Name = model.Name;
                }

                await context.SaveChangesAsync();

                if (model.FolderId != Guid.Empty && model.FolderId != null &&
                    model.ParentFolderId != Guid.Empty && model.ParentFolderId != null)
                {
                    return Redirect($"~/Tag?notebookId={model.NotebookId}&parentFolderId={model.ParentFolderId}&folderId={model.FolderId}&noteId={model.NoteId}");
                }
                else if (model.FolderId != Guid.Empty && model.FolderId != null)
                {
                    return Redirect($"~/Tag?notebookId={model.NotebookId}&folderId={model.FolderId}&noteId={model.NoteId}");
                }
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Remove(Guid notebookId, Guid? parentFolderId, Guid? folderId, Guid noteId, Guid tagId)
        {
            TagRemoveViewModel? model = await context.Tags
                .Where(t => t.Id == tagId)
                .AsNoTracking()
                .Select(t => new TagRemoveViewModel()
                {
                    Id = t.Id,
                    Name = t.Name,
                    NotebookId = notebookId,
                    ParentFolderId = parentFolderId,
                    FolderId = folderId,
                    NoteId = noteId
                })
                .FirstOrDefaultAsync();

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
                Tag? tag = await context.Tags
                    .Where(t => t.Id == model.Id)
                    .FirstOrDefaultAsync();

                if (tag != null)
                {
                    context.Tags.Remove(tag);
                    await context.SaveChangesAsync();
                }

                if (model.FolderId != Guid.Empty && model.FolderId != null &&
                    model.ParentFolderId != Guid.Empty && model.ParentFolderId != null)
                {
                    return Redirect($"~/Tag?notebookId={model.NotebookId}&parentFolderId={model.ParentFolderId}&folderId={model.FolderId}&noteId={model.NoteId}");
                }
                else if (model.FolderId != Guid.Empty && model.FolderId != null)
                {
                    return Redirect($"~/Tag?notebookId={model.NotebookId}&folderId={model.FolderId}&noteId={model.NoteId}");
                }
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
