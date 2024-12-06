using LunarApp.Data.Models;
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

        public async Task<(bool isSuccess, string? errorMessage)> CreateNoteAsync(NoteCreateViewModel model)
        {
            Notebook? notebook = await notebookRepository.GetByIdAsync(model.NotebookId);
            if (notebook == null)
            {
                return (false, "The selected notebook does not exist.");
            }

            if (model.FolderId != Guid.Empty && model.FolderId != null)
            {
                Folder? folder = await folderRepository.GetByIdAsync(model.FolderId.Value);
                if (folder == null)
                {
                    return (false, "The selected folder does not exist.");
                }
            }

            Note note = new Note
            {
                Title = model.Title,
                Body = model.Body,
                NotebookId = model.NotebookId,
                Notebook = notebook,
                FolderId = model.FolderId,
                TagId = model.SelectedTagId,
                DateCreated = model.DateCreated,
                LastSaved = model.LastSaved
            };

            await noteRepository.AddAsync(note);

            model.Tags = await GetAllTagsAsync();

            return (true, null);
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