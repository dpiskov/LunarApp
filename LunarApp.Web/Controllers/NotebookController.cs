using LunarApp.Data;
using LunarApp.Data.Models;
using LunarApp.Web.ViewModels.Notebook;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LunarApp.Web.Controllers
{
    [Authorize]
    public class NotebookController(INotebookService notebookService)
        : Controller
    {
        // GET method to display the list of notebooks
        public async Task<IActionResult> Index()
        {
            IEnumerable<NotebookInfoViewModel> notebooks = await notebookService.IndexGetAllOrderedByTitleAsync();

            // Returns the view with the model containing the notebooks data
            return View(notebooks);
        }

        // GET method to render the form for creating a new notebook
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View();                                             // Returns the create notebook form view
        }

        // POST method to handle form submission for creating a new notebook
        [HttpPost]
        public async Task<IActionResult> Create(NotebookCreateViewModel model)
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
        public async Task<IActionResult> Remove(Guid notebookId)
        {
            // Fetches the notebook to be deleted from the database
            var model = await context.Notebooks
                .Where(nb => nb.Id == notebookId)                       // Filters notebooks by ID
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

        // GET method to render the form for editing an existing notebook
        [HttpGet]
        public async Task<IActionResult> Edit(Guid notebookId)
        {
            // Fetches the notebook to be edited from the database by its ID
            var notebook = await context.Notebooks
                .Where(n => n.Id == notebookId)                 // Filters notebooks by ID
                .AsNoTracking()                                 // Disables tracking for read-only operation
                .FirstOrDefaultAsync();                         // Gets the first (or default) notebook matching the ID

            // If the notebook doesn't exist, redirects to the Index view
            if (notebook == null)
            {
                //return NotFound();                            // Optional: Uncomment to return a 404 if the notebook is not found
                return RedirectToAction(nameof(Index));         // Redirects to the main notebook list
            }

            // Prepares a view model with the notebook's data for editing
            var model = new NotebookEditViewModel
            {
                Id = notebookId,
                Title = notebook.Title                          // Sets the current title of the notebook in the view model
            };

            // Returns the edit view with the notebook data in the view model
            return View(model);
        }

        // POST method to handle form submission for updating a notebook's information
        [HttpPost]
        public async Task<IActionResult> Edit(NotebookEditViewModel model)
        {
            // Checks if the submitted form data is valid
            if (!ModelState.IsValid)
            {
                return View(model);                             // If not valid, returns the form view with validation errors
            }

            // Fetches the notebook to be updated from the database by its ID
            var notebook = await context.Notebooks.FindAsync(model.Id);

            // If the notebook doesn't exist, redirects to the Index view
            if (notebook == null)
            {
                // return NotFound();                           // Optional: Uncomment to return a 404 if the notebook is not found
                return RedirectToAction(nameof(Index));         // Redirects to the main notebook list
            }

            // Updates the notebook's title with the new value from the form
            notebook.Title = model.Title;

            // Saves the changes to the database asynchronously
            await context.SaveChangesAsync();

            // Redirects to the Index view to show the updated list of notebooks
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Details(Guid notebookId)
        {
            var notebook = await context.Notebooks
                .Where(n => n.Id == notebookId)
                .AsNoTracking()
                .FirstOrDefaultAsync();

            if (notebook is null)
            {
                return RedirectToAction(nameof(Index));
            }

            var model = new NotebookDetailsViewModel
            {
                Id = notebookId,
                Description = notebook.Description
            };

            ViewData["Title"] = notebook.Title;

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Details(NotebookDetailsViewModel model)
        {
            if (ModelState.IsValid is false)
            {
                return View(model);
            }

            var notebook = await context.Notebooks.FindAsync(model.Id);

            if (notebook is null)
            {
                return RedirectToAction(nameof(Index));
            }

            notebook.Description = model.Description;

            await context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}