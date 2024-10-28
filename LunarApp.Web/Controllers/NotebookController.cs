using LunarApp.Web.Data;
using LunarApp.Web.Data.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LunarApp.Web.Controllers
{
    public class NotebookController(ApplicationDbContext context) : Controller
    {
        public async Task<IActionResult> Index()
        {
            var model = await context.Notebooks
                .Select(nb => new Notebook()
                {
                    Id = nb.Id,
                    Title = nb.Title
                })
                .AsNoTracking()
                .ToListAsync();

            return View(model);
        }
    }
}
