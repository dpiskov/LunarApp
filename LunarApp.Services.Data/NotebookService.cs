using LunarApp.Data.Models;
using LunarApp.Data.Repository.Interfaces;
using LunarApp.Services.Data.Interfaces;
using LunarApp.Web.ViewModels.Notebook;
using Microsoft.EntityFrameworkCore;

namespace LunarApp.Services.Data
{
    public class NotebookService(IRepository<Notebook, Guid> notebookRepository, IRepository<Note, Guid> noteRepository, IFolderService folderService) : INotebookService
    {
        public async Task<Notebook?> GetByTitleAsync(string title, Guid userId)
        {
            return await notebookRepository.FirstOrDefaultAsync(n => n.Title == title && n.UserId == userId);
        }

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

        public async Task AddNotebookAsync(NotebookCreateViewModel model, Guid userId)
        {
            Notebook notebook = new Notebook()
            {
                Title = model.Title,
                UserId = userId
            };

            await notebookRepository.AddAsync(notebook);
        }

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

        public async Task DeleteNotebookAsync(Guid notebookId, Guid userId)
        {
            Notebook? notebook = await notebookRepository
                .GetAllAttached()
                .Where(n => n.Id == notebookId && n.UserId == userId)
                .Include(n => n.Notes)
                .Include(n => n.Folders)
                .ThenInclude(f => f.Notes) // Include notes in folders
                .FirstOrDefaultAsync(n => n.Id == notebookId);

            if (notebook == null)
            {
                throw new InvalidOperationException("Notebook not found.");
                //return;
            }

            var foldersToDelete = notebook.Folders.ToList(); // Create a copy of the list

            // Delete folders and notes recursively using FolderService
            foreach (Folder folder in foldersToDelete)
            {
                await folderService.DeleteFolderWithChildrenAsync(folder.Id, userId);
            }

            await noteRepository.DeleteRangeAsync(notebook.Notes);
            // Now delete the notebook itself
            await notebookRepository.DeleteAsync(notebookId);
            await notebookRepository.SaveChangesAsync();
        }

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

        public async Task<bool> EditNotebookAsync(NotebookEditViewModel? model, Guid userId)
        {
            if (model == null || string.IsNullOrWhiteSpace(model.Title))
            {
                return false;
            }

            //Notebook? notebook = await notebookRepository.GetByIdAsync(model.Id);
            Notebook? notebook = await notebookRepository
                .FirstOrDefaultAsync(nb => nb.Id == model.Id && nb.UserId == userId);

            if (notebook == null)
            {
                return false;
            }

            notebook.Title = model.Title;

            return await notebookRepository.UpdateAsync(notebook);
        }

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

        public async Task<bool> EditDetailsNotebookAsync(NotebookDetailsViewModel? model, Guid userId)
        {
            if (model == null || string.IsNullOrWhiteSpace(model.Title))
            {
                return false;
            }

            //Notebook? notebook = await notebookRepository.GetByIdAsync(model.Id);
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
