using LunarApp.Data.Models;
using LunarApp.Data.Repository.Interfaces;
using LunarApp.Services.Data.Interfaces;
using LunarApp.Web.ViewModels.Note;
using LunarApp.Web.ViewModels.Tag;

namespace LunarApp.Services.Data
{
    public class NoteService(
        IRepository<Notebook, Guid> notebookRepository,
        IRepository<Folder, Guid> folderRepository,
        IRepository<Note, Guid> noteRepository,
        IRepository<Tag, Guid> tagRepository)
        : INoteService
    {
        public Task<NoteCreateViewModel> GetCreateNoteAsync(Guid notebookId, Guid? parentFolderId, Guid? folderId)
        {
            throw new NotImplementedException();
        }

        public Task<(bool isSuccess, string? errorMessage)> CreateNoteAsync(NoteCreateViewModel model)
        {
            throw new NotImplementedException();
        }

        public Task<NoteDeleteViewModel?> GetNoteForDeleteByIdAsync(Guid notebookId, Guid? parentFolderId, Guid? folderId, Guid noteId)
        {
            throw new NotImplementedException();
        }

        public Task DeleteNoteAsync(Guid noteId)
        {
            throw new NotImplementedException();
        }

        public Task<NoteEditViewModel?> GetNoteForEditByIdAsync(Guid notebookId, Guid? parentFolderId, Guid? folderId, Guid noteId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> EditNoteAsync(NoteEditViewModel model)
        {
            throw new NotImplementedException();
        }

        public Task<List<TagViewModel>> GetAllTagsAsync()
        {
            throw new NotImplementedException();
        }
    }
}