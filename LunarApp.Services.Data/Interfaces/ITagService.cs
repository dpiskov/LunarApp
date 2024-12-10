using LunarApp.Data.Models;
using LunarApp.Web.ViewModels.Tag;

namespace LunarApp.Services.Data.Interfaces
{
    public interface ITagService
    {
        Task<Tag?> GetByTitleAsync(string name, Guid userId);

        Task<IEnumerable<TagViewModel>> IndexGetAllTagsOrderedByNameAsync(Guid userId);
        Task<TagCreateViewModel> GetCreateTagAsync(Guid? notebookId, Guid? parentFolderId, Guid? folderId,
            Guid? noteId);
        Task CreateTagAsync(TagCreateViewModel model, Guid userId);
        Task<TagEditViewModel?> GetTagForEditByIdAsync(Guid? notebookId, Guid? parentFolderId, Guid? folderId,
            Guid? noteId, Guid tagId, Guid userId);
        Task<bool> EditTagAsync(TagEditViewModel? model);
        Task<TagRemoveViewModel?> GetTagForDeleteByIdAsync(Guid notebookId, Guid? parentFolderId, Guid? folderId,
            Guid? noteId, Guid tagId);
        Task DeleteTagAsync(Guid tagId);
    }
}
