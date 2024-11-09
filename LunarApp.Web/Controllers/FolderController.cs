using LunarApp.Web.Data;
using LunarApp.Web.Data.Models;
using LunarApp.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LunarApp.Web.Controllers
{
    public class FolderController(ApplicationDbContext context) : Controller
    {
        public async Task<IActionResult> Index(Guid notebookId, Guid? parentFolderId)
        {
            IEnumerable<FolderInfoViewModel> folders;

            if (parentFolderId.HasValue && parentFolderId.Value != Guid.Empty)
            {
                folders = await context.Folders
                    .Where(f => f.ParentFolderId == parentFolderId.Value)
                    .Select(f => new FolderInfoViewModel
                    {
                        Id = f.Id,
                        Title = f.Title,
                        NotebookId = f.NotebookId,
                        ParentFolderId = f.ParentFolderId
                    })
                    .ToListAsync();

                ViewData["NotebookId"] = notebookId;
                ViewData["ParentFolderId"] = parentFolderId;
                ViewData["Title"] = "Folders in Folder";
            }
            else if (notebookId != Guid.Empty)
            {
                folders = await context.Folders
                    .Where(f => f.NotebookId == notebookId && f.ParentFolderId == null)
                    .Select(f => new FolderInfoViewModel
                    {
                        Id = f.Id,
                        Title = f.Title,
                        NotebookId = f.NotebookId
                    })
                    .ToListAsync();

                ViewData["NotebookId"] = notebookId;
                ViewData["Title"] = "Folders in Notebook";
            }
            else
            {
                return BadRequest("NotebookId or ParentFolderId must be provided.");
            }

            return View(folders);
        }

        [HttpGet]
        public async Task<IActionResult> Create(Guid notebookId, Guid? parentFolderId)
        {
            var model = new FolderViewModel
            {
                NotebookId = notebookId,
                ParentFolderId = parentFolderId
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(FolderViewModel model)
        {
            var notebookExists = await context.Notebooks.AnyAsync(n => n.Id == model.NotebookId);

            if (!notebookExists)
            {
                ModelState.AddModelError(string.Empty, "The specified notebook does not exist.");
                return View(model);
            }

            Folder folder = new Folder
            {
                Title = model.Title,
                NotebookId = model.NotebookId,
                ParentFolderId = model.ParentFolderId
            };

            await context.Folders.AddAsync(folder);
            await context.SaveChangesAsync();

            if (folder.ParentFolderId.HasValue)
            {
                return Redirect($"~/Folder?parentFolderId={folder.ParentFolderId.Value}&notebookId={model.NotebookId}");
            }

            return Redirect($"~/Folder?notebookId={model.NotebookId}");
        }
    }
}