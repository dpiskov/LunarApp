using LunarApp.Data.Models;
using LunarApp.Web.ViewModels.Folder;
using LunarApp.Web.ViewModels.Note;
using LunarApp.Web.ViewModels.Tag;

namespace LunarApp.Services.Data.Interfaces
{
    public interface INoteService
    {
        Task<NoteCreateViewModel> GetCreateNoteAsync(Guid notebookId, Guid? parentFolderId, Guid? folderId);
        Task<(bool isSuccess, string? errorMessage)> CreateNoteAsync(NoteCreateViewModel model);
        Task<NoteDeleteViewModel?> GetNoteForDeleteByIdAsync(Guid notebookId, Guid? parentFolderId, Guid? folderId,
            Guid noteId);
        Task DeleteNoteAsync(Guid noteId);
        Task<NoteEditViewModel?> GetNoteForEditByIdAsync(Guid notebookId, Guid? parentFolderId, Guid? folderId, Guid noteId);
        Task<bool> EditNoteAsync(NoteEditViewModel model);


        Task<List<TagViewModel>> GetAllTagsAsync();
    }
}
