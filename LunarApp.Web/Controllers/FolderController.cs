using LunarApp.Web.Data;
using LunarApp.Web.Data.Models;
using LunarApp.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LunarApp.Web.Controllers
{
    public class FolderController(ApplicationDbContext context) : Controller
    {
        public async Task<IActionResult> Index(Guid notebookId)
        {
            var folders = await context.Folders
                .Where(f => f.NotebookId == notebookId)
                .Select(f => new FolderInfoViewModel
                {
                    Id = f.Id,
                    Title = f.Title,
                    NotebookId = f.NotebookId
                })
                .AsNoTracking()
                .ToListAsync();

            ViewData["NotebookId"] = notebookId;

            return View(folders);
        }

        [HttpGet]
        public async Task<IActionResult> Create(Guid notebookId)
        {
            var model = new FolderViewModel
            {
                NotebookId = notebookId
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
                Id = Guid.NewGuid(),
                Title = model.Title,
                NotebookId = model.NotebookId
            };

            await context.Folders.AddAsync(folder);
            await context.SaveChangesAsync();

            return RedirectToAction("Index", "Notebook");
        }
    }
}
