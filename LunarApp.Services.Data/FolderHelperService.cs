using LunarApp.Data.Models;
using LunarApp.Data.Repository.Interfaces;
using LunarApp.Services.Data.Interfaces;

namespace LunarApp.Services.Data
{
    public class FolderHelperService(IRepository<Folder, Guid> folderRepository) : IFolderHelperService
    {
        public Task<Guid> GetParentFolderIdAsync(Guid? parentFolderId, Guid newParentFolderId)
        {
            throw new NotImplementedException();
        }

        public Task<(Guid newParentFolderId, Guid newFolderId)> GetFolderAndParentIdsAsync(Guid? parentFolderId, Guid newParentFolderId, Guid newFolderId)
        {
            throw new NotImplementedException();
        }
    }
}
