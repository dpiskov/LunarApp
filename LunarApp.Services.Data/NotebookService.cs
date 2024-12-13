using LunarApp.Data.Models;
using LunarApp.Data.Repository.Interfaces;
using LunarApp.Services.Data.Interfaces;
using LunarApp.Web.ViewModels.Notebook;
using Microsoft.EntityFrameworkCore;

namespace LunarApp.Services.Data
{
    /// <summary>
    /// Service for managing notebooks, including CRUD operations and other related actions.
    /// </summary>
    /// <remarks>
    /// The <see cref="NotebookService"/> class provides operations for managing <see cref="Notebook"/> entities, 
    /// including create, read, update, and delete (CRUD) actions. It also interacts with <see cref="Note"/> entities
    /// and leverages the <see cref="IFolderService"/> to perform operations related to folders.
    /// </remarks>
    /// <param name="notebookRepository">The repository for interacting with <see cref="Notebook"/> entities in the data source.</param>
    /// <param name="noteRepository">The repository for interacting with <see cref="Note"/> entities in the data source.</param>
    /// <param name="folderService">The service for managing folders, which assists with folder-related operations.</param>
    public class NotebookService(IRepository<Notebook, Guid> notebookRepository, IRepository<Note, Guid> noteRepository, IFolderService folderService) : INotebookService
    {
        /// <summary>
        /// Retrieves a notebook by its title and user ID.
        /// </summary>
        /// <param name="title">The title of the notebook.</param>
        /// <param name="userId">The ID of the user who owns the notebook.</param>
        /// <returns>A task representing the asynchronous operation, with the notebook if found, or null if not.</returns>

        public async Task<Notebook?> GetByTitleAsync(string title, Guid userId)
        {
            return await notebookRepository.FirstOrDefaultAsync(n => n.Title == title && n.UserId == userId);
        }

        /// <summary>
        /// Retrieves all notebooks for a specific user, ordered by title.
        /// </summary>
        /// <param name="userId">The ID of the user whose notebooks are to be retrieved.</param>
        /// <returns>A task representing the asynchronous operation, with a collection of notebooks.</returns>
        public async Task<IEnumerable<NotebookInfoViewModel>> IndexGetAllOrderedByTitleAsync(Guid userId)
        {
            IEnumerable<NotebookInfoViewModel> notebooks = await notebookRepository
                .GetAllAttached()
                .Where(n => n.UserId == userId)
                .Select(nb => new NotebookInfoViewModel()
                {
                    Id = nb.Id,
                    Title = nb.Title
                })
                .OrderBy(nb => nb.Title)
                .AsNoTracking()
                .ToListAsync();

            return notebooks;
        }

        /// <summary>
        /// Adds a new notebook for a user.
        /// </summary>
        /// <param name="model">The model containing the details of the new notebook.</param>
        /// <param name="userId">The ID of the user who owns the notebook.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task AddNotebookAsync(NotebookCreateViewModel model, Guid userId)
        {
            Notebook notebook = new Notebook()
            {
                Title = model.Title,
                UserId = userId
            };

            await notebookRepository.AddAsync(notebook);
        }

        /// <summary>
        /// Retrieves a notebook for deletion by its ID and user ID.
        /// </summary>
        /// <param name="notebookId">The ID of the notebook to be deleted.</param>
        /// <param name="userId">The ID of the user who owns the notebook.</param>
        /// <returns>A task representing the asynchronous operation, with a view model containing the notebook's details, or null if not found.</returns>
        public async Task<NotebookDeleteViewModel?> GetNotebookForDeleteByIdAsync(Guid notebookId, Guid userId)
        {
            NotebookDeleteViewModel? model = await notebookRepository
                .GetAllAttached()
                .Where(nb => nb.Id == notebookId && nb.UserId == userId)
                .AsNoTracking()
                .Select(nb => new NotebookDeleteViewModel()
                {
                    Id = nb.Id,
                    Title = nb.Title
                })
                .FirstOrDefaultAsync();

            return model;
        }

        /// <summary>
        /// Deletes a notebook and its associated folders and notes.
        /// </summary>
        /// <param name="notebookId">The ID of the notebook to be deleted.</param>
        /// <param name="userId">The ID of the user who owns the notebook.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task DeleteNotebookAsync(Guid notebookId, Guid userId)
        {
            Notebook? notebook = await notebookRepository
                .GetAllAttached()
                .Where(n => n.Id == notebookId && n.UserId == userId)
                .Include(n => n.Notes)
                .Include(n => n.Folders)
                .ThenInclude(f => f.Notes)
                .FirstOrDefaultAsync(n => n.Id == notebookId);

            if (notebook == null)
            {
                throw new InvalidOperationException("Notebook not found.");
                //return;
            }

            List<Folder> foldersToDelete = notebook.Folders.ToList();

            // Delete folders and notes recursively using FolderService
            foreach (Folder folder in foldersToDelete)
            {
                await folderService.DeleteFolderWithChildrenAsync(folder.Id, userId);
            }

            await noteRepository.DeleteRangeAsync(notebook.Notes);
            await notebookRepository.DeleteAsync(notebookId);

            await notebookRepository.SaveChangesAsync();
        }

        /// <summary>
        /// Retrieves a notebook's details for editing by its ID and user ID.
        /// </summary>
        /// <param name="notebookId">The ID of the notebook to be edited.</param>
        /// <param name="userId">The ID of the user who owns the notebook.</param>
        /// <returns>A task representing the asynchronous operation, with a view model containing the notebook's details for editing, or null if not found.</returns>
        public async Task<NotebookEditViewModel?> GetNotebookForEditByIdAsync(Guid notebookId, Guid userId)
        {
            NotebookEditViewModel? model = await notebookRepository
                .GetAllAttached()
                .Where(nb => nb.Id == notebookId && nb.UserId == userId)
                .Select(nb => new NotebookEditViewModel
                {
                    Id = nb.Id,
                    Title = nb.Title
                })
                .AsNoTracking()
                .FirstOrDefaultAsync();

            return model;
        }

        /// <summary>
        /// Edits a notebook's title.
        /// </summary>
        /// <param name="model">The model containing the updated notebook details.</param>
        /// <param name="userId">The ID of the user who owns the notebook.</param>
        /// <returns>A task representing the asynchronous operation, with a result indicating whether the update was successful.</returns>
        public async Task<bool> EditNotebookAsync(NotebookEditViewModel? model, Guid userId)
        {
            if (model == null || string.IsNullOrWhiteSpace(model.Title))
            {
                return false;
            }

            Notebook? notebook = await notebookRepository
                .FirstOrDefaultAsync(nb => nb.Id == model.Id && nb.UserId == userId);

            if (notebook == null)
            {
                return false;
            }

            notebook.Title = model.Title;

            return await notebookRepository.UpdateAsync(notebook);
        }

        /// <summary>
        /// Retrieves a notebook's details by its ID for viewing.
        /// </summary>
        /// <param name="notebookId">The ID of the notebook to retrieve details for.</param>
        /// <param name="userId">The ID of the user who owns the notebook.</param>
        /// <returns>A task representing the asynchronous operation, with a view model containing the notebook's details, or null if not found.</returns>
        public async Task<NotebookDetailsViewModel?> GetNotebookDetailsByIdAsync(Guid notebookId, Guid userId)
        {
            NotebookDetailsViewModel? model = await notebookRepository
                .GetAllAttached()
                .Where(nb => nb.Id == notebookId && nb.UserId == userId)
                .Select(nb => new NotebookDetailsViewModel
                {
                    Id = nb.Id,
                    Title = nb.Title,
                    Description = nb.Description
                })
                .AsNoTracking()
                .FirstOrDefaultAsync();

            return model;
        }

        /// <summary>
        /// Edits a notebook's description.
        /// </summary>
        /// <param name="model">The model containing the updated description for the notebook.</param>
        /// <param name="userId">The ID of the user who owns the notebook.</param>
        /// <returns>A task representing the asynchronous operation, with a result indicating whether the update was successful.</returns>
        public async Task<bool> EditDetailsNotebookAsync(NotebookDetailsViewModel? model, Guid userId)
        {
            if (model == null || string.IsNullOrWhiteSpace(model.Title))
            {
                return false;
            }

            Notebook? notebook = await notebookRepository
                .FirstOrDefaultAsync(nb => nb.Id == model.Id && nb.UserId == userId);

            if (notebook == null)
            {
                return false;
            }

            notebook.Description = model.Description;

            return await notebookRepository.UpdateAsync(notebook);
        }
    }
}
