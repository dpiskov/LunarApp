using LunarApp.Web.Data;
using LunarApp.Web.Data.Models;
using LunarApp.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LunarApp.Web.Controllers
{
    public class NotebookController(ApplicationDbContext context) : Controller
    {
        public async Task<IActionResult> Index()
        {
            var model = await context.Notebooks
                .Select(nb => new NotebookInfoViewModel()
                {
                    Id = nb.Id,
                    Title = nb.Title
                })
                .AsNoTracking()
                .ToListAsync();

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View();
        }
    }
}
