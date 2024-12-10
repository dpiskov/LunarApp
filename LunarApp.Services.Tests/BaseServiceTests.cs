using LunarApp.Data;
using LunarApp.Data.Models;
using LunarApp.Data.Repository;
using LunarApp.Data.Repository.Interfaces;
using LunarApp.Services.Data;
using LunarApp.Services.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LunarApp.Services.Tests
{
    [TestFixture]
    public class BaseServiceTests
    {
        private DbContextOptions<ApplicationDbContext> _options;
        private ApplicationDbContext _context;
        private IRepository<Tag, Guid> _tagRepository;
        private IRepository<Note, Guid> _noteRepository;
        private IBaseService _baseService;

        [SetUp]
        public void SetUp()
        {
            _options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            _context = new ApplicationDbContext(_options);
            _tagRepository = new BaseRepository<Tag, Guid>(_context);
            _noteRepository = new BaseRepository<Note, Guid>(_context);
            _baseService = new BaseService(_tagRepository, _noteRepository);

            SeedDatabase();
        }

        private void SeedDatabase()
        {
            var tag1 = new Tag { Id = Guid.NewGuid(), Name = "Work", UserId = Guid.NewGuid() };
            var tag2 = new Tag { Id = Guid.NewGuid(), Name = "Personal", UserId = tag1.UserId };

            _context.Tags.Add(tag1);
            _context.Tags.Add(tag2);

            var note1 = new Note
            {
                Id = Guid.NewGuid(),
                Title = "Work Note 1",
                NotebookId = Guid.NewGuid(),
                TagId = tag1.Id,
                UserId = tag1.UserId
            };
            var note2 = new Note
            {
                Id = Guid.NewGuid(),
                Title = "Personal Note 1",
                NotebookId = note1.NotebookId,
                TagId = tag2.Id,
                UserId = tag1.UserId
            };

            _context.Notes.Add(note1);
            _context.Notes.Add(note2);
            _context.SaveChanges();
        }

        [Test]
        public async Task GetFilteredNotesAsyncByNotebookId_ShouldReturnFilteredNotes()
        {
            var notebookId = Guid.NewGuid();
            var workTag = _context.Tags.FirstOrDefault(t => t.Name == "Work");
            if (workTag == null)
            {
                Assert.Fail("The 'Work' tag was not found in the database.");
            }

            var workNote = new Note
            {
                Title = "Work Note 1",
                NotebookId = notebookId,
                TagId = workTag.Id,
                UserId = workTag.UserId
            };
            _context.Notes.Add(workNote);
            await _context.SaveChangesAsync();

            Guid? parentFolderId = null;
            Guid? folderId = null;
            var searchQuery = "Work";
            var tagFilter = "Work";

            var result = await _baseService.GetFilteredNotesAsyncByNotebookId(notebookId, parentFolderId, folderId, workTag.UserId, searchQuery, tagFilter);

            Assert.NotNull(result);
            var notesList = result.Notes.ToList();
            Assert.AreEqual(1, notesList.Count);
            Assert.AreEqual("Work Note 1", notesList[0].Title);
        }

        [Test]
        public async Task GetFilteredNotesAsyncByNotebookId_ShouldReturnEmptyListWhenNoMatches()
        {
            var result = await _baseService.GetFilteredNotesAsyncByNotebookId(Guid.NewGuid(), null, null, Guid.NewGuid(), "Nonexistent", "Work");

            Assert.NotNull(result);
            var notesList = result.Notes.ToList();
            Assert.AreEqual(0, notesList.Count);
        }

        [Test]
        public async Task GetFilteredNotesAsync_ShouldReturnFilteredNotes()
        {
            var userId = _context.Tags.FirstOrDefault(t => t.Name == "Personal")?.UserId;

            if (userId == null)
            {
                Assert.Fail("No 'Personal' tag found in the database.");
            }

            var result = await _baseService.GetFilteredNotesAsync(userId.Value, "Personal", "Personal");

            Assert.NotNull(result);
            var notesList = result.Notes.ToList();
            Assert.AreEqual(1, notesList.Count);
            Assert.AreEqual("Personal Note 1", notesList[0].Title);
        }

        [Test]
        public async Task GetFilteredNotesAsync_ShouldReturnEmptyListWhenNoMatches()
        {
            var result = await _baseService.GetFilteredNotesAsync(Guid.NewGuid(), "Nonexistent", "Nonexistent");

            Assert.NotNull(result);
            var notesList = result.Notes.ToList();
            Assert.AreEqual(0, notesList.Count);
        }

        [Test]
        public async Task GetAllTagsAsync_ShouldReturnAllTags()
        {
            var userId = _context.Tags.FirstOrDefault(t => t.Name == "Work")?.UserId ?? Guid.NewGuid();

            var result = await _baseService.GetAllTagsAsync(userId);

            Assert.NotNull(result);
            Assert.AreEqual(2, result.Count());
            Assert.Contains("Work", result.ToList());
            Assert.Contains("Personal", result.ToList());
        }


        [TearDown]
        public void TearDown()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }
    }
}
