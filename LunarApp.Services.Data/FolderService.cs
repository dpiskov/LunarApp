﻿using LunarApp.Data.Models;
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
        public async Task<FolderNotesViewModel> IndexGetAllFoldersAsync(Guid notebookId, Guid? parentFolderId,
    Guid? folderId)
        {
            IEnumerable<FolderInfoViewModel> folders = new List<FolderInfoViewModel>();
            IEnumerable<NoteInfoViewModel> notes = new List<NoteInfoViewModel>();

            if (folderId != Guid.Empty && folderId != null && notebookId != Guid.Empty && notebookId != null)
            {
                folders = await folderRepository
                    .GetAllAttached()
                    .Where(f => f.ParentFolderId == folderId)
                    .Select(f => new FolderInfoViewModel
                    {
                        Id = f.Id,
                        Title = f.Title,
                        NotebookId = f.NotebookId,
                        ParentFolderId = f.ParentFolderId
                    })
                    .OrderBy(f => f.Title)
                    .ToListAsync();

                notes = await noteRepository
                    .GetAllAttached()
                    .Where(n => n.FolderId == folderId)
                    .Select(n => new NoteInfoViewModel
                    {
                        Id = n.Id,
                        Title = n.Title,
                        NotebookId = n.NotebookId,
                        FolderId = n.FolderId
                    })
                    .OrderBy(n => n.Title)
                    .ToListAsync();
            }
            else if (notebookId != Guid.Empty && notebookId != null)
            {
                folders = await folderRepository
                    .GetAllAttached()
                    .Where(f => f.NotebookId == notebookId &&
                                f.ParentFolderId == null)
                    .Select(f => new FolderInfoViewModel
                    {
                        Id = f.Id,
                        Title = f.Title,
                        NotebookId = f.NotebookId,
                        ParentFolderId = parentFolderId
                    })
                    .OrderBy(f => f.Title)
                    .ToListAsync();

                notes = await noteRepository
                    .GetAllAttached()
                    .Where(n => n.FolderId == null)
                    .Where(nb => nb.NotebookId == notebookId)
                    .Select(n => new NoteInfoViewModel
                    {
                        Id = n.Id,
                        Title = n.Title,
                        NotebookId = n.NotebookId,
                        FolderId = n.FolderId
                    })
                    .OrderBy(n => n.Title)
                    .ToListAsync();
            }

            return new FolderNotesViewModel
            {
                Folders = folders,
                Notes = notes
            };
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