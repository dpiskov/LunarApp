using LunarApp.Data.Models;
using LunarApp.Data.Repository.Interfaces;
using LunarApp.Services.Data.Interfaces;
using LunarApp.Web.ViewModels.Folder;
using LunarApp.Web.ViewModels.Note;
using Microsoft.EntityFrameworkCore;

namespace LunarApp.Services.Data
{
    /// <summary>
    /// Represents the base service for handling common operations across the application.
    /// This class provides shared logic for services that manage tags and notes.
    /// </summary>
    /// <remarks>
    /// The <see cref="BaseService"/> class utilizes repositories for interacting with the data models for <see cref="Tag"/> and <see cref="Note"/> entities.
    /// It implements <see cref="IBaseService"/> for general service-related functionalities.
    /// </remarks>
    /// <param name="tagRepository">The repository for interacting with <see cref="Tag"/> entities in the data source.</param>
    /// <param name="noteRepository">The repository for interacting with <see cref="Note"/> entities in the data source.</param>
    public class BaseService(IRepository<Tag, Guid> tagRepository, IRepository<Note, Guid> noteRepository)
        : IBaseService
    {
        /// <summary>
        /// Retrieves filtered notes for a specific notebook and folder, optionally filtered by a search query or tag.
        /// </summary>
        /// <param name="notebookId">ID of the notebook to filter notes.</param>
        /// <param name="parentFolderId">Optional ID of the parent folder.</param>
        /// <param name="folderId">Optional ID of the folder to filter notes.</param>
        /// <param name="userId">ID of the user requesting the notes.</param>
        /// <param name="searchQuery">Optional search query to filter notes by title.</param>
        /// <param name="tagFilter">Optional tag name to filter notes by tag.</param>
        /// <returns>A FolderNotesViewModel containing the filtered notes.</returns>
        public async Task<FolderNotesViewModel> GetFilteredNotesAsyncByNotebookId(Guid notebookId, Guid? parentFolderId, Guid? folderId, Guid userId, string? searchQuery, string? tagFilter)
        {
            Guid? tagId = null;

            // Retrieve tagId based on the tag name
            if (!string.IsNullOrWhiteSpace(tagFilter))
            {
                Guid tag = await tagRepository.GetAllAttached()
                    .Where(t => t.Name == tagFilter && t.UserId == userId)
                    .Select(t => t.Id)
                    .FirstOrDefaultAsync();

                if (tag != Guid.Empty)
                {
                    tagId = tag;
                }
            }

            IQueryable<Note> query = noteRepository
                .GetAllAttached()
                .Where(n => n.NotebookId == notebookId && n.UserId == userId);

            // Fetch notes from the database
            List<Note> notesInMemory = await query.ToListAsync();

            // Apply search query filter
            if (!string.IsNullOrWhiteSpace(searchQuery))
            {
                notesInMemory = notesInMemory
                    .Where(n => n.Title.Contains(searchQuery, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            // Apply tag filter
            if (tagId.HasValue)
            {
                notesInMemory = notesInMemory
                    .Where(n => n.TagId == tagId)
                    .ToList();
            }

            // Map the notes to the desired ViewModel
            List<NoteInfoViewModel> notes = notesInMemory.Select(n => new NoteInfoViewModel
            {
                Id = n.Id,
                Title = n.Title,
                NotebookId = n.NotebookId,
                ParentFolderId = parentFolderId,
                FolderId = n.FolderId,
                TagId = n.TagId
            })
                .OrderBy(n => n.Title)
                .ToList();

            return new FolderNotesViewModel
            {
                Folders = new List<FolderInfoViewModel>(),  // No folders, only notes
                Notes = notes
            };
        }

        /// <summary>
        /// Retrieves filtered notes for a user, optionally filtered by a search query or tag.
        /// </summary>
        /// <param name="userId">ID of the user requesting the notes.</param>
        /// <param name="searchQuery">Optional search query to filter notes by title.</param>
        /// <param name="tagFilter">Optional tag name to filter notes by tag.</param>
        /// <returns>A FolderNotesViewModel containing the filtered notes.</returns>
        public async Task<FolderNotesViewModel> GetFilteredNotesAsync(Guid userId, string? searchQuery, string? tagFilter)
        {
            Guid? tagId = null;

            // Retrieve tagId based on the tag name
            if (!string.IsNullOrWhiteSpace(tagFilter))
            {
                Guid tag = await tagRepository.GetAllAttached()
                    .Where(t => t.Name == tagFilter && t.UserId == userId)
                    .Select(t => t.Id)
                    .FirstOrDefaultAsync();

                if (tag != Guid.Empty)
                {
                    tagId = tag;
                }
            }

            IQueryable<Note> query = noteRepository
                .GetAllAttached()
                .Where(n => n.UserId == userId);

            // Fetch notes from the database
            List<Note> notesInMemory = await query.ToListAsync();

            // Apply search query filter
            if (!string.IsNullOrWhiteSpace(searchQuery))
            {
                notesInMemory = notesInMemory
                    .Where(n => n.Title.Contains(searchQuery, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            // Apply tag filter
            if (tagId.HasValue)
            {
                notesInMemory = notesInMemory
                    .Where(n => n.TagId == tagId)
                    .ToList();
            }

            // Map the notes to the desired ViewModel
            List<NoteInfoViewModel> notes = notesInMemory.Select(n => new NoteInfoViewModel
            {
                Id = n.Id,
                Title = n.Title,
                NotebookId = n.NotebookId,
                ParentFolderId = n.ParentFolderId,
                FolderId = n.FolderId,
                TagId = n.TagId
            })
                .OrderBy(n => n.Title)
                .ToList();

            return new FolderNotesViewModel
            {
                Folders = new List<FolderInfoViewModel>(),  // No folders, only notes
                Notes = notes
            };
        }

        /// <summary>
        /// Retrieves all distinct tag names associated with a specific user.
        /// </summary>
        /// <param name="userId">ID of the user whose tags should be retrieved.</param>
        /// <returns>An IEnumerable of tag names.</returns>
        public async Task<IEnumerable<string>> GetAllTagsAsync(Guid userId)
        {
            IEnumerable<string> allTags = await tagRepository
                .GetAllAttached()
                .Where(t => t.UserId == userId)
                .Select(t => t.Name)
                .Distinct()
                .ToArrayAsync();

            return allTags;
        }
    }
}