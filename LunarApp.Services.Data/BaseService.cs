using LunarApp.Data.Models;
using LunarApp.Data.Repository.Interfaces;
using LunarApp.Services.Data.Interfaces;
using LunarApp.Web.ViewModels.Folder;
using Microsoft.EntityFrameworkCore;

namespace LunarApp.Services.Data
{
    public class BaseService(IRepository<Tag, Guid> tagRepository, IRepository<Note, Guid> noteRepository)
        : IBaseService
    {
        public Task<FolderNotesViewModel> GetFilteredNotesAsyncByNotebookId(Guid notebookId, Guid? parentFolderId, Guid? folderId, string? searchQuery,
            string? tagFilter)
        {
            throw new NotImplementedException();
        }

        public Task<FolderNotesViewModel> GetFilteredNotesAsync(Guid? parentFolderId, Guid? folderId, string? searchQuery, string? tagFilter)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<string>> GetAllTagsAsync()
        {
            IEnumerable<string> allTags = await tagRepository
                .GetAllAttached()
                .Select(t => t.Name)
                .Distinct()
                .ToArrayAsync();

            return allTags;
        }
    }
}