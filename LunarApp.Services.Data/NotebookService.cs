using LunarApp.Data.Models;
using LunarApp.Data.Repository.Interfaces;
using LunarApp.Services.Data.Interfaces;
using LunarApp.Web.ViewModels.Notebook;
using Microsoft.EntityFrameworkCore;

namespace LunarApp.Services.Data
{
    public class NotebookService(IRepository<Notebook, Guid> notebookRepository) : INotebookService
    {
        public async Task<IEnumerable<NotebookInfoViewModel>> IndexGetAllOrderedByTitleAsync()
        {
            IEnumerable<NotebookInfoViewModel> notebooks = await notebookRepository
                .GetAllAttached()
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

        public async Task AddNotebookAsync(NotebookCreateViewModel model)
        {
            Notebook notebook = new Notebook()
            {
                Title = model.Title
            };

            await notebookRepository.AddAsync(notebook);
        }

        public async Task<NotebookDeleteViewModel?> GetNotebookForDeleteByIdAsync(Guid notebookId)
        {
            NotebookDeleteViewModel? model = await notebookRepository
                .GetAllAttached()
                .Where(nb => nb.Id == notebookId)
                .AsNoTracking()
                .Select(nb => new NotebookDeleteViewModel()
                {
                    Id = nb.Id,
                    Title = nb.Title
                })
                .FirstOrDefaultAsync();

            return model;
        }

        public async Task<bool> DeleteNotebookAsync(Guid notebookId)
        {
            Notebook? notebook = await notebookRepository
                .FirstOrDefaultAsync(nb => nb.Id == notebookId);

            if (notebook is null)
            {
                return false;
            }

            return await notebookRepository.DeleteAsync(notebookId);
        }

        public async Task<NotebookEditViewModel?> GetNotebookForEditByIdAsync(Guid notebookId)
        {
            NotebookEditViewModel? model = await notebookRepository
                .GetAllAttached()
                .Where(nb => nb.Id == notebookId)
                .Select(nb => new NotebookEditViewModel
                {
                    Id = nb.Id,
                    Title = nb.Title
                })
                .AsNoTracking()
                .FirstOrDefaultAsync();

            return model;
        }

        public Task<bool> EditNotebookAsync(NotebookEditViewModel? model)
        {
            throw new NotImplementedException();
        }

        public Task<NotebookDetailsViewModel?> GetNotebookDetailsByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> EditDetailsNotebookAsync(NotebookDetailsViewModel? model)
        {
            throw new NotImplementedException();
        }
    }
}
