using LunarApp.Web.ViewModels.Note;
using LunarApp.Web.ViewModels.Tag;

namespace LunarApp.Services.Data.Interfaces
{
    public interface INoteService
    {
        Task<NoteCreateViewModel> GetCreateNoteAsync(Guid notebookId, Guid? parentFolderId, Guid? folderId, Guid userId);
        Task<(bool isSuccess, string? errorMessage)> CreateNoteAsync(NoteCreateViewModel model, Guid userId);
        Task<NoteDeleteViewModel?> GetNoteForDeleteByIdAsync(Guid notebookId, Guid? parentFolderId, Guid? folderId,
            Guid noteId, Guid userId);
        Task DeleteNoteAsync(Guid noteId, Guid userId);
        Task<NoteEditViewModel?> GetNoteForEditByIdAsync(Guid notebookId, Guid? parentFolderId, Guid? folderId, Guid noteId, Guid userId);
        Task<bool> EditNoteAsync(NoteEditViewModel? model, Guid userId);


        Task<List<TagViewModel>> GetAllTagsAsync(Guid userId);
    }
}
