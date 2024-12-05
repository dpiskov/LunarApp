using LunarApp.Data.Models;
using LunarApp.Data.Repository.Interfaces;
using LunarApp.Services.Data.Interfaces;
using LunarApp.Web.ViewModels.Folder;
using LunarApp.Web.ViewModels.Note;
using Microsoft.EntityFrameworkCore;

namespace LunarApp.Services.Data
{
    public class FolderService(
        IRepository<Notebook, Guid> notebookRepository,
        IRepository<Folder, Guid> folderRepository,
        IRepository<Note, Guid> noteRepository,
        IFolderHelperService folderHelperService)
        : IFolderService
    {
        public Task<FolderNotesViewModel> IndexGetAllFoldersAsync(Guid notebookId, Guid? parentFolderId, Guid? folderId)
        {
            throw new NotImplementedException();
        }

        public Task<FolderCreateViewModel> GetAddFolderModelAsync(Guid notebookId, Guid? parentFolderId, Guid? folderId)
        {
            throw new NotImplementedException();
        }

        public Task<(bool isSuccess, string? errorMessage, Folder? folder)> AddFolderAsync(FolderCreateViewModel model)
        {
            throw new NotImplementedException();
        }

        public Task<(FolderCreateViewModel model, Guid newParentFolderId)> GetAddSubfolderModelAsync(Guid notebookId, Guid? parentFolderId, Guid? folderId)
        {
            throw new NotImplementedException();
        }

        public Task<(FolderDeleteViewModel? model, Guid newParentFolderId)> GetFolderForDeleteByIdAsync(Guid notebookId, Guid? parentFolderId, Guid folderId)
        {
            throw new NotImplementedException();
        }

        public Task DeleteFolderWithChildrenAsync(Guid folderId)
        {
            throw new NotImplementedException();
        }

        public Task<(FolderEditViewModel model, Guid newParentFolderId)> GetFolderForEditByIdAsync(Guid notebookId, Guid? parentFolderId, Guid folderId)
        {
            throw new NotImplementedException();
        }

        public Task<(bool isEdited, Folder? parentFolder)> EditFolderAsync(FolderEditViewModel model)
        {
            throw new NotImplementedException();
        }

        public Task<(FolderDetailsViewModel? model, Guid newParentFolderId)> GetFolderDetailsByIdAsync(Guid notebookId, Guid? parentFolderId, Guid folderId)
        {
            throw new NotImplementedException();
        }

        public Task<(bool isEdited, Folder? parentFolder)> EditDetailsFolderAsync(FolderDetailsViewModel? model)
        {
            throw new NotImplementedException();
        }

        public Task<string?> GetFolderTitleAsync(Guid folderId)
        {
            throw new NotImplementedException();
        }

        public Task<string?> GetNotebookTitleAsync(Guid notebookId)
        {
            throw new NotImplementedException();
        }

        public Task<Guid> GetParentFolderIdAsync(Guid? parentFolderId, Guid newParentFolderId)
        {
            throw new NotImplementedException();
        }

        public Task<(Guid newParentFolderId, Guid newFolderId)> GetFolderAndParentIdsAsync(Guid? parentFolderId, Guid newParentFolderId, Guid newFolderId)
        {
            throw new NotImplementedException();
        }

        public Task<Folder?> GetFolderForRedirectionAsync(Guid folderId, Guid? parentFolderId)
        {
            throw new NotImplementedException();
        }
    }
}