using LunarApp.Data.Models;
using LunarApp.Web.ViewModels.Notebook;

namespace LunarApp.Services.Data.Interfaces
{
    public interface INotebookService
    {
        Task<Notebook?> GetByTitleAsync(string title);

        Task<IEnumerable<NotebookInfoViewModel>> IndexGetAllOrderedByTitleAsync();

        Task AddNotebookAsync(NotebookCreateViewModel model);

        Task<NotebookDeleteViewModel?> GetNotebookForDeleteByIdAsync(Guid notebookId);

        Task DeleteNotebookAsync(Guid notebookId);

        Task<NotebookEditViewModel?> GetNotebookForEditByIdAsync(Guid notebookId);

        Task<bool> EditNotebookAsync(NotebookEditViewModel? model);


        Task<NotebookDetailsViewModel?> GetNotebookDetailsByIdAsync(Guid id);
        Task<bool> EditDetailsNotebookAsync(NotebookDetailsViewModel? model);

    }
}
