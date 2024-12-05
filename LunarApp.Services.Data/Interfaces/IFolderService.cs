using LunarApp.Data.Models;
using LunarApp.Web.ViewModels.Folder;

namespace LunarApp.Services.Data.Interfaces
{
    public interface IFolderService
    {
        Task<FolderNotesViewModel> IndexGetAllFoldersAsync(Guid notebookId, Guid? parentFolderId, Guid? folderId);
        Task<FolderCreateViewModel> GetAddFolderModelAsync(Guid notebookId, Guid? parentFolderId, Guid? folderId);
        Task<(bool isSuccess, string? errorMessage, Folder? folder)> AddFolderAsync(FolderCreateViewModel model);
        Task<(FolderCreateViewModel model, Guid newParentFolderId)> GetAddSubfolderModelAsync(Guid notebookId, Guid? parentFolderId, Guid? folderId);
        Task<(FolderDeleteViewModel? model, Guid newParentFolderId)> GetFolderForDeleteByIdAsync(Guid notebookId, Guid? parentFolderId, Guid folderId);
        Task DeleteFolderWithChildrenAsync(Guid folderId);
        Task<(FolderEditViewModel model, Guid newParentFolderId)> GetFolderForEditByIdAsync(Guid notebookId, Guid? parentFolderId, Guid folderId);
        Task<(bool isEdited, Folder? parentFolder)> EditFolderAsync(FolderEditViewModel model);
        Task<(FolderDetailsViewModel? model, Guid newParentFolderId)> GetFolderDetailsByIdAsync(Guid notebookId, Guid? parentFolderId, Guid folderId);
        Task<(bool isEdited, Folder? parentFolder)> EditDetailsFolderAsync(FolderDetailsViewModel? model);


        Task<string?> GetFolderTitleAsync(Guid folderId);
        Task<string?> GetNotebookTitleAsync(Guid notebookId);


        Task<Guid> GetParentFolderIdAsync(Guid? parentFolderId, Guid newParentFolderId);
        Task<(Guid newParentFolderId, Guid newFolderId)> GetFolderAndParentIdsAsync(Guid? parentFolderId, Guid newParentFolderId, Guid newFolderId);
        Task<Folder?> GetFolderForRedirectionAsync(Guid folderId, Guid? parentFolderId);
    }
}
