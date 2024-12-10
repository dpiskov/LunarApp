using LunarApp.Data;
using LunarApp.Data.Models;
using LunarApp.Data.Repository;
using LunarApp.Data.Repository.Interfaces;
using LunarApp.Services.Data;
using LunarApp.Web.ViewModels.Tag;
using Microsoft.EntityFrameworkCore;

namespace LunarApp.Services.Tests
{
    [TestFixture]
    public class TagServiceTests
    {
        private DbContextOptions<ApplicationDbContext> _dbContextOptions;
        private ApplicationDbContext _dbContext;
        private IRepository<Tag, Guid> _tagRepository;
        private TagService _tagService;

        [SetUp]
        public void SetUp()
        {
            _dbContextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase("LunarAppTestDb")
                .Options;

            _dbContext = new ApplicationDbContext(_dbContextOptions);
            _tagRepository = new BaseRepository<Tag, Guid>(_dbContext);
            _tagService = new TagService(_tagRepository);
        }

        [TearDown]
        public void TearDown()
        {
            _dbContext.Database.EnsureDeleted();
            _dbContext.Dispose();
        }

        [Test]
        public async Task GetByTitleAsync_ShouldReturnTag_WhenTagExists()
        {
            var userId = Guid.NewGuid();
            var tag = new Tag { Name = "TestTag", UserId = userId };
            await _tagRepository.AddAsync(tag);
            await _dbContext.SaveChangesAsync();

            var result = await _tagService.GetByTitleAsync("TestTag", userId);

            Assert.IsNotNull(result);
            Assert.AreEqual("TestTag", result.Name);
        }

        [Test]
        public async Task GetByTitleAsync_ShouldReturnNull_WhenTagDoesNotExist()
        {
            var userId = Guid.NewGuid();

            var result = await _tagService.GetByTitleAsync("NonExistentTag", userId);

            Assert.IsNull(result);
        }

        [Test]
        public async Task IndexGetAllTagsOrderedByNameAsync_ShouldReturnOrderedTags()
        {
            var userId = Guid.NewGuid();
            var tag1 = new Tag { Name = "ZTag", UserId = userId };
            var tag2 = new Tag { Name = "ATag", UserId = userId };
            await _tagRepository.AddAsync(tag1);
            await _tagRepository.AddAsync(tag2);
            await _dbContext.SaveChangesAsync();

            var result = await _tagService.IndexGetAllTagsOrderedByNameAsync(userId);

            Assert.AreEqual(2, result.Count());
            Assert.AreEqual("ATag", result.First().Name);
            Assert.AreEqual("ZTag", result.Last().Name);
        }

        [Test]
        public async Task CreateTagAsync_ShouldCreateTagSuccessfully()
        {
            var userId = Guid.NewGuid();
            var model = new TagCreateViewModel { Name = "NewTag" };

            await _tagService.CreateTagAsync(model, userId);
            var tag = await _dbContext.Tags.FirstOrDefaultAsync(t => t.Name == "NewTag");

            Assert.IsNotNull(tag);
            Assert.AreEqual("NewTag", tag.Name);
        }

        [Test]
        public async Task GetTagForEditByIdAsync_ShouldReturnTag_WhenTagExists()
        {
            var userId = Guid.NewGuid();
            var tag = new Tag { Name = "TagToEdit", UserId = userId };
            await _tagRepository.AddAsync(tag);
            await _dbContext.SaveChangesAsync();

            var result = await _tagService.GetTagForEditByIdAsync(null, null, null, null, tag.Id, userId);

            Assert.IsNotNull(result);
            Assert.AreEqual("TagToEdit", result.Name);
        }

        [Test]
        public async Task GetTagForEditByIdAsync_ShouldReturnNull_WhenTagDoesNotExist()
        {
            var userId = Guid.NewGuid();

            var result = await _tagService.GetTagForEditByIdAsync(null, null, null, null, Guid.NewGuid(), userId);

            Assert.IsNull(result);
        }

        [Test]
        public async Task EditTagAsync_ShouldEditTagSuccessfully()
        {
            var userId = Guid.NewGuid();
            var tag = new Tag { Name = "OldTag", UserId = userId };
            await _tagRepository.AddAsync(tag);
            await _dbContext.SaveChangesAsync();

            var model = new TagEditViewModel { Id = tag.Id, Name = "UpdatedTag" };

            var result = await _tagService.EditTagAsync(model);

            Assert.IsTrue(result);
            var updatedTag = await _dbContext.Tags.FindAsync(tag.Id);
            Assert.AreEqual("UpdatedTag", updatedTag?.Name);
        }

        [Test]
        public async Task EditTagAsync_ShouldReturnFalse_WhenTagDoesNotExist()
        {
            var model = new TagEditViewModel { Id = Guid.NewGuid(), Name = "NonExistingTag" };

            var result = await _tagService.EditTagAsync(model);

            Assert.IsFalse(result);
        }

        [Test]
        public async Task DeleteTagAsync_ShouldDeleteTagSuccessfully()
        {
            var userId = Guid.NewGuid();
            var tag = new Tag { Name = "TagToDelete", UserId = userId };
            await _tagRepository.AddAsync(tag);
            await _dbContext.SaveChangesAsync();

            await _tagService.DeleteTagAsync(tag.Id);
            var deletedTag = await _dbContext.Tags.FindAsync(tag.Id);

            Assert.IsNull(deletedTag);
        }

        [Test]
        public async Task DeleteTagAsync_ShouldNotDeleteNonExistingTag()
        {
            var tagId = Guid.NewGuid();

            await _tagService.DeleteTagAsync(tagId);

            var tag = await _dbContext.Tags.FindAsync(tagId);
            Assert.IsNull(tag);
        }
    }
}
