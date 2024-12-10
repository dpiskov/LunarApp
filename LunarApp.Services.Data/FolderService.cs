using LunarApp.Data.Models;
using LunarApp.Data.Repository.Interfaces;
using LunarApp.Services.Data.Interfaces;
using LunarApp.Web.ViewModels.Folder;
using LunarApp.Web.ViewModels.Note;
using Microsoft.EntityFrameworkCore;

namespace LunarApp.Services.Data
{
    public class FolderService(
        IRepository<Notebook, Guid> notebookRepository,
        IRepository<Folder, Guid> folderRepository,
        IRepository<Note, Guid> noteRepository)
        : IFolderService
    {
        public async Task<Folder?> GetByTitleForEditAsync(string title, Guid? parentFolderId, Guid folderId, Guid userId)
        {
            // Check if a folder with the same title exists in the parent folder
            return await folderRepository
                .GetAllAttached()
                .Where(f => f.UserId == userId && f.ParentFolderId == parentFolderId && f.Id != folderId) // Same parent folder, exclude current folder
                .FirstOrDefaultAsync(f => f.Title == title);
        }

        public async Task<Folder?> GetByTitleInNotebookAsync(string title, Guid notebookId, Guid userId)
        {
            // Check if a folder with the same title exists directly in the notebook
            return await folderRepository
                .GetAllAttached()
                .Where(f => f.UserId == userId && f.NotebookId == notebookId && f.ParentFolderId == null) // Check only top-level folders in the notebook
                .FirstOrDefaultAsync(f => f.Title == title);
        }
        public async Task<Folder?> GetByTitleAsync(string title, Guid notebookId, Guid? parentFolderId, Guid? folderId)
        {
            // Get the deepest folder
            var openedFolder = await folderRepository.GetAllAttached()
                .FirstOrDefaultAsync(f => f.Id == folderId);

            if (openedFolder == null)
            {
                return null; // Folder not found
            }

            // Check if a folder with the same title exists in the deepest folder
            return await folderRepository
                .GetAllAttached()
                .Where(f => f.ParentFolderId == openedFolder.Id) // Only check within the deepest folder
                .FirstOrDefaultAsync(f => f.Title == title);
        }

        public async Task<FolderNotesViewModel> IndexGetAllFoldersAsync(Guid notebookId, Guid? parentFolderId,
    Guid? folderId, Guid userId)
        {
            IEnumerable<FolderInfoViewModel> folders = new List<FolderInfoViewModel>();
            IEnumerable<NoteInfoViewModel> notes = new List<NoteInfoViewModel>();

            if (folderId != Guid.Empty && folderId != null && notebookId != Guid.Empty && notebookId != null)
            {
                folders = await folderRepository
                    .GetAllAttached()
                    .Where(f => f.UserId == userId)
                    .Where(f => f.ParentFolderId == folderId)
                    .Select(f => new FolderInfoViewModel
                    {
                        Id = f.Id,
                        Title = f.Title,
                        NotebookId = f.NotebookId,
                        ParentFolderId = f.ParentFolderId
                    })
                    .OrderBy(f => f.Title)
                    .ToListAsync();

                notes = await noteRepository
                    .GetAllAttached()
                    .Where(f => f.UserId == userId)
                    .Where(n => n.FolderId == folderId)
                    .Select(n => new NoteInfoViewModel
                    {
                        Id = n.Id,
                        Title = n.Title,
                        NotebookId = n.NotebookId,
                        ParentFolderId = parentFolderId,
                        FolderId = n.FolderId
                    })
                    .OrderBy(n => n.Title)
                    .ToListAsync();
            }
            else if (notebookId != Guid.Empty && notebookId != null)
            {
                folders = await folderRepository
                    .GetAllAttached()
                    .Where(f => f.UserId == userId)
                    .Where(f => f.NotebookId == notebookId &&
                                f.ParentFolderId == null)
                    .Select(f => new FolderInfoViewModel
                    {
                        Id = f.Id,
                        Title = f.Title,
                        NotebookId = f.NotebookId,
                        ParentFolderId = parentFolderId
                    })
                    .OrderBy(f => f.Title)
                    .ToListAsync();

                notes = await noteRepository
                    .GetAllAttached()
                    .Where(f => f.UserId == userId)
                    .Where(n => n.FolderId == null)
                    .Where(nb => nb.NotebookId == notebookId)
                    .Select(n => new NoteInfoViewModel
                    {
                        Id = n.Id,
                        Title = n.Title,
                        NotebookId = n.NotebookId,
                        ParentFolderId = parentFolderId,
                        FolderId = n.FolderId
                    })
                    .OrderBy(n => n.Title)
                    .ToListAsync();
            }

            return new FolderNotesViewModel
            {
                Folders = folders,
                Notes = notes
            };
        }

        public async Task<FolderCreateViewModel> GetAddFolderModelAsync(Guid notebookId, Guid? parentFolderId, Guid? folderId)
        {
            //bool isMadeDirectlyFromNotebook = folderId == Guid.Empty || folderId == null &&
            //    parentFolderId == Guid.Empty || parentFolderId == null;
            bool isMadeDirectlyFromNotebook = (folderId == Guid.Empty || folderId == null) &&
                                              (parentFolderId == Guid.Empty || parentFolderId == null);


            return new FolderCreateViewModel
            {
                Title = string.Empty,
                NotebookId = notebookId,
                ParentFolderId = parentFolderId,
                FolderId = folderId,
                IsMadeDirectlyFromNotebook = isMadeDirectlyFromNotebook
            };
        }

        public async Task<(bool isSuccess, string? errorMessage)> AddFolderAsync(FolderCreateViewModel model, Guid userId)
        {
            Notebook? notebook = await notebookRepository.GetByIdAsync(model.NotebookId);

            if (notebook == null)
            {
                return (false, "The selected notebook does not exist.");
            }

            Folder folder = new Folder
            {
                Title = model.Title,
                NotebookId = model.NotebookId,
                Notebook = notebook,
                ParentFolderId = model.FolderId,
                UserId = userId
            };

            await folderRepository.AddAsync(folder);

            return (true, null);
        }

        public async Task<(FolderCreateViewModel model, Guid newParentFolderId)> GetAddSubfolderModelAsync(Guid notebookId, Guid? parentFolderId, Guid? folderId, Guid userId)
        {
            bool isMadeDirectlyFromNotebook = parentFolderId == Guid.Empty || parentFolderId == null && folderId != Guid.Empty && folderId != null;

            FolderCreateViewModel model = new FolderCreateViewModel
            {
                Title = string.Empty,
                NotebookId = notebookId,
                ParentFolderId = parentFolderId,
                FolderId = folderId,
                IsMadeDirectlyFromNotebook = isMadeDirectlyFromNotebook
            };

            Guid newParentFolderId = Guid.Empty;

            if (parentFolderId != Guid.Empty && parentFolderId != null)
            {
                newParentFolderId = await GetParentFolderIdAsync(parentFolderId, newParentFolderId, userId);
            }

            return (model, newParentFolderId);
        }

        public async Task<(FolderDeleteViewModel? model, Guid newParentFolderId)> GetFolderForDeleteByIdAsync(Guid notebookId, Guid? parentFolderId, Guid folderId, Guid userId)
        {
            FolderDeleteViewModel? model = await folderRepository
                .GetAllAttached()
                .Where(f => f.Id == folderId)
                .AsNoTracking()
                .Select(f => new FolderDeleteViewModel()
                {
                    Title = f.Title,
                    NotebookId = f.NotebookId,
                    ParentFolderId = f.ParentFolderId,
                    FolderId = f.Id
                })
                .FirstOrDefaultAsync();

            Guid newParentFolderId = Guid.Empty;

            if (parentFolderId != Guid.Empty && parentFolderId != null)
            {
                newParentFolderId = await GetParentFolderIdAsync(parentFolderId, newParentFolderId, userId);
            }

            return (model, newParentFolderId);
        }

        public async Task DeleteFolderWithChildrenAsync(Guid folderId, Guid userId)
        {
            Folder? folder = await folderRepository
                .GetAllAttached()
                .Include(f => f.Notes)
                .Include(f => f.ChildrenFolders)
                .FirstOrDefaultAsync(f => f.Id == folderId && f.UserId == userId);

            if (folder == null)
            {
                throw new InvalidOperationException("Folder not found.");
            }

            List<Folder> childFolders = folder.ChildrenFolders.ToList();

            foreach (Folder? childFolder in childFolders)
            {
                await DeleteFolderWithChildrenAsync(childFolder.Id, userId);
            }

            await noteRepository.DeleteRangeAsync(folder.Notes);
            await folderRepository.DeleteAsync(folder.Id);

            await folderRepository.SaveChangesAsync();
        }

        public async Task<(FolderEditViewModel? model, Guid newParentFolderId)> GetFolderForEditByIdAsync(Guid notebookId, Guid? parentFolderId, Guid folderId, Guid userId)
        {
            //bool isAccessedDirectlyFromNotebook = folderId == Guid.Empty || folderId == null &&
            //    parentFolderId == Guid.Empty || parentFolderId == null;

            bool isAccessedDirectlyFromNotebook = (folderId == Guid.Empty || folderId == null) &&
                                                  (parentFolderId == Guid.Empty || parentFolderId == null);

            Folder? folder = await folderRepository
                .GetAllAttached()
                .Where(f => f.UserId == userId && f.Id == folderId)
                .AsNoTracking()
                .FirstOrDefaultAsync();

            if (folder == null)
            {
                return (null, Guid.Empty);
            }

            FolderEditViewModel model = new FolderEditViewModel
            {
                Title = folder.Title,
                NotebookId = notebookId,
                ParentFolderId = parentFolderId,
                FolderId = folderId,
                IsAccessedDirectlyFromNotebook = isAccessedDirectlyFromNotebook
            };

            Guid newParentFolderId = Guid.Empty;

            if (parentFolderId != Guid.Empty && parentFolderId != null)
            {
                newParentFolderId = await GetParentFolderIdAsync(parentFolderId, newParentFolderId, userId);
            }

            return (model, newParentFolderId);
        }

        public async Task<(bool isEdited, Folder? parentFolder)> EditFolderAsync(FolderEditViewModel? model)
        {
            if (model == null || string.IsNullOrWhiteSpace(model.Title))
            {
                return (false, null);
            }

            Folder? folder = await folderRepository.GetByIdAsync(model.FolderId);
            if (folder == null)
            {
                return (false, null);
            }

            folder.Title = model.Title;

            Folder? parentFolder = null;

            if (model.ParentFolderId.HasValue)
            {
                parentFolder = await folderRepository.GetByIdAsync(model.ParentFolderId.Value);
            }

            bool isEdited = await folderRepository.UpdateAsync(folder);

            return (isEdited, parentFolder);
        }

        public async Task<(FolderDetailsViewModel? model, Guid newParentFolderId)> GetFolderDetailsByIdAsync(Guid notebookId, Guid? parentFolderId, Guid folderId, Guid userId)
        {
            bool isAccessedDirectlyFromNotebook = folderId == Guid.Empty || folderId == null &&
                parentFolderId == Guid.Empty || parentFolderId == null;

            Folder? folder = await folderRepository
                .GetAllAttached()
                .Where(f => f.UserId == userId && f.Id == folderId)
                .AsNoTracking()
                .FirstOrDefaultAsync();

            if (folder == null)
            {
                return (null, Guid.Empty);
            }

            FolderDetailsViewModel? model = new FolderDetailsViewModel()
            {
                Title = folder.Title,
                Description = folder.Description,
                NotebookId = notebookId,
                ParentFolderId = parentFolderId,
                FolderId = folderId,
                IsAccessedDirectlyFromNotebook = isAccessedDirectlyFromNotebook
            };

            Guid newParentFolderId = Guid.Empty;

            if (parentFolderId != Guid.Empty && parentFolderId != null)
            {
                newParentFolderId = await GetParentFolderIdAsync(parentFolderId, newParentFolderId, userId);
            }

            return (model, newParentFolderId);
        }

        public async Task<(bool isEdited, Folder? parentFolder)> EditDetailsFolderAsync(FolderDetailsViewModel? model)
        {
            if (model == null || string.IsNullOrWhiteSpace(model.Title))
            {
                return (false, null);
            }

            Folder? folder = await folderRepository.GetByIdAsync(model.FolderId);

            if (folder == null)
            {
                return (false, null);
            }

            folder.Description = model.Description;

            Folder? parentFolder = null;

            if (model.ParentFolderId.HasValue)
            {
                parentFolder = await folderRepository.GetByIdAsync(model.ParentFolderId.Value);
            }

            bool isEdited = await folderRepository.UpdateAsync(folder);

            return (isEdited, parentFolder);
        }

        public async Task<string?> GetFolderTitleAsync(Guid folderId)
        {
            return await folderRepository
                .GetAllAttached()
                .Where(f => f.Id == folderId)
                .Select(f => f.Title)
                .FirstOrDefaultAsync();
        }

        public async Task<string?> GetNotebookTitleAsync(Guid notebookId, Guid userId)
        {
            return await notebookRepository
                .GetAllAttached()
                .Where(nb => nb.Id == notebookId && nb.UserId == userId)
                .Select(nb => nb.Title)
                .FirstOrDefaultAsync();
        }

        public async Task<Guid> GetParentFolderIdAsync(Guid? parentFolderId, Guid newParentFolderId, Guid userId)
        {
            Folder? folder = await folderRepository
                .GetAllAttached()
                .Where(f => f.UserId == userId)
                .SelectMany(f => f.ChildrenFolders)
                .Where(f => f.Id == parentFolderId)
                .FirstOrDefaultAsync();

            if (folder != null && folder.ParentFolderId.HasValue && folder.ParentFolderId != Guid.Empty)
            {
                newParentFolderId = folder.ParentFolderId.Value;
            }

            return newParentFolderId;
        }

        public async Task<(Guid newParentFolderId, Guid newFolderId)> GetFolderAndParentIdsAsync(Guid? parentFolderId, Guid newParentFolderId, Guid newFolderId, Guid userId)
        {
            Folder? folder = await folderRepository
                .GetAllAttached()
                .Where(f => f.UserId == userId)
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

        public async Task<Folder?> GetFolderForRedirectionAsync(Guid folderId, Guid? parentFolderId)
        {
            Folder? folder = await folderRepository
                .GetAllAttached()
                .Where(f => f.Id == folderId)
                .Select(f => new Folder
                {
                    Id = f.Id,
                    Title = f.Title,
                    ParentFolderId = f.ParentFolderId,
                    NotebookId = f.NotebookId,
                })
                .FirstOrDefaultAsync();

            if (folder == null && parentFolderId != null)
            {
                folder = await folderRepository
                    .GetAllAttached()
                    .Where(f => f.Id == parentFolderId)
                    .Select(f => new Folder
                    {
                        Id = f.Id,
                        Title = f.Title,
                        ParentFolderId = f.ParentFolderId,
                        NotebookId = f.NotebookId,
                    })
                    .FirstOrDefaultAsync();
            }

            return folder;
        }
    }
}