using LunarApp.Data.Models;
using LunarApp.Data.Repository.Interfaces;
using LunarApp.Services.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LunarApp.Services.Data
{
    public class FolderHelperService(IRepository<Folder, Guid> folderRepository) : IFolderHelperService
    {
        public async Task<Guid> GetParentFolderIdAsync(Guid? parentFolderId, Guid newParentFolderId)
        {
            Folder? folder = await folderRepository
                .GetAllAttached()
                .SelectMany(f => f.ChildrenFolders)
                .Where(f => f.Id == parentFolderId)
                .FirstOrDefaultAsync();

            if (folder != null && folder.ParentFolderId.HasValue && folder.ParentFolderId != Guid.Empty)
            {
                newParentFolderId = folder.ParentFolderId.Value;
            }

            return newParentFolderId;
        }

        public async Task<(Guid newParentFolderId, Guid newFolderId)> GetFolderAndParentIdsAsync(Guid? parentFolderId, Guid newParentFolderId, Guid newFolderId)
        {
            Folder? folder = await folderRepository
                .GetAllAttached()
                .SelectMany(f => f.ChildrenFolders)
                .Where(f => f.Id == parentFolderId)
                .FirstOrDefaultAsync();

            if (folder != null && folder.ParentFolderId.HasValue && folder.ParentFolderId != Guid.Empty)
            {
                newParentFolderId = folder.ParentFolderId.Value;
            }
            else if (folder == null && parentFolderId != null)
            {
                newFolderId = parentFolderId.Value;
            }

            return (newParentFolderId, newFolderId);
        }
    }
}
