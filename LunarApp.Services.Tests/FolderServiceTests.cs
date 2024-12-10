using LunarApp.Data;
using LunarApp.Data.Models;
using LunarApp.Data.Repository;
using LunarApp.Data.Repository.Interfaces;
using LunarApp.Services.Data;
using LunarApp.Services.Data.Interfaces;
using LunarApp.Web.ViewModels.Folder;
using Microsoft.EntityFrameworkCore;

namespace LunarApp.Services.Tests
{
    [TestFixture]
    public class FolderServiceTests
    {
        private ApplicationDbContext _dbContext;
        private IRepository<Notebook, Guid> _notebookRepository;
        private IRepository<Folder, Guid> _folderRepository;
        private IRepository<Note, Guid> _noteRepository;
        private IFolderService _folderService;

        [SetUp]
        public void SetUp()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            _dbContext = new ApplicationDbContext(options);

            _notebookRepository = new BaseRepository<Notebook, Guid>(_dbContext);
            _folderRepository = new BaseRepository<Folder, Guid>(_dbContext);
            _noteRepository = new BaseRepository<Note, Guid>(_dbContext);

            _folderService = new FolderService(_notebookRepository, _folderRepository, _noteRepository);

            SeedDatabase();
        }

        private void SeedDatabase()
        {
            var testUserId = Guid.NewGuid();

            var notebook = new Notebook
            {
                Id = Guid.NewGuid(),
                Title = "Test Notebook"
            };
            _dbContext.Notebooks.Add(notebook);

            var parentFolder = new Folder
            {
                Id = Guid.NewGuid(),
                Title = "Parent Folder",
                NotebookId = notebook.Id,
                UserId = testUserId
            };
            _dbContext.Folders.Add(parentFolder);

            var childFolder = new Folder
            {
                Id = Guid.NewGuid(),
                Title = "Child Folder",
                NotebookId = notebook.Id,
                ParentFolderId = parentFolder.Id,
                UserId = testUserId
            };
            _dbContext.Folders.Add(childFolder);

            var note = new Note
            {
                Id = Guid.NewGuid(),
                Title = "Test Note",
                NotebookId = notebook.Id,
                FolderId = childFolder.Id,
                UserId = testUserId
            };
            _dbContext.Notes.Add(note);

            _dbContext.SaveChanges();
        }

        [TearDown]
        public void TearDown()
        {
            _dbContext.Dispose();
        }

        [Test]
        public async Task GetByTitleForEditAsync_ShouldReturnNull_WhenFolderDoesNotExist()
        {
            var result = await _folderService.GetByTitleForEditAsync("Nonexistent", null, Guid.NewGuid(), Guid.NewGuid());
            Assert.IsNull(result);
        }

        [Test]
        public async Task GetByTitleInNotebookAsync_ShouldReturnFolder_WhenFolderExists()
        {
            var notebookId = _dbContext.Notebooks.First().Id;
            var userId = _dbContext.Folders.First(f => f.Title == "Parent Folder").UserId;
            var result = await _folderService.GetByTitleInNotebookAsync("Parent Folder", notebookId, userId);
            Assert.NotNull(result);
            Assert.AreEqual("Parent Folder", result.Title);
        }

        [Test]
        public async Task GetByTitleAsync_ShouldReturnFolder_WhenFolderWithTitleExists()
        {
            var userId = Guid.NewGuid();
            var notebookId = Guid.NewGuid();
            var parentFolder = new Folder
            {
                Id = Guid.NewGuid(),
                Title = "Parent Folder",
                NotebookId = notebookId,
                UserId = userId
            };
            var childFolder = new Folder
            {
                Id = Guid.NewGuid(),
                Title = "Child Folder",
                ParentFolderId = parentFolder.Id,
                NotebookId = notebookId,
                UserId = userId
            };
            _dbContext.Folders.AddRange(parentFolder, childFolder);
            await _dbContext.SaveChangesAsync();
            var result = await _folderService.GetByTitleAsync("Child Folder", notebookId, parentFolder.Id, parentFolder.Id);
            Assert.NotNull(result);
            Assert.AreEqual("Child Folder", result?.Title);
        }

        [Test]
        public async Task GetByTitleAsync_ShouldReturnNull_WhenFolderWithTitleDoesNotExist()
        {
            var userId = Guid.NewGuid();
            var notebookId = Guid.NewGuid();
            var parentFolder = new Folder
            {
                Id = Guid.NewGuid(),
                Title = "Parent Folder",
                NotebookId = notebookId,
                UserId = userId
            };
            var childFolder = new Folder
            {
                Id = Guid.NewGuid(),
                Title = "Child Folder",
                ParentFolderId = parentFolder.Id,
                NotebookId = notebookId,
                UserId = userId
            };
            _dbContext.Folders.AddRange(parentFolder, childFolder);
            await _dbContext.SaveChangesAsync();
            var result = await _folderService.GetByTitleAsync("Nonexistent Folder", notebookId, parentFolder.Id, childFolder.Id);
            Assert.IsNull(result);
        }

        [Test]
        public async Task GetByTitleAsync_ShouldReturnNull_WhenDeepestFolderNotFound()
        {
            var userId = Guid.NewGuid();
            var notebookId = Guid.NewGuid();
            var parentFolder = new Folder
            {
                Id = Guid.NewGuid(),
                Title = "Parent Folder",
                NotebookId = notebookId,
                UserId = userId
            };
            _dbContext.Folders.Add(parentFolder);
            await _dbContext.SaveChangesAsync();
            var result = await _folderService.GetByTitleAsync("Nonexistent Folder", notebookId, parentFolder.Id, Guid.NewGuid());
            Assert.IsNull(result);
        }

        [Test]
        public async Task IndexGetAllFoldersAsync_ShouldReturnFoldersAndNotes()
        {
            var notebookId = _dbContext.Notebooks.First().Id;
            var folderId = _dbContext.Folders.First(f => f.Title == "Child Folder").Id;
            var userId = _dbContext.Folders.First().UserId;
            var result = await _folderService.IndexGetAllFoldersAsync(notebookId, null, folderId, userId);
            Assert.NotNull(result);
            Assert.AreEqual(0, result.Folders.Count());
            Assert.AreEqual(1, result.Notes.Count());
        }

        [Test]
        public async Task GetAddFolderModelAsync_ShouldReturnCorrectFlag_WhenMadeDirectlyFromNotebook()
        {
            var notebookId = _dbContext.Notebooks.First().Id;
            var result = await _folderService.GetAddFolderModelAsync(notebookId, null, null);
            Assert.NotNull(result);
            Assert.IsTrue(result.IsMadeDirectlyFromNotebook);
        }

        [Test]
        public async Task AddFolderAsync_ShouldReturnError_WhenNotebookDoesNotExist()
        {
            var model = new FolderCreateViewModel
            {
                Title = "New Folder",
                NotebookId = Guid.NewGuid()
            };
            var (isSuccess, errorMessage) = await _folderService.AddFolderAsync(model, Guid.NewGuid());
            Assert.IsFalse(isSuccess);
            Assert.AreEqual("The selected notebook does not exist.", errorMessage);
        }

        [Test]
        public async Task GetAddSubfolderModelAsync_ShouldReturnCorrectModel_WhenCreatingDirectlyFromNotebook()
        {
            var userId = Guid.NewGuid();
            var notebookId = Guid.NewGuid();
            var parentFolderId = Guid.Empty;
            var folderId = Guid.NewGuid();
            var (model, newParentFolderId) = await _folderService.GetAddSubfolderModelAsync(notebookId, parentFolderId, folderId, userId);
            Assert.NotNull(model);
            Assert.AreEqual(notebookId, model.NotebookId);
            Assert.AreEqual(parentFolderId, model.ParentFolderId);
            Assert.AreEqual(folderId, model.FolderId);
            Assert.IsTrue(model.IsMadeDirectlyFromNotebook);
            Assert.AreEqual(Guid.Empty, newParentFolderId);
        }

        [Test]
        public async Task GetFolderForDeleteByIdAsync_ShouldReturnFolderDeleteViewModel_WhenFolderExistsAndParentFolderIsEmpty()
        {
            var userId = Guid.NewGuid();
            var notebookId = Guid.NewGuid();
            var parentFolder = new Folder
            {
                Id = Guid.NewGuid(),
                Title = "Parent Folder",
                NotebookId = notebookId,
                UserId = userId
            };
            var folderToDelete = new Folder
            {
                Id = Guid.NewGuid(),
                Title = "Child Folder",
                ParentFolderId = parentFolder.Id,
                NotebookId = notebookId,
                UserId = userId
            };
            _dbContext.Folders.AddRange(parentFolder, folderToDelete);
            await _dbContext.SaveChangesAsync();
            var (model, newParentFolderId) = await _folderService.GetFolderForDeleteByIdAsync(notebookId, Guid.Empty, folderToDelete.Id, userId);
            Assert.NotNull(model);
            Assert.AreEqual("Child Folder", model?.Title);
            Assert.AreEqual(Guid.Empty, newParentFolderId);
        }

        [Test]
        public async Task DeleteFolderWithChildrenAsync_ShouldRemoveAllNestedFolders()
        {
            var testUserId = _dbContext.Folders.First().UserId;
            var parentFolderId = _dbContext.Folders.First(f => f.Title == "Parent Folder").Id;
            await _folderService.DeleteFolderWithChildrenAsync(parentFolderId, testUserId);
            Assert.AreEqual(0, _dbContext.Folders.Count());
            Assert.AreEqual(0, _dbContext.Notes.Count());
        }

        [Test]
        public async Task GetFolderForEditByIdAsync_ShouldReturnNull_WhenFolderDoesNotExist()
        {
            var userId = Guid.NewGuid();
            var notebookId = Guid.NewGuid();
            var folderId = Guid.NewGuid();
            var (model, newParentFolderId) = await _folderService.GetFolderForEditByIdAsync(notebookId, Guid.NewGuid(), folderId, userId);
            Assert.IsNull(model);
            Assert.AreEqual(Guid.Empty, newParentFolderId);
        }

        [Test]
        public async Task EditFolderAsync_ShouldUpdateFolderTitle_WhenValidModelProvided()
        {
            var folder = _dbContext.Folders.First(f => f.Title == "Parent Folder");
            var model = new FolderEditViewModel
            {
                FolderId = folder.Id,
                Title = "Updated Folder Title",
                NotebookId = folder.NotebookId
            };
            var (isEdited, parentFolder) = await _folderService.EditFolderAsync(model);
            Assert.IsTrue(isEdited);
            Assert.AreEqual("Updated Folder Title", folder.Title);
        }

        [Test]
        public async Task GetFolderDetailsByIdAsync_ShouldReturnNull_WhenFolderDoesNotExist()
        {
            var userId = Guid.NewGuid();
            var notebookId = Guid.NewGuid();
            var folderId = Guid.NewGuid();
            var (model, newParentFolderId) = await _folderService.GetFolderDetailsByIdAsync(notebookId, Guid.NewGuid(), folderId, userId);
            Assert.IsNull(model);
            Assert.AreEqual(Guid.Empty, newParentFolderId);
        }

        [Test]
        public async Task EditDetailsFolderAsync_ShouldReturnFalse_WhenModelIsNull()
        {
            var (isEdited, parentFolder) = await _folderService.EditDetailsFolderAsync(null);
            Assert.IsFalse(isEdited);
            Assert.IsNull(parentFolder);
        }

        [Test]
        public async Task EditDetailsFolderAsync_ShouldReturnFalse_WhenFolderDoesNotExist()
        {
            var model = new FolderDetailsViewModel
            {
                FolderId = Guid.NewGuid(),
                Title = "Test Folder",
                Description = "Updated description"
            };
            var (isEdited, parentFolder) = await _folderService.EditDetailsFolderAsync(model);
            Assert.IsFalse(isEdited);
            Assert.IsNull(parentFolder);
        }

        [Test]
        public async Task GetFolderTitleAsync_ShouldReturnTitle_WhenFolderExists()
        {
            var folderId = _dbContext.Folders.First(f => f.Title == "Parent Folder").Id;
            var result = await _folderService.GetFolderTitleAsync(folderId);
            Assert.AreEqual("Parent Folder", result);
        }

        [Test]
        public async Task GetNotebookTitleAsync_ShouldReturnTitle_WhenNotebookExists()
        {
            var notebook = _dbContext.Notebooks.First();
            var notebookId = notebook.Id;
            var userId = Guid.NewGuid();
            notebook.UserId = userId;
            await _dbContext.SaveChangesAsync();
            var result = await _folderService.GetNotebookTitleAsync(notebookId, userId);
            Assert.NotNull(result);
            Assert.AreEqual(notebook.Title, result);
        }

        [Test]
        public async Task GetNotebookTitleAsync_ShouldReturnNull_WhenNotebookDoesNotExist()
        {
            var notebookId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var result = await _folderService.GetNotebookTitleAsync(notebookId, userId);
            Assert.IsNull(result);
        }

        [Test]
        public async Task GetParentFolderIdAsync_ShouldReturnParentFolderId_WhenParentExists()
        {
            var userId = Guid.NewGuid();
            var parentFolder = new Folder
            {
                Id = Guid.NewGuid(),
                Title = "Parent Folder",
                UserId = userId
            };
            var childFolder = new Folder
            {
                Id = Guid.NewGuid(),
                Title = "Child Folder",
                ParentFolderId = parentFolder.Id,
                UserId = userId
            };
            _dbContext.Folders.AddRange(parentFolder, childFolder);
            await _dbContext.SaveChangesAsync();
            var result = await _folderService.GetParentFolderIdAsync(childFolder.Id, Guid.Empty, userId);
            Assert.AreEqual(parentFolder.Id, result);
        }

        [Test]
        public async Task GetParentFolderIdAsync_ShouldReturnEmptyGuid_WhenParentDoesNotExist()
        {
            var userId = Guid.NewGuid();
            var folderId = Guid.NewGuid();
            var result = await _folderService.GetParentFolderIdAsync(folderId, Guid.Empty, userId);
            Assert.AreEqual(Guid.Empty, result);
        }

        [Test]
        public async Task GetFolderAndParentIdsAsync_ShouldReturnParentAndFolderIds_WhenParentExists()
        {
            var userId = Guid.NewGuid();
            var parentFolder = new Folder
            {
                Id = Guid.NewGuid(),
                Title = "Parent Folder",
                UserId = userId
            };
            var childFolder = new Folder
            {
                Id = Guid.NewGuid(),
                Title = "Child Folder",
                ParentFolderId = parentFolder.Id,
                UserId = userId
            };
            _dbContext.Folders.AddRange(parentFolder, childFolder);
            await _dbContext.SaveChangesAsync();
            var (newParentFolderId, newFolderId) = await _folderService.GetFolderAndParentIdsAsync(childFolder.Id, Guid.Empty, Guid.Empty, userId);
            Assert.AreEqual(parentFolder.Id, newParentFolderId);
            Assert.AreEqual(childFolder.Id, newFolderId);
        }

        [Test]
        public async Task GetFolderAndParentIdsAsync_ShouldReturnOriginalFolderId_WhenParentDoesNotExist()
        {
            var userId = Guid.NewGuid();
            var folderId = Guid.NewGuid();
            var (newParentFolderId, newFolderId) = await _folderService.GetFolderAndParentIdsAsync(folderId, Guid.Empty, Guid.Empty, userId);
            Assert.AreEqual(Guid.Empty, newParentFolderId);
            Assert.AreEqual(folderId, newFolderId);
        }

        [Test]
        public async Task GetFolderForRedirectionAsync_ShouldReturnFolder_WhenFolderExists()
        {
            var folder = new Folder
            {
                Id = Guid.NewGuid(),
                Title = "Test Folder",
                NotebookId = Guid.NewGuid(),
                ParentFolderId = null
            };
            _dbContext.Folders.Add(folder);
            await _dbContext.SaveChangesAsync();
            var result = await _folderService.GetFolderForRedirectionAsync(folder.Id, null);
            Assert.NotNull(result);
            Assert.AreEqual(folder.Id, result.Id);
        }

        [Test]
        public async Task GetFolderForRedirectionAsync_ShouldReturnParentFolder_WhenFolderDoesNotExist()
        {
            var parentFolder = new Folder
            {
                Id = Guid.NewGuid(),
                Title = "Parent Folder",
                NotebookId = Guid.NewGuid()
            };
            _dbContext.Folders.Add(parentFolder);
            await _dbContext.SaveChangesAsync();
            var result = await _folderService.GetFolderForRedirectionAsync(Guid.NewGuid(), parentFolder.Id);
            Assert.NotNull(result);
            Assert.AreEqual(parentFolder.Id, result.Id);
        }

        [Test]
        public async Task GetFolderForRedirectionAsync_ShouldReturnNull_WhenNeitherFolderNorParentExists()
        {
            var result = await _folderService.GetFolderForRedirectionAsync(Guid.NewGuid(), Guid.NewGuid());
            Assert.IsNull(result);
        }
    }
}
