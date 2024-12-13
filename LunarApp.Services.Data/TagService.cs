using LunarApp.Data.Models;
using LunarApp.Data.Repository.Interfaces;
using LunarApp.Services.Data.Interfaces;
using LunarApp.Web.ViewModels.Tag;
using Microsoft.EntityFrameworkCore;

namespace LunarApp.Services.Data
{
    /// <summary>
    /// Service for managing tags, including creating, editing, deleting, and retrieving tags.
    /// </summary>
    /// <remarks>
    /// The <see cref="TagService"/> class provides operations for managing <see cref="Tag"/> entities, including 
    /// create, read, update, and delete (CRUD) operations. It interacts with the <see cref="Tag"/> repository 
    /// to perform actions related to tag management.
    /// </remarks>
    /// <param name="tagRepository">The repository for interacting with <see cref="Tag"/> entities in the data source.</param>
    public class TagService(
        IRepository<Tag, Guid> tagRepository
        ) : ITagService
    {
        /// <summary>
        /// Retrieves a tag by its name and the associated user ID.
        /// </summary>
        /// <param name="name">The name of the tag to retrieve.</param>
        /// <param name="userId">The ID of the user who owns the tag.</param>
        /// <returns>A task representing the asynchronous operation, with the tag if found, or null if not found.</returns>
        public async Task<Tag?> GetByTitleAsync(string name, Guid userId)
        {
            return await tagRepository.FirstOrDefaultAsync(n => n.Name == name && n.UserId == userId);
        }

        /// <summary>
        /// Retrieves all tags associated with a user, ordered by name.
        /// </summary>
        /// <param name="userId">The ID of the user whose tags are to be retrieved.</param>
        /// <returns>A task representing the asynchronous operation, with a list of tags ordered by name.</returns>
        public async Task<IEnumerable<TagViewModel>> IndexGetAllTagsOrderedByNameAsync(Guid userId)
        {
            IEnumerable<TagViewModel> tags = await tagRepository
                .GetAllAttached()
                .Where(t => t.UserId == userId)
                .Select(t => new TagViewModel
                {
                    Id = t.Id,
                    Name = t.Name
                })
                .OrderBy(t => t.Name)
                .AsNoTracking()
                .ToListAsync();

            return tags;
        }

        /// <summary>
        /// Retrieves a model to create a new tag, including associated notebook, folder, and note information if provided.
        /// </summary>
        /// <param name="notebookId">The ID of the associated notebook, if any.</param>
        /// <param name="parentFolderId">The ID of the parent folder, if any.</param>
        /// <param name="folderId">The ID of the folder, if any.</param>
        /// <param name="noteId">The ID of the associated note, if any.</param>
        /// <returns>A task representing the asynchronous operation, with a view model containing the tag creation data.</returns>
        public async Task<TagCreateViewModel> GetCreateTagAsync(Guid? notebookId, Guid? parentFolderId, Guid? folderId, Guid? noteId)
        {
            TagCreateViewModel model = new TagCreateViewModel
            {
                Name = string.Empty,
                NotebookId = notebookId,
                ParentFolderId = parentFolderId,
                FolderId = folderId,
                NoteId = noteId
            };

            return model;
        }

        /// <summary>
        /// Creates a new tag based on the provided model.
        /// </summary>
        /// <param name="model">The model containing the details for the new tag.</param>
        /// <param name="userId">The ID of the user creating the tag.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task CreateTagAsync(TagCreateViewModel model, Guid userId)
        {
            Tag tag = new Tag
            {
                Name = model.Name,
                UserId = userId
            };

            await tagRepository.AddAsync(tag);
        }

        /// <summary>
        /// Retrieves a tag's details for editing based on its ID and the user ID.
        /// </summary>
        /// <param name="notebookId">The ID of the associated notebook, if any.</param>
        /// <param name="parentFolderId">The ID of the parent folder, if any.</param>
        /// <param name="folderId">The ID of the folder, if any.</param>
        /// <param name="noteId">The ID of the associated note, if any.</param>
        /// <param name="tagId">The ID of the tag to be edited.</param>
        /// <param name="userId">The ID of the user who owns the tag.</param>
        /// <returns>A task representing the asynchronous operation, with a view model containing the tag's details for editing, or null if not found.</returns>
        public async Task<TagEditViewModel?> GetTagForEditByIdAsync(Guid? notebookId, Guid? parentFolderId, Guid? folderId, Guid? noteId, Guid tagId, Guid userId)
        {
            Tag? tag = await tagRepository
                .GetAllAttached()
                .Where(t => t.Id == tagId && t.UserId == userId)
                .AsNoTracking()
                .FirstOrDefaultAsync();

            if (tag == null)
            {
                return null;
            }

            TagEditViewModel model = new TagEditViewModel
            {
                Id = tagId,
                Name = tag.Name,
                NotebookId = notebookId,
                ParentFolderId = parentFolderId,
                FolderId = folderId,
                NoteId = noteId
            };

            return model;
        }

        /// <summary>
        /// Edits an existing tag's details based on the provided model.
        /// </summary>
        /// <param name="model">The model containing the updated tag details.</param>
        /// <returns>A task representing the asynchronous operation, with a result indicating whether the update was successful.</returns>
        public async Task<bool> EditTagAsync(TagEditViewModel? model)
        {
            if (model == null || string.IsNullOrWhiteSpace(model.Name))
            {
                return false;
            }

            Tag? tag = await tagRepository.GetByIdAsync(model.Id);

            if (tag == null)
            {
                return false;
            }

            tag.Name = model.Name;

            bool isEdited = await tagRepository.UpdateAsync(tag);

            return isEdited;
        }

        /// <summary>
        /// Retrieves a tag's details for deletion based on its ID, with associated notebook, folder, and note information if provided.
        /// </summary>
        /// <param name="notebookId">The ID of the associated notebook, if any.</param>
        /// <param name="parentFolderId">The ID of the parent folder, if any.</param>
        /// <param name="folderId">The ID of the folder, if any.</param>
        /// <param name="noteId">The ID of the associated note, if any.</param>
        /// <param name="tagId">The ID of the tag to be deleted.</param>
        /// <returns>A task representing the asynchronous operation, with a view model containing the tag's details for deletion, or null if not found.</returns>
        public async Task<TagRemoveViewModel?> GetTagForDeleteByIdAsync(Guid notebookId, Guid? parentFolderId, Guid? folderId, Guid? noteId, Guid tagId)
        {
            TagRemoveViewModel? model = await tagRepository
                .GetAllAttached()
                .Where(t => t.Id == tagId)
                .AsNoTracking()
                .Select(t => new TagRemoveViewModel()
                {
                    Id = t.Id,
                    Name = t.Name,
                    NotebookId = notebookId,
                    ParentFolderId = parentFolderId,
                    FolderId = folderId,
                    NoteId = noteId
                })
                .FirstOrDefaultAsync();

            return model;
        }

        /// <summary>
        /// Deletes a tag based on its ID.
        /// </summary>
        /// <param name="tagId">The ID of the tag to be deleted.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task DeleteTagAsync(Guid tagId)
        {
            Tag? tag = await tagRepository
                .GetAllAttached()
                .Where(t => t.Id == tagId)
                .FirstOrDefaultAsync();

            if (tag == null)
            {
                return;
            }

            await tagRepository.DeleteAsync(tagId);
        }
    }
}
