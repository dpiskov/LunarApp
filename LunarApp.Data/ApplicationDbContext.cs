using LunarApp.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace LunarApp.Data
{
    //public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>(options)
    //{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>
    {
        public ApplicationDbContext()
        {

        }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }
        public virtual DbSet<Notebook> Notebooks { get; set; } = null!;
        public virtual DbSet<Folder> Folders { get; set; } = null!;
        public virtual DbSet<Note> Notes { get; set; } = null!;
        public virtual DbSet<Tag> Tags { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            // Create Notebooks
            var softwareEngineeringNotebook = new Notebook
            {
                Id = Guid.NewGuid(),
                Title = "Software Engineering",
                Description = "A notebook for all things software development."
            };

            var musicProductionNotebook = new Notebook
            {
                Id = Guid.NewGuid(),
                Title = "Music Production",
                Description = "For all music production-related notes."
            };

            var videoEditingNotebook = new Notebook
            {
                Id = Guid.NewGuid(),
                Title = "Video Editing",
                Description = "Notes on video editing, techniques, and tutorials."
            };

            var personalDevelopmentNotebook = new Notebook
            {
                Id = Guid.NewGuid(),
                Title = "Personal Development",
                Description = "A place for goals, self-improvement tips, and productivity hacks."
            };

            var creativeWritingNotebook = new Notebook
            {
                Id = Guid.NewGuid(),
                Title = "Creative Writing",
                Description = "A notebook for stories, poetry, and creative writing ideas."
            };

            // Create Folders for each Notebook
            var softwareResourcesFolder = new Folder
            {
                Id = Guid.NewGuid(),
                Title = "Resources",
                Description = "Links to useful software engineering resources.",
                NotebookId = softwareEngineeringNotebook.Id
            };

            var softwareCodeSnippetsFolder = new Folder
            {
                Id = Guid.NewGuid(),
                Title = "Code Snippets",
                Description = "Quick access to reusable code.",
                NotebookId = softwareEngineeringNotebook.Id
            };

            var musicSamplesFolder = new Folder
            {
                Id = Guid.NewGuid(),
                Title = "Samples",
                Description = "A collection of music samples for production.",
                NotebookId = musicProductionNotebook.Id
            };

            var videoTechniquesFolder = new Folder
            {
                Id = Guid.NewGuid(),
                Title = "Techniques",
                Description = "Tutorials on video editing techniques.",
                NotebookId = videoEditingNotebook.Id
            };

            var personalGoalsFolder = new Folder
            {
                Id = Guid.NewGuid(),
                Title = "Goals",
                Description = "Track personal development goals.",
                NotebookId = personalDevelopmentNotebook.Id
            };

            var writingIdeasFolder = new Folder
            {
                Id = Guid.NewGuid(),
                Title = "Ideas",
                Description = "Ideas for future stories or poems.",
                NotebookId = creativeWritingNotebook.Id
            };

            // Create Notes for each Folder
            var softwareDesignPatternsNote = new Note
            {
                Id = Guid.NewGuid(),
                Title = "Design Patterns",
                Body = "Notes on various software design patterns, including Singleton, Factory, and Observer.",
                NotebookId = softwareEngineeringNotebook.Id,
                FolderId = softwareCodeSnippetsFolder.Id,
                DateCreated = DateTime.Now,
                LastSaved = DateTime.Now
            };

            var musicMixingTipsNote = new Note
            {
                Id = Guid.NewGuid(),
                Title = "Mixing Tips",
                Body = "Techniques for mixing tracks in a DAW, with focus on EQ and compression.",
                NotebookId = musicProductionNotebook.Id,
                FolderId = musicSamplesFolder.Id,
                DateCreated = DateTime.Now,
                LastSaved = DateTime.Now
            };

            var videoColorGradingNote = new Note
            {
                Id = Guid.NewGuid(),
                Title = "Color Grading Techniques",
                Body = "A guide to color grading for video editors using different software.",
                NotebookId = videoEditingNotebook.Id,
                FolderId = videoTechniquesFolder.Id,
                DateCreated = DateTime.Now,
                LastSaved = DateTime.Now
            };

            var personalMorningRoutineNote = new Note
            {
                Id = Guid.NewGuid(),
                Title = "Morning Routine",
                Body = "A step-by-step guide to an effective morning routine to start the day right.",
                NotebookId = personalDevelopmentNotebook.Id,
                FolderId = personalGoalsFolder.Id,
                DateCreated = DateTime.Now,
                LastSaved = DateTime.Now
            };

            var writingCharacterDevelopmentNote = new Note
            {
                Id = Guid.NewGuid(),
                Title = "Character Development",
                Body = "Techniques for creating deep, multi-dimensional characters in fiction.",
                NotebookId = creativeWritingNotebook.Id,
                FolderId = writingIdeasFolder.Id,
                DateCreated = DateTime.Now,
                LastSaved = DateTime.Now
            };

            builder.Entity<Notebook>().HasData(
                softwareEngineeringNotebook,
                musicProductionNotebook,
                videoEditingNotebook,
                personalDevelopmentNotebook,
                creativeWritingNotebook
            );

            builder.Entity<Folder>().HasData(
                softwareResourcesFolder,
                softwareCodeSnippetsFolder,
                musicSamplesFolder,
                videoTechniquesFolder,
                personalGoalsFolder,
                writingIdeasFolder
            );

            builder.Entity<Note>().HasData(
                softwareDesignPatternsNote,
                musicMixingTipsNote,
                videoColorGradingNote,
                personalMorningRoutineNote,
                writingCharacterDevelopmentNote
            );

        }
    }
}
