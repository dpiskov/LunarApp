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
    }
}
