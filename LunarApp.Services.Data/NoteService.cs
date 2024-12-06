﻿using LunarApp.Data.Models;
using LunarApp.Data.Repository.Interfaces;
using LunarApp.Services.Data.Interfaces;
using LunarApp.Web.ViewModels.Note;
using LunarApp.Web.ViewModels.Tag;
using Microsoft.EntityFrameworkCore;

namespace LunarApp.Services.Data
{
    public class NoteService(
        IRepository<Notebook, Guid> notebookRepository,
        IRepository<Folder, Guid> folderRepository,
        IRepository<Note, Guid> noteRepository,
        IRepository<Tag, Guid> tagRepository)
        : INoteService
    {
        public async Task<NoteCreateViewModel> GetCreateNoteAsync(Guid notebookId, Guid? parentFolderId, Guid? folderId)
        {
            List<TagViewModel> tags = await GetAllTagsAsync();

            NoteCreateViewModel model = new NoteCreateViewModel
            {
                Title = string.Empty,
                NotebookId = notebookId,
                Tags = tags
            };

            if (folderId != Guid.Empty && folderId != null)
            {
                model.FolderId = folderId;

                if (parentFolderId != Guid.Empty && parentFolderId != null)
                {
                    model.ParentFolderId = parentFolderId;
                }
            }
            else if (parentFolderId != Guid.Empty && parentFolderId != null)
            {
                model.FolderId = parentFolderId;
            }

            return model;
        }

        public Task<(bool isSuccess, string? errorMessage)> CreateNoteAsync(NoteCreateViewModel model)
        {
            throw new NotImplementedException();
        }

        public Task<NoteDeleteViewModel?> GetNoteForDeleteByIdAsync(Guid notebookId, Guid? parentFolderId, Guid? folderId, Guid noteId)
        {
            throw new NotImplementedException();
        }

        public Task DeleteNoteAsync(Guid noteId)
        {
            throw new NotImplementedException();
        }

        public Task<NoteEditViewModel?> GetNoteForEditByIdAsync(Guid notebookId, Guid? parentFolderId, Guid? folderId, Guid noteId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> EditNoteAsync(NoteEditViewModel model)
        {
            throw new NotImplementedException();
        }

        public async Task<List<TagViewModel>> GetAllTagsAsync()
        {
            return await tagRepository
                .GetAllAttached()
                .AsNoTracking()
                .Select(t => new TagViewModel
                {
                    Id = t.Id,
                    Name = t.Name
                })
                .ToListAsync();
        }
    }
}