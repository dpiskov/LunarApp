using LunarApp.Data.Models;
using LunarApp.Data.Repository.Interfaces;
using LunarApp.Services.Data.Interfaces;
using LunarApp.Web.ViewModels.Note;
using LunarApp.Web.ViewModels.Tag;
using Microsoft.EntityFrameworkCore;

namespace LunarApp.Services.Data
{
    /// <summary>
    /// Service for managing notes, including creating, editing, deleting, and retrieving notes and their associated tags.
    /// </summary>
    /// <remarks>
    /// The <see cref="NoteService"/> class provides operations for managing <see cref="Note"/> entities, including 
    /// create, read, update, and delete (CRUD) operations. It also handles the retrieval and management of tags associated 
    /// with notes, interacting with <see cref="Notebook"/>, <see cref="Folder"/>, and <see cref="Tag"/> entities in the data source.
    /// </remarks>
    /// <param name="notebookRepository">The repository for interacting with <see cref="Notebook"/> entities in the data source.</param>
    /// <param name="folderRepository">The repository for interacting with <see cref="Folder"/> entities in the data source.</param>
    /// <param name="noteRepository">The repository for interacting with <see cref="Note"/> entities in the data source.</param>
    /// <param name="tagRepository">The repository for interacting with <see cref="Tag"/> entities in the data source.</param>
    public class NoteService(
        IRepository<Notebook, Guid> notebookRepository,
        IRepository<Folder, Guid> folderRepository,
        IRepository<Note, Guid> noteRepository,
        IRepository<Tag, Guid> tagRepository)
        : INoteService
    {
        /// <summary>
        /// Retrieves a model to create a new note, including available tags and folder information.
        /// </summary>
        /// <param name="notebookId">The ID of the notebook the note belongs to.</param>
        /// <param name="parentFolderId">The ID of the parent folder, if any.</param>
        /// <param name="folderId">The ID of the folder, if any.</param>
        /// <param name="userId">The ID of the user creating the note.</param>
        /// <returns>A task representing the asynchronous operation, with a view model containing the note creation data.</returns>
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

        /// <summary>
        /// Creates a new note based on the provided model.
        /// </summary>
        /// <param name="model">The model containing the details for the new note.</param>
        /// <param name="userId">The ID of the user creating the note.</param>
        /// <returns>A task representing the asynchronous operation, with a success flag and an optional error message.</returns>
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

        /// <summary>
        /// Retrieves a note's details for deletion based on its ID, user ID, and associated notebook and folder details.
        /// </summary>
        /// <param name="notebookId">The ID of the notebook the note belongs to.</param>
        /// <param name="parentFolderId">The ID of the parent folder, if any.</param>
        /// <param name="folderId">The ID of the folder, if any.</param>
        /// <param name="noteId">The ID of the note to be deleted.</param>
        /// <param name="userId">The ID of the user who owns the note.</param>
        /// <returns>A task representing the asynchronous operation, with a view model containing the note's details for deletion, or null if not found.</returns>
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

        /// <summary>
        /// Deletes a note based on its ID and user ID.
        /// </summary>
        /// <param name="noteId">The ID of the note to be deleted.</param>
        /// <param name="userId">The ID of the user who owns the note.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
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

        /// <summary>
        /// Retrieves a note's details for editing based on its ID, user ID, and associated notebook and folder details.
        /// </summary>
        /// <param name="notebookId">The ID of the notebook the note belongs to.</param>
        /// <param name="parentFolderId">The ID of the parent folder, if any.</param>
        /// <param name="folderId">The ID of the folder, if any.</param>
        /// <param name="noteId">The ID of the note to be edited.</param>
        /// <param name="userId">The ID of the user who owns the note.</param>
        /// <returns>A task representing the asynchronous operation, with a view model containing the note's details for editing, or null if not found.</returns>
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

        /// <summary>
        /// Edits an existing note's details based on the provided model.
        /// </summary>
        /// <param name="model">The model containing the updated note details.</param>
        /// <param name="userId">The ID of the user who owns the note.</param>
        /// <returns>A task representing the asynchronous operation, with a result indicating whether the update was successful.</returns>
        public async Task<bool> EditNoteAsync(NoteEditViewModel? model, Guid userId)
        {
            if (model == null || string.IsNullOrWhiteSpace(model.Title))
            {
                return false;
            }

            Note? note = await noteRepository
                .GetAllAttached()
                .Where(n => n.Id == model.Id && n.UserId == userId)
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

        /// <summary>
        /// Retrieves all tags associated with a user.
        /// </summary>
        /// <param name="userId">The ID of the user whose tags are to be retrieved.</param>
        /// <returns>A task representing the asynchronous operation, with a list of tags associated with the user.</returns>
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