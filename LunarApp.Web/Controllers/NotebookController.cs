using LunarApp.Web.Data;
using LunarApp.Web.Data.Models;
using LunarApp.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LunarApp.Web.Controllers
{
    public class NotebookController(ApplicationDbContext context) : Controller
    {
        // GET method to display the list of notebooks
        public async Task<IActionResult> Index()
        {
            // Fetches all notebooks from the database and selects only necessary fields for the view model
            var model = await context.Notebooks
                .Select(nb => new NotebookInfoViewModel()       // Creates a simplified view model for the notebooks
                {
                    Id = nb.Id,
                    Title = nb.Title
                })
                .AsNoTracking()                                        // Disables tracking of entities for better performance (read-only)
                .ToListAsync();                                        // Asynchronously gets the list of notebooks

            // Returns the view with the model containing the notebooks data
            return View(model);
        }

        // GET method to render the form for creating a new notebook
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View();                                             // Returns the create notebook form view
        }

        // POST method to handle form submission for creating a new notebook
        [HttpPost]
        public async Task<IActionResult> Create(NotebookViewModel model)
        {
            // Checks if the submitted form data is valid
            if (ModelState.IsValid == false)
            {
                return View(model);                                    // If not valid, returns the form view with validation errors
            }

            // Creates a new Notebook object using the model data
            Notebook notebook = new Notebook()
            {
                Title = model.Title                                    // Sets the notebook's title from the form input
            };

            // Adds the new notebook to the database and saves changes asynchronously
            await context.Notebooks.AddAsync(notebook);
            await context.SaveChangesAsync();

            // Redirects to the Index action to show the updated list of notebooks
            return RedirectToAction(nameof(Index));
        }

        // GET method to render the confirmation page for deleting a notebook
        [HttpGet]
        public async Task<IActionResult> Remove(Guid id)
        {
            // Fetches the notebook to be deleted from the database
            var model = await context.Notebooks
                .Where(nb => nb.Id == id)                       // Filters notebooks by ID
                .AsNoTracking()                                         // Disables tracking for read-only operation
                .Select(nb => new NotebookDeleteViewModel()     // Creates a view model for the notebook to be deleted
                {
                    Id = nb.Id,
                    Title = nb.Title
                })
                .FirstOrDefaultAsync();                                 // Gets the first (or default) notebook matching the ID

            // If the notebook doesn't exist, redirects to the Index view
            if (model == null)
            {
                return RedirectToAction(nameof(Index));
            }

            // Returns the confirmation view with the notebook data
            return View(model);
        }

        // POST method to actually remove a notebook from the database
        [HttpPost]
        public async Task<IActionResult> Remove(NotebookDeleteViewModel model)
        {
            // Fetches the notebook from the database by its ID
            Notebook? notebook = await context.Notebooks
                .Where(nb => nb.Id == model.Id)
                .FirstOrDefaultAsync();

            // If the notebook exists, removes it from the database
            if (notebook != null)
            {
                context.Notebooks.Remove(notebook);                     // Removes the notebook from the context (database)

                // Saves changes to the database asynchronously
                await context.SaveChangesAsync();
            }

            // Redirects to the Index view to show the updated list of notebooks
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var notebook = await context.Notebooks
                .Where(n => n.Id == id)
                .AsNoTracking()
                .FirstOrDefaultAsync();

            if (notebook == null)
            {
                //return NotFound();
                return RedirectToAction(nameof(Index));
            }

            var model = new NotebookViewModel()
            {
                Title = notebook.Title
            };

            return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> Edit(NotebookViewModel model, Guid id)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var notebook = await context.Notebooks.FindAsync(id);

            if (notebook == null)
            {
                //return NotFound();
                return RedirectToAction(nameof(Index));
            }

            notebook.Title = model.Title;

            await context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
        // The Details action is commented out for now, but it's designed to show notebook details
        // The method would include related folders and notes as part of the notebook details
        //[HttpGet]
        //public async Task<IActionResult> Details(Guid id)
        //{
        // Fetches the notebook with related folders and notes
        //    var notebook = await context.Notebooks
        //        .Include(n => n.Folders)                              // Includes related folders
        //        .Include(n => n.Notes)                                // Includes related notes
        //        .FirstOrDefaultAsync(n => n.Id == id);                // Filters by notebook ID

        // Returns NotFound if the notebook doesn't exist
        //    if (notebook == null)
        //    {
        //        return NotFound();
        //    }

        // Passes the notebook's ID to the view
        //    ViewData["NotebookId"] = notebook.Id;

        // Creates a view model to display the notebook's data in the view
        //    var viewModel = new NotebookInfoViewModel
        //    {
        //        Id = notebook.Id,
        //        Title = notebook.Title,
        //        Folders = notebook.Folders,
        //        Notes = notebook.Notes
        //    };

        // Returns the view with the notebook details
        //    return View(viewModel);
        //}
    }
}
