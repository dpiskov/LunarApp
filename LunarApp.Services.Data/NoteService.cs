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
        public async Task<NoteCreateViewModel> GetCreateNoteAsync(Guid notebookId, Guid? parentFolderId, Guid? folderId, Guid userId)
        {
            List<TagViewModel> tags = await GetAllTagsAsync(userId);

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

        public async Task<(bool isSuccess, string? errorMessage)> CreateNoteAsync(NoteCreateViewModel model, Guid userId)
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
                ParentFolderId = model.ParentFolderId,
                FolderId = model.FolderId,
                TagId = model.SelectedTagId,
                UserId = userId,
                DateCreated = model.DateCreated,
                LastSaved = model.LastSaved
            };

            await noteRepository.AddAsync(note);

            model.Tags = await GetAllTagsAsync(userId);

            return (true, null);
        }

        public async Task<NoteDeleteViewModel?> GetNoteForDeleteByIdAsync(Guid notebookId, Guid? parentFolderId, Guid? folderId, Guid noteId, Guid userId)
        {
            NoteDeleteViewModel? model = await noteRepository
                .GetAllAttached()
                .Where(n => n.Id == noteId && n.UserId == userId)
                .AsNoTracking()
                .Select(n => new NoteDeleteViewModel()
                {
                    Id = n.Id,
                    Title = n.Title,
                    NotebookId = n.NotebookId,
                    ParentFolderId = n.ParentFolderId,
                    FolderId = n.FolderId,
                })
                .FirstOrDefaultAsync();

            return model;
        }

        public async Task DeleteNoteAsync(Guid noteId, Guid userId)
        {
            Note? note = await noteRepository
                .GetAllAttached()
                .Where(n => n.Id == noteId && n.UserId == userId)
                .FirstOrDefaultAsync();

            if (note == null)
            {
                return;
            }

            await noteRepository.DeleteAsync(noteId);
        }

        public async Task<NoteEditViewModel?> GetNoteForEditByIdAsync(Guid notebookId, Guid? parentFolderId, Guid? folderId, Guid noteId, Guid userId)
        {
            Note? note = await noteRepository
                .GetAllAttached()
                .Where(n => n.Id == noteId && n.UserId == userId)
                .AsNoTracking()
                .FirstOrDefaultAsync();

            if (note == null)
            {
                return null;
            }

            List<TagViewModel> tags = await GetAllTagsAsync(userId);

            NoteEditViewModel model = new NoteEditViewModel
            {
                Id = noteId,
                Title = note.Title,
                Body = note.Body,
                NotebookId = notebookId,
                ParentFolderId = parentFolderId,
                FolderId = folderId,
                DateCreated = note.DateCreated,
                LastSaved = note.LastSaved,
                SelectedTagId = note.TagId,
                Tags = tags
            };

            return model;
        }

        public async Task<bool> EditNoteAsync(NoteEditViewModel? model, Guid userId)
        {
            if (model == null || string.IsNullOrWhiteSpace(model.Title))
            {
                return false;
            }

            //Note? note = await noteRepository.GetByIdAsync(model.Id);
            Note? note = await noteRepository
                .GetAllAttached()
                .Where(n => n.Id == model.Id && n.UserId == userId) // Verify user ownership
                .FirstOrDefaultAsync();

            if (note == null)
            {
                return false;
            }

            note.Title = model.Title;
            note.Body = model.Body;
            note.TagId = model.SelectedTagId;
            note.LastSaved = DateTime.Now;

            bool isEdited = await noteRepository.UpdateAsync(note);

            return isEdited;
        }

        public async Task<List<TagViewModel>> GetAllTagsAsync(Guid userId)
        {
            return await tagRepository
                .GetAllAttached()
                .Where(t => t.UserId == userId)
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