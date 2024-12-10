using LunarApp.Data;
using LunarApp.Data.Models;
using LunarApp.Data.Repository;
using LunarApp.Data.Repository.Interfaces;
using LunarApp.Services.Data;
using LunarApp.Services.Data.Interfaces;
using LunarApp.Web.ViewModels.Notebook;
using Microsoft.EntityFrameworkCore;

namespace LunarApp.Services.Tests
{
    [TestFixture]
    public class NotebookServiceTests
    {
        private DbContextOptions<ApplicationDbContext> _dbOptions;
        private ApplicationDbContext _dbContext;
        private IRepository<Notebook, Guid> _notebookRepository;
        private IRepository<Folder, Guid> _folderRepository;
        private IRepository<Note, Guid> _noteRepository;
        private IFolderService _folderService;
        private INotebookService _notebookService;

        [SetUp]
        public void Setup()
        {
            _dbOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: $"TestDb_{Guid.NewGuid()}")
                .Options;

            _dbContext = new ApplicationDbContext(_dbOptions);

            _notebookRepository = new BaseRepository<Notebook, Guid>(_dbContext);
            _folderRepository = new BaseRepository<Folder, Guid>(_dbContext);
            _noteRepository = new BaseRepository<Note, Guid>(_dbContext);

            _folderService = new FolderService(_notebookRepository, _folderRepository, _noteRepository);
            _notebookService = new NotebookService(_notebookRepository, _noteRepository, _folderService);
        }

        [TearDown]
        public void TearDown()
        {
            _dbContext.Database.EnsureDeleted();
            _dbContext.Dispose();
        }

        [Test]
        public async Task GetByTitleAsync_ShouldReturnNotebook()
        {
            var userId = Guid.NewGuid();
            var notebook = new Notebook { Title = "My Notebook", UserId = userId };
            _dbContext.Notebooks.Add(notebook);
            await _dbContext.SaveChangesAsync();

            var result = await _notebookService.GetByTitleAsync("My Notebook", userId);

            Assert.IsNotNull(result);
            Assert.AreEqual("My Notebook", result.Title);
        }

        [Test]
        public async Task GetByTitleAsync_ShouldReturnNull_WhenNotFound()
        {
            var userId = Guid.NewGuid();

            var result = await _notebookService.GetByTitleAsync("Nonexistent", userId);

            Assert.IsNull(result);
        }

        [Test]
        public async Task IndexGetAllOrderedByTitleAsync_ShouldReturnOrderedNotebooks()
        {
            var userId = Guid.NewGuid();
            _dbContext.Notebooks.AddRange(
                new Notebook { Title = "B Notebook", UserId = userId },
                new Notebook { Title = "A Notebook", UserId = userId });
            await _dbContext.SaveChangesAsync();

            var result = await _notebookService.IndexGetAllOrderedByTitleAsync(userId);

            Assert.AreEqual(2, result.Count());
            Assert.AreEqual("A Notebook", result.First().Title);
        }

        [Test]
        public async Task AddNotebookAsync_ShouldAddNotebook()
        {
            var userId = Guid.NewGuid();
            var model = new NotebookCreateViewModel { Title = "Test Notebook" };

            await _notebookService.AddNotebookAsync(model, userId);
            var notebooks = _dbContext.Notebooks.ToList();

            Assert.AreEqual(1, notebooks.Count);
            Assert.AreEqual("Test Notebook", notebooks[0].Title);
            Assert.AreEqual(userId, notebooks[0].UserId);
        }

        [Test]
        public async Task GetNotebookForDeleteByIdAsync_ShouldReturnNotebookDeleteViewModel_WhenNotebookExists()
        {
            var userId = Guid.NewGuid();
            var notebook = new Notebook { Id = Guid.NewGuid(), Title = "Test Notebook", UserId = userId };
            _dbContext.Notebooks.Add(notebook);
            await _dbContext.SaveChangesAsync();

            var result = await _notebookService.GetNotebookForDeleteByIdAsync(notebook.Id, userId);

            Assert.IsNotNull(result);
            Assert.AreEqual(notebook.Id, result?.Id);
            Assert.AreEqual(notebook.Title, result?.Title);
        }

        [Test]
        public async Task GetNotebookForDeleteByIdAsync_ShouldReturnNull_WhenUserIsNotOwner()
        {
            var userId = Guid.NewGuid();
            var otherUserId = Guid.NewGuid();  
            var notebook = new Notebook { Id = Guid.NewGuid(), Title = "Test Notebook", UserId = otherUserId };
            _dbContext.Notebooks.Add(notebook);
            await _dbContext.SaveChangesAsync();

            var result = await _notebookService.GetNotebookForDeleteByIdAsync(notebook.Id, userId);

            Assert.IsNull(result);
        }

        [Test]
        public async Task DeleteNotebookAsync_ShouldDeleteNotebook()
        {
            var userId = Guid.NewGuid();
            var notebook = new Notebook { Id = Guid.NewGuid(), Title = "Notebook", UserId = userId };
            _dbContext.Notebooks.Add(notebook);
            await _dbContext.SaveChangesAsync();

            await _notebookService.DeleteNotebookAsync(notebook.Id, userId);

            Assert.IsFalse(_dbContext.Notebooks.Any());
        }

        [Test]
        public async Task DeleteNotebookAsync_ShouldThrowException_WhenNotebookNotFound()
        {
            var userId = Guid.NewGuid();

            Assert.ThrowsAsync<InvalidOperationException>(async () =>
            {
                await _notebookService.DeleteNotebookAsync(Guid.NewGuid(), userId);
            });
        }

        [Test]
        public async Task DeleteNotebookAsync_ShouldDeleteNotebookAndRelatedFoldersAndNotes()
        {
            var userId = Guid.NewGuid();

            var notebook = new Notebook
            {
                Id = Guid.NewGuid(),
                Title = "Notebook",
                UserId = userId,
                Folders = new List<Folder>
                {
                    new Folder
                    {
                        Id = Guid.NewGuid(),
                        Title = "Folder 1",
                        UserId = userId,
                        Notes = new List<Note>
                        {
                            new Note { Id = Guid.NewGuid(), Title = "Note 1" }
                        }
                    },
                    new Folder
                    {
                        Id = Guid.NewGuid(),
                        Title = "Folder 2",
                        UserId = userId,
                        Notes = new List<Note>
                        {
                            new Note { Id = Guid.NewGuid(), Title = "Note 2" }
                        }
                    }
                },
                Notes = new List<Note>
                {
                    new Note { Id = Guid.NewGuid(), Title = "Notebook Note" }
                }
            };

            _dbContext.Notebooks.Add(notebook);
            await _dbContext.SaveChangesAsync();

            var folderService = new FolderService(
                _notebookRepository,
                _folderRepository,
                _noteRepository
            );

            var notebookService = new NotebookService(
                _notebookRepository,
                _noteRepository,
                folderService
            );

            await notebookService.DeleteNotebookAsync(notebook.Id, userId);

            Assert.IsFalse(_dbContext.Notebooks.Any(), "Notebook was not deleted.");
            Assert.IsFalse(_dbContext.Folders.Any(), "Folders were not deleted.");
            Assert.IsFalse(_dbContext.Notes.Any(), "Notes were not deleted.");
        }

        [Test]
        public async Task EditNotebookAsync_ShouldUpdateNotebookTitle()
        {
            var userId = Guid.NewGuid();
            var notebook = new Notebook { Title = "Old Title", UserId = userId };
            _dbContext.Notebooks.Add(notebook);
            await _dbContext.SaveChangesAsync();

            var model = new NotebookEditViewModel { Id = notebook.Id, Title = "New Title" };

            var result = await _notebookService.EditNotebookAsync(model, userId);

            Assert.IsTrue(result);
            var updatedNotebook = _dbContext.Notebooks.FirstOrDefault(n => n.Id == notebook.Id);
            Assert.AreEqual("New Title", updatedNotebook?.Title);
        }

        [Test]
        public async Task GetNotebookForEditByIdAsync_ShouldReturnNull_WhenNotebookNotFound()
        {
            var userId = Guid.NewGuid();

            var result = await _notebookService.GetNotebookForEditByIdAsync(Guid.NewGuid(), userId);

            Assert.IsNull(result);
        }

        [Test]
        public async Task EditNotebookAsync_ShouldReturnFalse_WhenNotebookNotFound()
        {
            var userId = Guid.NewGuid();
            var model = new NotebookEditViewModel { Id = Guid.NewGuid(), Title = "New Title" };

            var result = await _notebookService.EditNotebookAsync(model, userId);

            Assert.IsFalse(result);
        }

        [Test]
        public async Task GetNotebookDetailsByIdAsync_ShouldReturnNotebookDetails_WhenNotebookExistsAndUserIsOwner()
        {
            var userId = Guid.NewGuid();
            var notebook = new Notebook { Id = Guid.NewGuid(), Title = "Test Notebook", Description = "Test Description", UserId = userId };
            _dbContext.Notebooks.Add(notebook);
            await _dbContext.SaveChangesAsync();

            var result = await _notebookService.GetNotebookDetailsByIdAsync(notebook.Id, userId);

            Assert.IsNotNull(result);
            Assert.AreEqual(notebook.Id, result?.Id);
            Assert.AreEqual(notebook.Title, result?.Title);
            Assert.AreEqual(notebook.Description, result?.Description);
        }

        [Test]
        public async Task EditDetailsNotebookAsync_ShouldUpdateNotebookDescription()
        {
            var userId = Guid.NewGuid();
            var notebook = new Notebook { Title = "Test Notebook", Description = "Old Description", UserId = userId };
            _dbContext.Notebooks.Add(notebook);
            await _dbContext.SaveChangesAsync();

            var model = new NotebookDetailsViewModel { Id = notebook.Id, Title = notebook.Title, Description = "New Description" };

            var result = await _notebookService.EditDetailsNotebookAsync(model, userId);

            Assert.IsTrue(result);
            var updatedNotebook = _dbContext.Notebooks.FirstOrDefault(n => n.Id == notebook.Id);
            Assert.AreEqual("New Description", updatedNotebook?.Description);
        }
    }
}