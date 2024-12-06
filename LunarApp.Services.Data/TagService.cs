using LunarApp.Data.Models;
using LunarApp.Data.Repository.Interfaces;
using LunarApp.Services.Data.Interfaces;
using LunarApp.Web.ViewModels.Tag;
using Microsoft.EntityFrameworkCore;

namespace LunarApp.Services.Data
{
    public class TagService(
        IRepository<Tag, Guid> tagRepository
        ) : ITagService
    {
        public async Task<IEnumerable<TagViewModel>> IndexGetAllTagsOrderedByNameAsync()
        {
            IEnumerable<TagViewModel> tags = await tagRepository
                .GetAllAttached()
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

        public async Task CreateTagAsync(TagCreateViewModel model)
        {
            Tag tag = new Tag
            {
                Name = model.Name
            };

            await tagRepository.AddAsync(tag);
        }

        public async Task<TagEditViewModel?> GetTagForEditByIdAsync(Guid? notebookId, Guid? parentFolderId, Guid? folderId, Guid? noteId, Guid tagId)
        {
            Tag? tag = await tagRepository
                .GetAllAttached()
                .Where(t => t.Id == tagId)
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

        public async Task<TagRemoveViewModel?> GetTagForDeleteByIdAsync(Guid notebookId, Guid? parentFolderId, Guid? folderId, Guid noteId, Guid tagId)
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
