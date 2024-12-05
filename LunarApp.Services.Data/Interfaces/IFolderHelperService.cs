namespace LunarApp.Services.Data.Interfaces
{
    public interface IFolderHelperService
    {
        Task<Guid> GetParentFolderIdAsync(Guid? parentFolderId, Guid newParentFolderId);

        Task<(Guid newParentFolderId, Guid newFolderId)> GetFolderAndParentIdsAsync(Guid? parentFolderId,
            Guid newParentFolderId, Guid newFolderId);
    }
}