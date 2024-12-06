﻿using LunarApp.Web.ViewModels.Tag;

namespace LunarApp.Services.Data.Interfaces
{
    public interface ITagService
    {
        Task<IEnumerable<TagViewModel>> IndexGetAllTagsOrderedByNameAsync();
        Task<TagCreateViewModel> GetCreateTagAsync(Guid? notebookId, Guid? parentFolderId, Guid? folderId,
            Guid? noteId);
        Task CreateTagAsync(TagCreateViewModel model);
        Task<TagEditViewModel?> GetTagForEditByIdAsync(Guid? notebookId, Guid? parentFolderId, Guid? folderId,
            Guid? noteId, Guid tagId);
        Task<bool> EditTagAsync(TagEditViewModel model);
        Task<TagRemoveViewModel> GetTagForDeleteByIdAsync(Guid notebookId, Guid? parentFolderId, Guid? folderId,
            Guid noteId, Guid tagId);
        Task DeleteTagAsync(Guid tagId);
    }
}
