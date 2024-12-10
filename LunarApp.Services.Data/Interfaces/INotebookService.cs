using LunarApp.Data.Models;
using LunarApp.Web.ViewModels.Notebook;

namespace LunarApp.Services.Data.Interfaces
{
    public interface INotebookService
    {
        Task<Notebook?> GetByTitleAsync(string title, Guid userId);

        Task<IEnumerable<NotebookInfoViewModel>> IndexGetAllOrderedByTitleAsync(Guid userId);

        Task AddNotebookAsync(NotebookCreateViewModel model, Guid userId);

        Task<NotebookDeleteViewModel?> GetNotebookForDeleteByIdAsync(Guid notebookId, Guid userId);

        Task DeleteNotebookAsync(Guid notebookId , Guid userId);

        Task<NotebookEditViewModel?> GetNotebookForEditByIdAsync(Guid notebookId , Guid userId);

        Task<bool> EditNotebookAsync(NotebookEditViewModel? model, Guid userId);


        Task<NotebookDetailsViewModel?> GetNotebookDetailsByIdAsync(Guid id, Guid userId);
        Task<bool> EditDetailsNotebookAsync(NotebookDetailsViewModel? model, Guid userId);

    }
}
