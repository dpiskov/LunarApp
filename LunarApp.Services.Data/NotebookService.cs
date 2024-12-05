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

        public Task AddNotebookAsync(NotebookCreateViewModel model)
        {
            throw new NotImplementedException();
        }

        public Task<NotebookDeleteViewModel?> GetNotebookForDeleteByIdAsync(Guid notebookId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteNotebookAsync(Guid notebookId)
        {
            throw new NotImplementedException();
        }

        public Task<NotebookEditViewModel?> GetNotebookForEditByIdAsync(Guid notebookId)
        {
            throw new NotImplementedException();
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
