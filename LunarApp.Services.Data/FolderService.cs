using LunarApp.Data.Models;
using LunarApp.Data.Repository.Interfaces;
using LunarApp.Services.Data.Interfaces;
using LunarApp.Web.ViewModels.Folder;
using LunarApp.Web.ViewModels.Note;
using Microsoft.EntityFrameworkCore;

namespace LunarApp.Services.Data
{
    /// <summary>
    /// Provides services for managing folders and their related operations, 
    /// including retrieval, creation, editing, and deletion of folders and notes.
    /// </summary>
    /// <remarks>
    /// The <see cref="FolderService"/> class handles operations related to managing folders and their associated notes.
    /// It interacts with repositories for the <see cref="Notebook"/>, <see cref="Folder"/>, and <see cref="Note"/> entities,
    /// allowing for CRUD operations on these entities in the data source.
    /// </remarks>
    /// <param name="notebookRepository">The repository for interacting with <see cref="Notebook"/> entities in the data source.</param>
    /// <param name="folderRepository">The repository for interacting with <see cref="Folder"/> entities in the data source.</param>
    /// <param name="noteRepository">The repository for interacting with <see cref="Note"/> entities in the data source.</param>
    public class FolderService(
        IRepository<Notebook, Guid> notebookRepository,
        IRepository<Folder, Guid> folderRepository,
        IRepository<Note, Guid> noteRepository)
        : IFolderService
    {
        /// <summary>
        /// Checks if a folder with the specified title exists within the same parent folder for editing.
        /// </summary>
        /// <param name="title">The title of the folder to check.</param>
        /// <param name="parentFolderId">The ID of the parent folder.</param>
        /// <param name="folderId">The ID of the folder being edited.</param>
        /// <param name="userId">The ID of the user performing the operation.</param>
        /// <returns>The folder if it exists; otherwise, null.</returns>
        public async Task<Folder?> GetByTitleForEditAsync(string title, Guid? parentFolderId, Guid folderId, Guid userId)
        {
            return await folderRepository
                .GetAllAttached()
                .Where(f => f.UserId == userId && f.ParentFolderId == parentFolderId && f.Id != folderId) 
                .FirstOrDefaultAsync(f => f.Title == title);
        }

       /// <summary>
        /// Checks if a folder with the specified title exists directly in a notebook.
        /// </summary>
        /// <param name="title">The title of the folder to check.</param>
        /// <param name="notebookId">The ID of the notebook.</param>
        /// <param name="userId">The ID of the user performing the operation.</param>
        /// <returns>The folder if it exists; otherwise, null.</returns>
        public async Task<Folder?> GetByTitleInNotebookAsync(string title, Guid notebookId, Guid userId)
        {
            return await folderRepository
                .GetAllAttached()
                .Where(f => f.UserId == userId && f.NotebookId == notebookId && f.ParentFolderId == null) 
                .FirstOrDefaultAsync(f => f.Title == title);
        }

        /// <summary>
        /// Retrieves a folder with the specified title from the children of the folder identified by the given folderId.
        /// </summary>
        /// <param name="title">The title of the folder to search for.</param>
        /// <param name="notebookId">The ID of the notebook containing the folder (not used in this method).</param>
        /// <param name="parentFolderId">The ID of the parent folder (not used in this method).</param>
        /// <param name="folderId">The ID of the folder whose children will be searched.</param>
        /// <returns>
        /// The folder with the specified title from the children of the folder identified by folderId,
        /// or <c>null</c> if no such folder exists.
        /// </returns>

        public async Task<Folder?> GetByTitleAsync(string title, Guid notebookId, Guid? parentFolderId, Guid? folderId)
        {
            Folder? openedFolder = await folderRepository.GetAllAttached()
                .FirstOrDefaultAsync(f => f.Id == folderId);

            if (openedFolder == null)
            {
                return null;
            }

            return await folderRepository
                .GetAllAttached()
                .Where(f => f.ParentFolderId == openedFolder.Id)
                .FirstOrDefaultAsync(f => f.Title == title);
        }

        /// <summary>
        /// Retrieves all folders and notes for a specific notebook or parent folder.
        /// </summary>
        /// <param name="notebookId">The ID of the notebook.</param>
        /// <param name="parentFolderId">The ID of the parent folder.</param>
        /// <param name="folderId">The ID of the folder to retrieve contents from.</param>
        /// <param name="userId">The ID of the user performing the operation.</param>
        /// <returns>A view model containing folders and notes.</returns>
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

        /// <summary>
        /// Prepares a model for adding a new folder.
        /// </summary>
        /// <param name="notebookId">The ID of the notebook.</param>
        /// <param name="parentFolderId">The ID of the parent folder.</param>
        /// <param name="folderId">The ID of the folder being added.</param>
        /// <returns>A view model for creating a folder.</returns>
        public async Task<FolderCreateViewModel> GetAddFolderModelAsync(Guid notebookId, Guid? parentFolderId, Guid? folderId)
        {
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

        /// <summary>
        /// Adds a new folder to the database.
        /// </summary>
        /// <param name="model">The view model containing folder details.</param>
        /// <param name="userId">The ID of the user adding the folder.</param>
        /// <returns>A tuple containing a success flag and an error message if applicable.</returns>
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


        /// <summary>
        /// Prepares a model for adding a subfolder and retrieves the new parent folder ID.
        /// </summary>
        /// <param name="notebookId">The ID of the notebook where the subfolder will be added.</param>
        /// <param name="parentFolderId">The ID of the parent folder, if any.</param>
        /// <param name="folderId">The ID of the folder being edited or accessed.</param>
        /// <param name="userId">The ID of the user performing the operation.</param>
        /// <returns>A tuple containing the folder creation model and the new parent folder ID.</returns>
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

        /// <summary>
        /// Retrieves a folder for deletion by its ID and retrieves the new parent folder ID.
        /// </summary>
        /// <param name="notebookId">The ID of the notebook containing the folder.</param>
        /// <param name="parentFolderId">The ID of the parent folder, if any.</param>
        /// <param name="folderId">The ID of the folder to delete.</param>
        /// <param name="userId">The ID of the user performing the operation.</param>
        /// <returns>A tuple containing the folder delete model and the new parent folder ID.</returns>
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

        /// <summary>
        /// Deletes a folder and all its child folders recursively.
        /// </summary>
        /// <param name="folderId">The ID of the folder to delete.</param>
        /// <param name="userId">The ID of the user performing the operation.</param>
        public async Task DeleteFolderWithChildrenAsync(Guid folderId, Guid userId)
        {
            Folder? folder = await folderRepository
                .GetAllAttached()
                .Include(f => f.Notes)
                .Include(f => f.ChildrenFolders)
                .FirstOrDefaultAsync(f => f.Id == folderId && f.UserId == userId);

            if (folder == null)
            {
                return;
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

        /// <summary>
        /// Retrieves a folder for editing by its ID and retrieves the new parent folder ID.
        /// </summary>
        /// <param name="notebookId">The ID of the notebook containing the folder.</param>
        /// <param name="parentFolderId">The ID of the parent folder, if any.</param>
        /// <param name="folderId">The ID of the folder to edit.</param>
        /// <param name="userId">The ID of the user performing the operation.</param>
        /// <returns>A tuple containing the folder edit model and the new parent folder ID.</returns>
        public async Task<(FolderEditViewModel? model, Guid newParentFolderId)> GetFolderForEditByIdAsync(Guid notebookId, Guid? parentFolderId, Guid folderId, Guid userId)
        {
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

        /// <summary>
        /// Edits the details of a folder.
        /// </summary>
        /// <param name="model">The folder edit view model containing updated details.</param>
        /// <returns>A tuple indicating whether the folder was edited successfully and the parent folder, if any.</returns>
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

        /// <summary>
        /// Retrieves detailed information about a folder by its ID and retrieves the new parent folder ID.
        /// </summary>
        /// <param name="notebookId">The ID of the notebook containing the folder.</param>
        /// <param name="parentFolderId">The ID of the parent folder, if any.</param>
        /// <param name="folderId">The ID of the folder to retrieve details for.</param>
        /// <param name="userId">The ID of the user performing the operation.</param>
        /// <returns>A tuple containing the folder details model and the new parent folder ID.</returns>
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

        /// <summary>
        /// Edits the details of a folder, including its description.
        /// </summary>
        /// <param name="model">The folder details view model containing updated details.</param>
        /// <returns>A tuple indicating whether the folder details were edited successfully and the parent folder, if any.</returns>
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

        /// <summary>
        /// Retrieves the title of a folder by its ID.
        /// </summary>
        /// <param name="folderId">The ID of the folder.</param>
        /// <returns>The title of the folder, or null if not found.</returns>
        public async Task<string?> GetFolderTitleAsync(Guid folderId)
        {
            return await folderRepository
                .GetAllAttached()
                .Where(f => f.Id == folderId)
                .Select(f => f.Title)
                .FirstOrDefaultAsync();
        }

        /// <summary>
        /// Retrieves the title of a notebook by its ID and the user ID.
        /// </summary>
        /// <param name="notebookId">The ID of the notebook.</param>
        /// <param name="userId">The ID of the user performing the operation.</param>
        /// <returns>The title of the notebook, or null if not found.</returns>
        public async Task<string?> GetNotebookTitleAsync(Guid notebookId, Guid userId)
        {
            return await notebookRepository
                .GetAllAttached()
                .Where(nb => nb.Id == notebookId && nb.UserId == userId)
                .Select(nb => nb.Title)
                .FirstOrDefaultAsync();
        }

        /// <summary>
        /// Retrieves the parent folder ID for a given folder.
        /// </summary>
        /// <param name="parentFolderId">The ID of the parent folder, if any.</param>
        /// <param name="newParentFolderId">The current new parent folder ID to update.</param>
        /// <param name="userId">The ID of the user performing the operation.</param>
        /// <returns>The updated parent folder ID.</returns>
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

        /// <summary>
        /// Retrieves the folder and parent folder IDs for a given parent folder ID.
        /// </summary>
        /// <param name="parentFolderId">The ID of the parent folder, if any.</param>
        /// <param name="newParentFolderId">The current new parent folder ID to update.</param>
        /// <param name="newFolderId">The current new folder ID to update.</param>
        /// <param name="userId">The ID of the user performing the operation.</param>
        /// <returns>A tuple containing the updated parent folder ID and folder ID.</returns>
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
                newFolderId = folder.Id; 

            }
            else if (folder == null && parentFolderId != null)
            {
                newFolderId = parentFolderId.Value;
            }

            return (newParentFolderId, newFolderId);
        }

        /// <summary>
        /// Retrieves a folder for redirection based on its ID or parent folder ID.
        /// </summary>
        /// <param name="folderId">The ID of the folder to retrieve.</param>
        /// <param name="parentFolderId">The ID of the parent folder, if any.</param>
        /// <returns>The folder, or null if not found.</returns>
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