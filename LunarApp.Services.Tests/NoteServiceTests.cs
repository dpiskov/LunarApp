using LunarApp.Data;
using LunarApp.Data.Models;
using LunarApp.Data.Repository;
using LunarApp.Data.Repository.Interfaces;
using LunarApp.Services.Data;
using LunarApp.Web.ViewModels.Note;
using Microsoft.EntityFrameworkCore;

namespace LunarApp.Services.Tests
{
    [TestFixture]
    public class NoteServiceTests
    {
        private ApplicationDbContext _dbContext;
        private NoteService _noteService;
        private IRepository<Notebook, Guid> _notebookRepository;
        private IRepository<Folder, Guid> _folderRepository;
        private IRepository<Note, Guid> _noteRepository;
        private IRepository<Tag, Guid> _tagRepository;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            _dbContext = new ApplicationDbContext(options);
            _notebookRepository = new BaseRepository<Notebook, Guid>(_dbContext);
            _folderRepository = new BaseRepository<Folder, Guid>(_dbContext);
            _noteRepository = new BaseRepository<Note, Guid>(_dbContext);
            _tagRepository = new BaseRepository<Tag, Guid>(_dbContext);
            _noteService = new NoteService(_notebookRepository, _folderRepository, _noteRepository, _tagRepository);

            SeedData();
        }

        private void SeedData()
        {
            var notebook = new Notebook { Id = Guid.NewGuid(), Title = "Test Notebook" };
            _dbContext.Notebooks.Add(notebook);
            _dbContext.SaveChanges();

            var folder = new Folder { Id = Guid.NewGuid(), Title = "Test Folder", NotebookId = notebook.Id };
            _dbContext.Folders.Add(folder);
            _dbContext.SaveChanges();

            var tag = new Tag { Id = Guid.NewGuid(), Name = "Test Tag", UserId = Guid.NewGuid() };
            _dbContext.Tags.Add(tag);
            _dbContext.SaveChanges();

            var note = new Note
            {
                Id = Guid.NewGuid(),
                Title = "Test Note",
                Body = "This is a test note.",
                NotebookId = notebook.Id,
                FolderId = folder.Id,
                TagId = tag.Id,
                UserId = Guid.NewGuid(),
                DateCreated = DateTime.Now,
                LastSaved = DateTime.Now
            };
            _dbContext.Notes.Add(note);
            _dbContext.SaveChanges();
        }

        [Test]
        public async Task GetCreateNoteAsync_ShouldReturnValidViewModel()
        {
            var notebookId = Guid.NewGuid();
            var parentFolderId = Guid.NewGuid();
            var folderId = Guid.NewGuid();
            var userId = Guid.NewGuid();

            var result = await _noteService.GetCreateNoteAsync(notebookId, parentFolderId, folderId, userId);

            Assert.IsNotNull(result);
            Assert.AreEqual(notebookId, result.NotebookId);
            Assert.IsNotNull(result.Tags);
            Assert.IsEmpty(result.Title);
        }

        [Test]
        public async Task CreateNoteAsync_ShouldCreateNoteSuccessfully()
        {
            var notebook = _dbContext.Notebooks.FirstOrDefault();
            var folder = _dbContext.Folders.FirstOrDefault();
            var tag = _dbContext.Tags.FirstOrDefault();

            Assert.IsNotNull(notebook);
            Assert.IsNotNull(folder);
            Assert.IsNotNull(tag);

            var model = new NoteCreateViewModel
            {
                NotebookId = notebook.Id,
                Title = "New Note",
                Body = "This is a new note body.",
                SelectedTagId = tag.Id,
                DateCreated = DateTime.Now,
                LastSaved = DateTime.Now
            };

            var userId = Guid.NewGuid();

            var result = await _noteService.CreateNoteAsync(model, userId);

            Assert.IsTrue(result.isSuccess);
            Assert.IsNull(result.errorMessage);

            var createdNote = await _dbContext.Notes
                                              .Where(n => n.Title == model.Title && n.UserId == userId)
                                              .FirstOrDefaultAsync();

            Assert.IsNotNull(createdNote);
            Assert.AreEqual(model.Title, createdNote.Title);
            Assert.AreEqual(model.Body, createdNote.Body);
            Assert.AreEqual(model.NotebookId, createdNote.NotebookId);
            Assert.AreEqual(model.SelectedTagId, createdNote.TagId);
        }

        [Test]
        public async Task GetNoteForDeleteByIdAsync_ShouldReturnNoteForDelete()
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync();

            if (user == null)
            {
                user = new ApplicationUser
                {
                    UserName = "testuser",
                    Email = "testuser@example.com",
                };
                _dbContext.Users.Add(user);
                await _dbContext.SaveChangesAsync();
            }

            var userId = user.Id;

            var notebook = await _dbContext.Notebooks.FirstOrDefaultAsync(n => n.UserId == userId);
            if (notebook == null)
            {
                notebook = new Notebook
                {
                    Id = Guid.NewGuid(),
                    Title = "Test Notebook",
                    UserId = userId
                };
                _dbContext.Notebooks.Add(notebook);
                await _dbContext.SaveChangesAsync();
            }

            var folder = await _dbContext.Folders.FirstOrDefaultAsync(f => f.NotebookId == notebook.Id);
            if (folder == null)
            {
                folder = new Folder
                {
                    Id = Guid.NewGuid(),
                    Title = "Test Folder",
                    NotebookId = notebook.Id
                };
                _dbContext.Folders.Add(folder);
                await _dbContext.SaveChangesAsync();
            }

            var note = await _dbContext.Notes.FirstOrDefaultAsync(n => n.UserId == userId);
            if (note == null)
            {
                note = new Note
                {
                    Id = Guid.NewGuid(),
                    Title = "Test Note",
                    Body = "Test note body",
                    NotebookId = notebook.Id,
                    FolderId = folder.Id,
                    UserId = userId,
                    DateCreated = DateTime.Now,
                    LastSaved = DateTime.Now
                };
                _dbContext.Notes.Add(note);
                await _dbContext.SaveChangesAsync();
            }

            var result = await _noteService.GetNoteForDeleteByIdAsync(notebook.Id, null, folder.Id, note.Id, userId);

            Assert.IsNotNull(result);
            Assert.AreEqual(note.Id, result.Id);
            Assert.AreEqual(note.Title, result.Title);
        }

        [Test]
        public async Task DeleteNoteAsync_ShouldDeleteNoteSuccessfully()
        {
            var noteId = Guid.NewGuid();
            var userId = Guid.NewGuid();

            await _noteService.DeleteNoteAsync(noteId, userId);

            var deletedNote = await _dbContext.Notes.FindAsync(noteId);
            Assert.IsNull(deletedNote);
        }

        [Test]
        public async Task GetNoteForEditByIdAsync_ShouldReturnNoteForEdit()
        {
            var notebook = _dbContext.Notebooks.FirstOrDefault();
            var folder = _dbContext.Folders.FirstOrDefault();
            var note = _dbContext.Notes.FirstOrDefault();

            Assert.IsNotNull(notebook);
            Assert.IsNotNull(folder);
            Assert.IsNotNull(note);

            var userId = note.UserId;

            var result = await _noteService.GetNoteForEditByIdAsync(notebook.Id, null, folder.Id, note.Id, userId);

            Assert.IsNotNull(result);
            Assert.AreEqual(note.Id, result.Id);
            Assert.AreEqual(note.Title, result.Title);
            Assert.AreEqual(note.Body, result.Body);
            Assert.AreEqual(note.TagId, result.SelectedTagId);
            Assert.AreEqual(notebook.Id, result.NotebookId);
            Assert.AreEqual(folder.Id, result.FolderId);
        }

        [Test]
        public async Task EditNoteAsync_ShouldUpdateNoteSuccessfully()
        {
            var note = _dbContext.Notes.FirstOrDefault();

            Assert.IsNotNull(note);

            var model = new NoteEditViewModel
            {
                Id = note.Id,
                Title = "Updated Note",
                Body = "This is an updated note.",
                SelectedTagId = note.TagId,
                DateCreated = note.DateCreated,
            };

            var userId = note.UserId;

            var result = await _noteService.EditNoteAsync(model, userId);

            Assert.IsTrue(result);

            var updatedNote = await _dbContext.Notes
                .Where(n => n.Id == note.Id && n.UserId == userId)
                .FirstOrDefaultAsync();

            Assert.IsNotNull(updatedNote);
            Assert.AreEqual(model.Title, updatedNote.Title);
            Assert.AreEqual(model.Body, updatedNote.Body);
            Assert.AreEqual(model.SelectedTagId, updatedNote.TagId);

            Assert.AreEqual(note.LastSaved, updatedNote.LastSaved);
        }

        [TearDown]
        public void TearDown()
        {
            _dbContext.Dispose();
        }
    }
}
