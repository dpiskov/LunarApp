using LunarApp.Web.ViewModels.Folder;

namespace LunarApp.Services.Data.Interfaces
{
    public interface IBaseService
    {
        Task<FolderNotesViewModel> GetFilteredNotesAsyncByNotebookId(Guid notebookId, Guid? parentFolderId, Guid? folderId, Guid userId, string? searchQuery, string? tagFilter);
        Task<FolderNotesViewModel> GetFilteredNotesAsync(Guid userId, string? searchQuery, string? tagFilter);
        Task<IEnumerable<string>> GetAllTagsAsync(Guid userId);
    }
}
