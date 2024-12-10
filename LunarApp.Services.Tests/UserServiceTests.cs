using LunarApp.Data;
using LunarApp.Data.Models;
using LunarApp.Data.Repository;
using LunarApp.Data.Repository.Interfaces;
using LunarApp.Services.Data;
using LunarApp.Services.Data.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace LunarApp.Services.Tests
{
    public class UserServiceTests
    {
        private DbContextOptions<ApplicationDbContext> _dbContextOptions;
        private ApplicationDbContext _dbContext;
        private UserManager<ApplicationUser> _userManager;
        private RoleManager<IdentityRole<Guid>> _roleManager;
        private IRepository<Notebook, Guid> _notebookRepository;
        private IRepository<Folder, Guid> _folderRepository;
        private IRepository<Note, Guid> _noteRepository;
        private IRepository<Tag, Guid> _tagRepository;
        private IUserService _userService;

        [SetUp]
        public async Task SetUp()
        {
            _dbContextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "LunarAppTestDb")
                .Options;

            _dbContext = new ApplicationDbContext(_dbContextOptions);

            var userStore = new UserStore<ApplicationUser, IdentityRole<Guid>, ApplicationDbContext, Guid>(_dbContext);
            var roleStore = new RoleStore<IdentityRole<Guid>, ApplicationDbContext, Guid>(_dbContext);

            _userManager = new UserManager<ApplicationUser>(userStore, null, new PasswordHasher<ApplicationUser>(),
                new IUserValidator<ApplicationUser>[0], new IPasswordValidator<ApplicationUser>[0],
                new UpperInvariantLookupNormalizer(), new IdentityErrorDescriber(), null, null);

            _roleManager = new RoleManager<IdentityRole<Guid>>(roleStore, new IRoleValidator<IdentityRole<Guid>>[0],
                new UpperInvariantLookupNormalizer(), new IdentityErrorDescriber(), null);

            _notebookRepository = new BaseRepository<Notebook, Guid>(_dbContext);
            _folderRepository = new BaseRepository<Folder, Guid>(_dbContext);
            _noteRepository = new BaseRepository<Note, Guid>(_dbContext);
            _tagRepository = new BaseRepository<Tag, Guid>(_dbContext);

            _userService = new UserService(
                _userManager, _roleManager,
                _notebookRepository, _folderRepository,
                _noteRepository, _tagRepository
            );

            var allUsers = _userManager.Users.ToList();
            foreach (var user in allUsers)
            {
                await _userManager.DeleteAsync(user);
            }
        }

        [TearDown]
        public void TearDown()
        {
            _dbContext.Dispose();
            _userManager.Dispose();
            _roleManager.Dispose();
        }

        [Test]
        public async Task GetAllUsersAsync_ShouldReturnAllUsers()
        {
            var adminEmail = "admin@lunarapp.com";
            var adminUser = await _userManager.FindByEmailAsync(adminEmail);
            if (adminUser == null)
            {
                adminUser = new ApplicationUser { Email = adminEmail, UserName = "Admin" };
                await _userManager.CreateAsync(adminUser, "admin123");
            }

            var user1 = new ApplicationUser { Id = Guid.NewGuid(), Email = "user1@example.com" };
            var user2 = new ApplicationUser { Id = Guid.NewGuid(), Email = "user2@example.com" };

            await _userManager.CreateAsync(user1);
            await _userManager.CreateAsync(user2);

            var result = await _userService.GetAllUsersAsync();

            Assert.AreEqual(3, result.Count());
            var emails = result.Select(u => u.Email).ToList();
            Assert.Contains(adminEmail, emails);
            Assert.Contains(user1.Email, emails);
            Assert.Contains(user2.Email, emails);
        }

        [Test]
        public async Task UserExistsByIdAsync_ShouldReturnTrueIfUserExists()
        {
            var user = new ApplicationUser { Id = Guid.NewGuid(), Email = "user1@example.com" };
            await _userManager.CreateAsync(user);

            var result = await _userService.UserExistsByIdAsync(user.Id);

            Assert.IsTrue(result);
        }

        [Test]
        public async Task UserExistsByIdAsync_ShouldReturnFalseIfUserDoesNotExist()
        {
            var result = await _userService.UserExistsByIdAsync(Guid.NewGuid());

            Assert.IsFalse(result);
        }

        [Test]
        public async Task AssignUserToRoleAsync_ShouldAssignRoleToUser()
        {
            var user = new ApplicationUser { Id = Guid.NewGuid(), Email = "user1@example.com" };
            await _userManager.CreateAsync(user);
            var roleName = "Admin";
            await _roleManager.CreateAsync(new IdentityRole<Guid>(roleName));

            var result = await _userService.AssignUserToRoleAsync(user.Id, roleName);

            Assert.IsTrue(result);
            var roles = await _userManager.GetRolesAsync(user);
            Assert.Contains(roleName, roles.ToList());
        }

        [Test]
        public async Task RemoveUserRoleAsync_ShouldRemoveRoleFromUser()
        {
            var user = new ApplicationUser { Id = Guid.NewGuid(), Email = "user1@example.com" };
            await _userManager.CreateAsync(user);

            var roleName = "Admin";
            var role = new IdentityRole<Guid>(roleName);

            var existingRole = await _roleManager.FindByNameAsync(roleName);
            if (existingRole == null)
            {
                await _roleManager.CreateAsync(role);
            }

            await _userManager.AddToRoleAsync(user, roleName);

            var result = await _userService.RemoveUserRoleAsync(user.Id, roleName);

            Assert.IsTrue(result);

            var roles = await _userManager.GetRolesAsync(user);
            Assert.IsEmpty(roles);
        }

        [Test]
        public async Task DeleteUserAsync_ShouldDeleteUserAndRelatedData()
        {
            var user = new ApplicationUser { Id = Guid.NewGuid(), Email = "user1@example.com" };
            await _userManager.CreateAsync(user);

            var notebook = new Notebook { Id = Guid.NewGuid(), Title = "Test Notebook", UserId = user.Id };
            _dbContext.Add(notebook);
            await _dbContext.SaveChangesAsync();

            var result = await _userService.DeleteUserAsync(user.Id);

            Assert.IsTrue(result);

            var deletedUser = await _userManager.FindByIdAsync(user.Id.ToString());
            Assert.IsNull(deletedUser);

            var deletedNotebook = await _notebookRepository.GetByIdAsync(notebook.Id);
            Assert.IsNull(deletedNotebook);
        }
    }
}
