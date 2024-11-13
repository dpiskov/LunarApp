using Humanizer;
using LunarApp.Web.Data;
using LunarApp.Web.Data.Models;
using LunarApp.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LunarApp.Web.Controllers
{
    public class NoteController(ApplicationDbContext context) : Controller
    {
        public async Task<IActionResult> Index(Guid notebookId, Guid? parentFolderId)
        {
            IEnumerable<NoteViewModel> notes;

            notes = await context.Notes
                .Where(n => n.FolderId == parentFolderId)
                .Select(n => new NoteViewModel
                {
                    Title = n.Title,
                    Body = n.Body,
                    NotebookId = n.NotebookId,
                    FolderId = n.FolderId
                })
                .ToListAsync();

            notes = await context.Notes
                .Where(n => n.NotebookId == notebookId && n.FolderId == null)
                .Select(n => new NoteViewModel
                {
                    Title = n.Title,
                    Body = n.Body,
                    NotebookId = n.NotebookId
                })
                .ToListAsync();

            ViewData["NotebookId"] = notebookId;
            ViewData["Title"] = "Notes in Notebook";

            return RedirectToAction(nameof(Index), "Notebook");
        }

        [HttpGet]
        public async Task<IActionResult> Create(Guid notebookId, Guid? parentFolderId)
        {
            var model = new NoteViewModel
            {
                NotebookId = notebookId,
                FolderId = parentFolderId
            };

            return View(model);
        }
    }
}