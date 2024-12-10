using LunarApp.Data.Models;
using LunarApp.Web.ViewModels.Folder;

namespace LunarApp.Services.Data.Interfaces
{
    public interface IFolderService
    {
        Task<Folder?> GetByTitleForEditAsync(string title, Guid? parentFolderId, Guid folderId, Guid userId);

        Task<Folder?> GetByTitleInNotebookAsync(string title, Guid notebookId, Guid userId);
        Task<Folder?> GetByTitleAsync(string title, Guid notebookId, Guid? parentFolderId, Guid? folderId);

        Task<FolderNotesViewModel> IndexGetAllFoldersAsync(Guid notebookId, Guid? parentFolderId, Guid? folderId, Guid userId);
        Task<FolderCreateViewModel> GetAddFolderModelAsync(Guid notebookId, Guid? parentFolderId, Guid? folderId);
        Task<(bool isSuccess, string? errorMessage)> AddFolderAsync(FolderCreateViewModel model, Guid userId);
        Task<(FolderCreateViewModel model, Guid newParentFolderId)> GetAddSubfolderModelAsync(Guid notebookId, Guid? parentFolderId, Guid? folderId, Guid userId);
        Task<(FolderDeleteViewModel? model, Guid newParentFolderId)> GetFolderForDeleteByIdAsync(Guid notebookId, Guid? parentFolderId, Guid folderId, Guid userId);
        Task DeleteFolderWithChildrenAsync(Guid folderId, Guid userId);
        Task<(FolderEditViewModel? model, Guid newParentFolderId)> GetFolderForEditByIdAsync(Guid notebookId, Guid? parentFolderId, Guid folderId, Guid userId);
        Task<(bool isEdited, Folder? parentFolder)> EditFolderAsync(FolderEditViewModel? model);
        Task<(FolderDetailsViewModel? model, Guid newParentFolderId)> GetFolderDetailsByIdAsync(Guid notebookId, Guid? parentFolderId, Guid folderId, Guid userId);
        Task<(bool isEdited, Folder? parentFolder)> EditDetailsFolderAsync(FolderDetailsViewModel? model);


        Task<string?> GetFolderTitleAsync(Guid folderId);
        Task<string?> GetNotebookTitleAsync(Guid notebookId, Guid userId);


        Task<Guid> GetParentFolderIdAsync(Guid? parentFolderId, Guid newParentFolderId, Guid userId);
        Task<(Guid newParentFolderId, Guid newFolderId)> GetFolderAndParentIdsAsync(Guid? parentFolderId,
            Guid newParentFolderId, Guid newFolderId, Guid userId);
        Task<Folder?> GetFolderForRedirectionAsync(Guid folderId, Guid? parentFolderId);
    }
}
