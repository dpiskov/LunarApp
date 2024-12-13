using LunarApp.Data;
using LunarApp.Data.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

using static LunarApp.Common.ApplicationConstants;

namespace LunarApp.Web.Infrastructure.Extensions
{
    /// <summary>
    /// Provides extension methods for the <see cref="IApplicationBuilder"/> to handle database migrations and seeding operations.
    /// </summary>
    public static class ApplicationBuilderExtensions
    {
        /// <summary>
        /// Applies any pending migrations to the database at the start of the application.
        /// This ensures the database schema is up-to-date with the application's models.
        /// </summary>
        /// <param name="app">The <see cref="IApplicationBuilder"/> instance to apply the migration on.</param>
        /// <returns>The updated <see cref="IApplicationBuilder"/> instance.</returns>
        public static IApplicationBuilder ApplyMigrations(this IApplicationBuilder app)
        {
            using var serviceScope = app.ApplicationServices.CreateScope();
            ApplicationDbContext dbContext = serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            dbContext.Database.Migrate();

            return app;
        }

        /// <summary>
        /// Seeds the administrator user and role into the system if they do not already exist.
        /// Creates the <see cref="AdminRoleName"/> role and a user with the specified credentials, 
        /// then assigns the user to the role.
        /// </summary>
        /// <param name="app">The <see cref="IApplicationBuilder"/> instance to run the seeding operation.</param>
        /// <param name="email">The email address of the administrator.</param>
        /// <param name="username">The username of the administrator.</param>
        /// <param name="password">The password for the administrator user.</param>
        /// <returns>The updated <see cref="IApplicationBuilder"/> instance.</returns>
        /// <exception cref="ArgumentNullException">Thrown if any required service (RoleManager, IUserStore, UserManager) is missing.</exception>
        /// <exception cref="InvalidOperationException">Thrown if there are errors creating roles or adding the user to a role.</exception>
        public static IApplicationBuilder SeedAdministrator(this IApplicationBuilder app, string email, string username, string password)
        {
            using IServiceScope serviceScope = app.ApplicationServices.CreateAsyncScope();

            IServiceProvider serviceProvider = serviceScope.ServiceProvider;

            RoleManager<IdentityRole<Guid>>? roleManager = serviceProvider
                    .GetService<RoleManager<IdentityRole<Guid>>>();
            IUserStore<ApplicationUser>? userStore = serviceProvider.GetService<IUserStore<ApplicationUser>>();
            UserManager<ApplicationUser>? userManager = serviceProvider.GetService<UserManager<ApplicationUser>>();

            if (roleManager == null)
            {
                throw new ArgumentNullException(nameof(roleManager),
                    $"Service for {typeof(RoleManager<IdentityRole<Guid>>)} cannot be obtained!");
            }

            if (userStore == null)
            {
                throw new ArgumentNullException(nameof(userStore),
                    $"Service for {typeof(IUserStore<ApplicationUser>)} cannot be obtained!");
            }

            if (userManager == null)
            {
                throw new ArgumentNullException(nameof(userManager),
                    $"Service for {typeof(UserManager<ApplicationUser>)} cannot be obtained!");
            }

            Task.Run(async () =>
            {
                bool roleExists = await roleManager.RoleExistsAsync(AdminRoleName);

                IdentityRole<Guid>? adminRole = null;

                if (roleExists == false)
                {
                    adminRole = new IdentityRole<Guid>(AdminRoleName);

                    IdentityResult result = await roleManager.CreateAsync(adminRole);

                    if (result.Succeeded == false)
                    {
                        throw new InvalidOperationException($"Error occured while creating the {AdminRoleName} role!");
                    }
                }
                else
                {
                    adminRole = await roleManager.FindByNameAsync(AdminRoleName);
                }


                ApplicationUser? adminUser = await userManager.FindByEmailAsync(email);
                if (adminUser == null)
                {
                    adminUser = await CreateAdminUserAsync(email, username, password, userStore, userManager);
                }

                if (await userManager.IsInRoleAsync(adminUser, AdminRoleName))
                {
                    return app;
                }

                IdentityResult userResult = await userManager.AddToRoleAsync(adminUser, AdminRoleName);

                if (userResult.Succeeded == false)
                {
                    throw new InvalidOperationException($"Error occured while adding the user {username} to the {AdminRoleName} role!");
                }

                return app;
            })
            .GetAwaiter()
            .GetResult();

            return app;
        }

        /// <summary>
        /// Creates a new administrator user with the specified credentials and assigns them to the <see cref="AdminRoleName"/> role.
        /// </summary>
        /// <param name="email">The email address of the administrator.</param>
        /// <param name="username">The username for the administrator.</param>
        /// <param name="password">The password for the administrator user.</param>
        /// <param name="userStore">The <see cref="IUserStore{ApplicationUser}"/> used to manage user data.</param>
        /// <param name="userManager">The <see cref="UserManager{ApplicationUser}"/> used for creating and managing users.</param>
        /// <returns>The newly created <see cref="ApplicationUser"/> instance representing the administrator.</returns>
        /// <exception cref="InvalidOperationException">Thrown if user creation fails.</exception>
        private static async Task<ApplicationUser> CreateAdminUserAsync(string email, string username, string password,
            IUserStore<ApplicationUser> userStore, UserManager<ApplicationUser> userManager)
        {
            ApplicationUser applicationUser = new ApplicationUser
            {
                Email = email
            };

            await userStore.SetUserNameAsync(applicationUser, username, CancellationToken.None);
            IdentityResult result = await userManager.CreateAsync(applicationUser, password);

            if (result.Succeeded == false)
            {
                throw new InvalidOperationException($"Error occured while registering {AdminRoleName} user!");
            }

            return applicationUser;
        }
    }
}
