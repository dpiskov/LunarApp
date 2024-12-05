using LunarApp.Data;
using LunarApp.Data.Models;
using LunarApp.Web.ViewModels.Tag;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LunarApp.Web.Controllers
{
    [Authorize]
    public class TagController(ApplicationDbContext context) : Controller
    {
        public async Task<IActionResult> Index(Guid? notebookId, Guid? parentFolderId, Guid? folderId, Guid? noteId)
        {
            IEnumerable<TagViewModel> tags = await context.Tags
                .Select(t => new TagViewModel
                {
                    Id = t.Id,
                    Name = t.Name
                })
                .OrderBy(t => t.Name)
                .AsNoTracking()
                .ToListAsync();
            //List<Tag> tags = context.Tags.ToList();

            ViewData["NotebookId"] = notebookId;
            ViewData["ParentFolderId"] = parentFolderId;
            ViewData["FolderId"] = folderId;
            ViewData["NoteId"] = noteId;

            return View(tags);
        }

        [HttpGet]
        public async Task<IActionResult> Create(Guid? notebookId, Guid? parentFolderId, Guid? folderId, Guid? noteId)
        {
            TagCreateViewModel? model = null;

            model = new TagCreateViewModel
            {
                Name = string.Empty,
                NotebookId = notebookId,
                ParentFolderId = parentFolderId,
                FolderId = folderId,
                NoteId = noteId
            };

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
    }
}
