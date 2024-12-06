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
        public Task<IEnumerable<TagViewModel>> IndexGetAllTagsOrderedByNameAsync()
        {
            throw new NotImplementedException();
        }

        public Task<TagCreateViewModel> GetCreateTagAsync(Guid? notebookId, Guid? parentFolderId, Guid? folderId, Guid? noteId)
        {
            throw new NotImplementedException();
        }

        public Task CreateTagAsync(TagCreateViewModel model)
        {
            throw new NotImplementedException();
        }

        public Task<TagEditViewModel?> GetTagForEditByIdAsync(Guid? notebookId, Guid? parentFolderId, Guid? folderId, Guid? noteId, Guid tagId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> EditTagAsync(TagEditViewModel? model)
        {
            throw new NotImplementedException();
        }

        public Task<TagRemoveViewModel?> GetTagForDeleteByIdAsync(Guid notebookId, Guid? parentFolderId, Guid? folderId, Guid noteId, Guid tagId)
        {
            throw new NotImplementedException();
        }

        public Task DeleteTagAsync(Guid tagId)
        {
            throw new NotImplementedException();
        }
    }
}
