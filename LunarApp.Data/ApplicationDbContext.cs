using LunarApp.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace LunarApp.Data
{
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

            var hasher = new PasswordHasher<ApplicationUser>();

            var user1Id = Guid.Parse("e3173a9a-c123-4cca-a93f-7fad5181bf42");
            var user2Id = Guid.Parse("3a7c43e8-a082-43fc-8c3b-199fea251913");

            builder.Entity<ApplicationUser>().HasData(
                new ApplicationUser
                {
                    Id = user1Id,
                    UserName = "User1",
                    NormalizedUserName = "USER1",
                    Email = "user1@lunarapp.com",
                    NormalizedEmail = "USER1@LUNARAPP.COM",
                    EmailConfirmed = true,
                    PasswordHash = hasher.HashPassword(null, "user123"),
                    SecurityStamp = Guid.NewGuid().ToString()
                },
                new ApplicationUser
                {
                    Id = user2Id,
                    UserName = "User2",
                    NormalizedUserName = "USER2",
                    Email = "user2@lunarapp.com",
                    NormalizedEmail = "USER2@LUNARAPP.COM",
                    EmailConfirmed = true,
                    PasswordHash = hasher.HashPassword(null, "user123"),
                    SecurityStamp = Guid.NewGuid().ToString()
                }
            );

            // Tag seeding for User1
            var tag1 = new Tag { Id = Guid.NewGuid(), Name = "Important", UserId = user1Id };
            var tag2 = new Tag { Id = Guid.NewGuid(), Name = "Urgent", UserId = user1Id };
            var tag3 = new Tag { Id = Guid.NewGuid(), Name = "To-Do", UserId = user1Id };
            var tag4 = new Tag { Id = Guid.NewGuid(), Name = "Review", UserId = user1Id };
            var tag5 = new Tag { Id = Guid.NewGuid(), Name = "Completed", UserId = user1Id };

            // Tag seeding for User2
            var tag6 = new Tag { Id = Guid.NewGuid(), Name = "Completed", UserId = user2Id };
            var tag7 = new Tag { Id = Guid.NewGuid(), Name = "In Progress", UserId = user2Id };
            var tag8 = new Tag { Id = Guid.NewGuid(), Name = "Backlog", UserId = user2Id };
            var tag9 = new Tag { Id = Guid.NewGuid(), Name = "Priority", UserId = user2Id };
            var tag10 = new Tag { Id = Guid.NewGuid(), Name = "Ideas", UserId = user2Id };

            builder.Entity<Tag>().HasData(tag1, tag2, tag3, tag4, tag5, tag6, tag7, tag8, tag9, tag10);

            var notebook1 = new Notebook
            {
                Id = Guid.NewGuid(),
                Title = "Web Development",
                Description = "Tips and tricks for building websites.",
                UserId = user1Id
            };

            // Folders for "Web Development"
            var folder1 = new Folder
            {
                Id = Guid.NewGuid(),
                Title = "HTML Basics",
                Description = "HTML structure and tags.",
                NotebookId = notebook1.Id,
                UserId = user1Id
            };

            var folder2 = new Folder
            {
                Id = Guid.NewGuid(),
                Title = "CSS Styling",
                Description = "Styling techniques with CSS.",
                NotebookId = notebook1.Id,
                UserId = user1Id
            };

            var folder3 = new Folder
            {
                Id = Guid.NewGuid(),
                Title = "JavaScript Essentials",
                Description = "JavaScript fundamentals for interactivity.",
                NotebookId = notebook1.Id,
                UserId = user1Id
            };

            var folder4 = new Folder
            {
                Id = Guid.NewGuid(),
                Title = "Responsive Design",
                Description = "Designing websites for all devices.",
                NotebookId = notebook1.Id,
                UserId = user1Id
            };

            var folder5 = new Folder
            {
                Id = Guid.NewGuid(),
                Title = "Web Development Frameworks",
                Description = "Exploring popular frameworks like React and Angular.",
                NotebookId = notebook1.Id,
                UserId = user1Id
            };

            // Subfolders for "HTML Basics"
            var subFolder1 = new Folder
            {
                Id = Guid.NewGuid(),
                Title = "HTML Structure",
                Description = "Basic structure of an HTML document.",
                ParentFolderId = folder1.Id,
                NotebookId = notebook1.Id,
                UserId = user1Id
            };

            var subFolder2 = new Folder
            {
                Id = Guid.NewGuid(),
                Title = "HTML Elements",
                Description = "Common HTML elements and their uses.",
                ParentFolderId = folder1.Id,
                NotebookId = notebook1.Id,
                UserId = user1Id
            };

            // Subfolders for "CSS Styling"
            var subFolder3 = new Folder
            {
                Id = Guid.NewGuid(),
                Title = "CSS Layouts",
                Description = "Different layout techniques using CSS.",
                ParentFolderId = folder2.Id,
                NotebookId = notebook1.Id,
                UserId = user1Id
            };

            var subFolder4 = new Folder
            {
                Id = Guid.NewGuid(),
                Title = "CSS Animations",
                Description = "Using CSS animations for dynamic websites.",
                ParentFolderId = folder2.Id,
                NotebookId = notebook1.Id,
                UserId = user1Id
            };

            // Subfolders for "JavaScript Essentials"
            var subFolder5 = new Folder
            {
                Id = Guid.NewGuid(),
                Title = "JavaScript Variables",
                Description = "Understanding variables and data types in JavaScript.",
                ParentFolderId = folder3.Id,
                NotebookId = notebook1.Id,
                UserId = user1Id
            };

            var subFolder6 = new Folder
            {
                Id = Guid.NewGuid(),
                Title = "JavaScript Functions",
                Description = "Creating and using functions in JavaScript.",
                ParentFolderId = folder3.Id,
                NotebookId = notebook1.Id,
                UserId = user1Id
            };

            // Subfolders for "Responsive Design"
            var subFolder7 = new Folder
            {
                Id = Guid.NewGuid(),
                Title = "Media Queries",
                Description = "Using media queries for responsive layouts.",
                ParentFolderId = folder4.Id,
                NotebookId = notebook1.Id,
                UserId = user1Id
            };

            var subFolder8 = new Folder
            {
                Id = Guid.NewGuid(),
                Title = "Mobile-First Design",
                Description = "Building websites with mobile-first principles.",
                ParentFolderId = folder4.Id,
                NotebookId = notebook1.Id,
                UserId = user1Id
            };

            // Subfolders for "Web Development Frameworks"
            var subFolder9 = new Folder
            {
                Id = Guid.NewGuid(),
                Title = "React Basics",
                Description = "Getting started with React framework.",
                ParentFolderId = folder5.Id,
                NotebookId = notebook1.Id,
                UserId = user1Id
            };

            var subFolder10 = new Folder
            {
                Id = Guid.NewGuid(),
                Title = "Angular Introduction",
                Description = "Basic concepts and setup of Angular framework.",
                ParentFolderId = folder5.Id,
                NotebookId = notebook1.Id,
                UserId = user1Id
            };

            // Notes for "HTML Basics"
            var note1 = new Note
            {
                Id = Guid.NewGuid(),
                Title = "HTML Structure",
                Body = "The basic structure of an HTML document with tags like <html>, <head>, <body>, etc.",
                FolderId = folder1.Id,
                NotebookId = notebook1.Id,
                UserId = user1Id,
                DateCreated = DateTime.Now,
                LastSaved = DateTime.Now
            };

            var note2 = new Note
            {
                Id = Guid.NewGuid(),
                Title = "Common HTML Elements",
                Body = "An overview of commonly used HTML elements like <div>, <h1>, <p>, etc.",
                FolderId = subFolder1.Id,
                NotebookId = notebook1.Id,
                UserId = user1Id,
                DateCreated = DateTime.Now,
                LastSaved = DateTime.Now
            };

            // Notes for "CSS Styling"
            var note3 = new Note
            {
                Id = Guid.NewGuid(),
                Title = "CSS Selectors",
                Body = "An introduction to CSS selectors for styling HTML elements.",
                FolderId = folder2.Id,
                NotebookId = notebook1.Id,
                UserId = user1Id,
                DateCreated = DateTime.Now,
                LastSaved = DateTime.Now
            };

            var note4 = new Note
            {
                Id = Guid.NewGuid(),
                Title = "Box Model",
                Body = "Understanding the CSS box model with padding, margin, border, and content.",
                FolderId = subFolder3.Id,
                NotebookId = notebook1.Id,
                UserId = user1Id,
                DateCreated = DateTime.Now,
                LastSaved = DateTime.Now
            };

            // Notes for "JavaScript Essentials"
            var note5 = new Note
            {
                Id = Guid.NewGuid(),
                Title = "JavaScript Variables",
                Body = "Understanding the different types of variables in JavaScript, including var, let, and const.",
                FolderId = folder3.Id,
                NotebookId = notebook1.Id,
                UserId = user1Id,
                DateCreated = DateTime.Now,
                LastSaved = DateTime.Now
            };

            var note6 = new Note
            {
                Id = Guid.NewGuid(),
                Title = "JavaScript Loops",
                Body = "Different types of loops in JavaScript like for, while, and do-while.",
                FolderId = subFolder5.Id,
                NotebookId = notebook1.Id,
                UserId = user1Id,
                DateCreated = DateTime.Now,
                LastSaved = DateTime.Now
            };

            // Notes for "Responsive Design"
            var note7 = new Note
            {
                Id = Guid.NewGuid(),
                Title = "CSS Grid",
                Body = "An introduction to CSS Grid layout system for building responsive designs.",
                FolderId = folder4.Id,
                NotebookId = notebook1.Id,
                UserId = user1Id,
                DateCreated = DateTime.Now,
                LastSaved = DateTime.Now
            };

            var note8 = new Note
            {
                Id = Guid.NewGuid(),
                Title = "Flexbox",
                Body = "How to use Flexbox for flexible and responsive layouts.",
                FolderId = subFolder7.Id,
                NotebookId = notebook1.Id,
                UserId = user1Id,
                DateCreated = DateTime.Now,
                LastSaved = DateTime.Now
            };

            // Notes for "Web Development Frameworks"
            var note9 = new Note
            {
                Id = Guid.NewGuid(),
                Title = "React Setup",
                Body = "Steps for setting up React using Create React App.",
                FolderId = folder5.Id,
                NotebookId = notebook1.Id,
                UserId = user1Id,
                DateCreated = DateTime.Now,
                LastSaved = DateTime.Now
            };

            var note10 = new Note
            {
                Id = Guid.NewGuid(),
                Title = "Angular Basics",
                Body = "Basic Angular setup and introduction to components, services, and modules.",
                FolderId = subFolder9.Id,
                NotebookId = notebook1.Id,
                UserId = user1Id,
                DateCreated = DateTime.Now,
                LastSaved = DateTime.Now
            };

            var notebook2 = new Notebook
            {
                Id = Guid.NewGuid(),
                Title = "Photography",
                Description = "Ideas for photo shoots, editing techniques, and gear recommendations.",
                UserId = user1Id
            };

            // Folders for "Photography"
            var folder6 = new Folder
            {
                Id = Guid.NewGuid(),
                Title = "Photo Shoot Ideas",
                Description = "Creative ideas for photo shoots.",
                NotebookId = notebook2.Id,
                UserId = user1Id
            };

            var folder7 = new Folder
            {
                Id = Guid.NewGuid(),
                Title = "Editing Techniques",
                Description = "Post-processing tips for photo editing.",
                NotebookId = notebook2.Id,
                UserId = user1Id
            };

            var folder8 = new Folder
            {
                Id = Guid.NewGuid(),
                Title = "Gear Recommendations",
                Description = "Best photography gear for different scenarios.",
                NotebookId = notebook2.Id,
                UserId = user1Id
            };

            // Subfolders for "Photo Shoot Ideas"
            var subFolder11 = new Folder
            {
                Id = Guid.NewGuid(),
                Title = "Outdoor Shoots",
                Description = "Ideas for shooting in outdoor environments.",
                ParentFolderId = folder6.Id,
                NotebookId = notebook2.Id,
                UserId = user1Id
            };

            var subFolder12 = new Folder
            {
                Id = Guid.NewGuid(),
                Title = "Indoor Shoots",
                Description = "Creative photo shoot ideas for indoor settings.",
                ParentFolderId = folder6.Id,
                NotebookId = notebook2.Id,
                UserId = user1Id
            };

            // Subfolders for "Editing Techniques"
            var subFolder13 = new Folder
            {
                Id = Guid.NewGuid(),
                Title = "Lightroom Techniques",
                Description = "Post-processing tips using Lightroom.",
                ParentFolderId = folder7.Id,
                NotebookId = notebook2.Id,
                UserId = user1Id
            };

            var subFolder14 = new Folder
            {
                Id = Guid.NewGuid(),
                Title = "Photoshop Editing",
                Description = "Advanced editing using Photoshop.",
                ParentFolderId = folder7.Id,
                NotebookId = notebook2.Id,
                UserId = user1Id
            };

            // Subfolders for "Gear Recommendations"
            var subFolder15 = new Folder
            {
                Id = Guid.NewGuid(),
                Title = "Cameras",
                Description = "Recommended cameras for various types of photography.",
                ParentFolderId = folder8.Id,
                NotebookId = notebook2.Id,
                UserId = user1Id
            };

            var subFolder16 = new Folder
            {
                Id = Guid.NewGuid(),
                Title = "Lenses",
                Description = "Choosing the right lenses for different shots.",
                ParentFolderId = folder8.Id,
                NotebookId = notebook2.Id,
                UserId = user1Id
            };

            // Notes for "Photo Shoot Ideas"
            var note11 = new Note
            {
                Id = Guid.NewGuid(),
                Title = "Nature Photography",
                Body = "Ideas for capturing the beauty of nature through the lens.",
                FolderId = folder6.Id,
                NotebookId = notebook2.Id,
                UserId = user1Id,
                DateCreated = DateTime.Now,
                LastSaved = DateTime.Now
            };

            var note12 = new Note
            {
                Id = Guid.NewGuid(),
                Title = "Studio Lighting",
                Body = "Tips for using studio lighting to enhance portraits.",
                FolderId = subFolder11.Id,
                NotebookId = notebook2.Id,
                UserId = user1Id,
                DateCreated = DateTime.Now,
                LastSaved = DateTime.Now
            };

            // Notes for "Editing Techniques"
            var note13 = new Note
            {
                Id = Guid.NewGuid(),
                Title = "Color Grading in Lightroom",
                Body = "Techniques for enhancing colors in Lightroom.",
                FolderId = folder7.Id,
                NotebookId = notebook2.Id,
                UserId = user1Id,
                DateCreated = DateTime.Now,
                LastSaved = DateTime.Now
            };

            var note14 = new Note
            {
                Id = Guid.NewGuid(),
                Title = "Retouching in Photoshop",
                Body = "Step-by-step guide to retouching portraits in Photoshop.",
                FolderId = subFolder13.Id,
                NotebookId = notebook2.Id,
                UserId = user1Id,
                DateCreated = DateTime.Now,
                LastSaved = DateTime.Now
            };

            // Notes for "Gear Recommendations"
            var note15 = new Note
            {
                Id = Guid.NewGuid(),
                Title = "Best DSLR Cameras for Beginners",
                Body = "A list of the best DSLR cameras for photography novices.",
                FolderId = folder8.Id,
                NotebookId = notebook2.Id,
                UserId = user1Id,
                DateCreated = DateTime.Now,
                LastSaved = DateTime.Now
            };

            var note16 = new Note
            {
                Id = Guid.NewGuid(),
                Title = "Choosing Lenses for Portrait Photography",
                Body = "A guide to selecting the best lenses for portrait shots.",
                FolderId = subFolder15.Id,
                NotebookId = notebook2.Id,
                UserId = user1Id,
                DateCreated = DateTime.Now,
                LastSaved = DateTime.Now
            };

            // User 2 Notebooks and Folders
            var notebook3 = new Notebook
            {
                Id = Guid.NewGuid(),
                Title = "Digital Marketing",
                Description = "Strategies for online marketing.",
                UserId = user2Id
            };

            // Folders for "Digital Marketing"
            var folder9 = new Folder
            {
                Id = Guid.NewGuid(),
                Title = "SEO Strategies",
                Description = "Techniques for optimizing websites for search engines.",
                NotebookId = notebook3.Id,
                UserId = user2Id
            };

            var folder10 = new Folder
            {
                Id = Guid.NewGuid(),
                Title = "Social Media Campaigns",
                Description = "Creating and managing successful social media campaigns.",
                NotebookId = notebook3.Id,
                UserId = user2Id
            };

            var folder11 = new Folder
            {
                Id = Guid.NewGuid(),
                Title = "Email Marketing",
                Description = "Building and managing email marketing lists.",
                NotebookId = notebook3.Id,
                UserId = user2Id
            };

            // Subfolders for "SEO Strategies"
            var subFolder17 = new Folder
            {
                Id = Guid.NewGuid(),
                Title = "On-Page SEO",
                Description = "Techniques for optimizing individual web pages.",
                ParentFolderId = folder9.Id,
                NotebookId = notebook3.Id,
                UserId = user2Id
            };

            var subFolder18 = new Folder
            {
                Id = Guid.NewGuid(),
                Title = "Off-Page SEO",
                Description = "Link building and other off-page SEO techniques.",
                ParentFolderId = folder9.Id,
                NotebookId = notebook3.Id,
                UserId = user2Id
            };

            // Subfolders for "Social Media Campaigns"
            var subFolder19 = new Folder
            {
                Id = Guid.NewGuid(),
                Title = "Facebook Ads",
                Description = "Creating effective Facebook ads campaigns.",
                ParentFolderId = folder10.Id,
                NotebookId = notebook3.Id,
                UserId = user2Id
            };

            var subFolder20 = new Folder
            {
                Id = Guid.NewGuid(),
                Title = "Instagram Marketing",
                Description = "Growing your brand on Instagram with engaging content.",
                ParentFolderId = folder10.Id,
                NotebookId = notebook3.Id,
                UserId = user2Id
            };

            // Subfolders for "Email Marketing"
            var subFolder21 = new Folder
            {
                Id = Guid.NewGuid(),
                Title = "List Building",
                Description = "Techniques for building a quality email list.",
                ParentFolderId = folder11.Id,
                NotebookId = notebook3.Id,
                UserId = user2Id
            };

            var subFolder22 = new Folder
            {
                Id = Guid.NewGuid(),
                Title = "Email Campaign Design",
                Description = "Best practices for designing effective email campaigns.",
                ParentFolderId = folder11.Id,
                NotebookId = notebook3.Id,
                UserId = user2Id
            };

            // Notes for "SEO Strategies"
            var note17 = new Note
            {
                Id = Guid.NewGuid(),
                Title = "Keyword Research",
                Body = "How to find the right keywords for SEO.",
                FolderId = folder9.Id,
                NotebookId = notebook3.Id,
                UserId = user2Id,
                DateCreated = DateTime.Now,
                LastSaved = DateTime.Now
            };

            var note18 = new Note
            {
                Id = Guid.NewGuid(),
                Title = "Optimizing for Google",
                Body = "Techniques for optimizing your website for Google's algorithms.",
                FolderId = subFolder17.Id,
                NotebookId = notebook3.Id,
                UserId = user2Id,
                DateCreated = DateTime.Now,
                LastSaved = DateTime.Now
            };

            // Notes for "Social Media Campaigns"
            var note19 = new Note
            {
                Id = Guid.NewGuid(),
                Title = "Creating Facebook Ads",
                Body = "Step-by-step guide to creating effective Facebook ads.",
                FolderId = folder10.Id,
                NotebookId = notebook3.Id,
                UserId = user2Id,
                DateCreated = DateTime.Now,
                LastSaved = DateTime.Now
            };

            var note20 = new Note
            {
                Id = Guid.NewGuid(),
                Title = "Engaging Instagram Content",
                Body = "How to create content that engages your Instagram audience.",
                FolderId = subFolder19.Id,
                NotebookId = notebook3.Id,
                UserId = user2Id,
                DateCreated = DateTime.Now,
                LastSaved = DateTime.Now
            };

            // Notes for "Email Marketing"
            var note21 = new Note
            {
                Id = Guid.NewGuid(),
                Title = "Building an Email List",
                Body = "Tips for growing a list of engaged email subscribers.",
                FolderId = folder11.Id,
                NotebookId = notebook3.Id,
                UserId = user2Id,
                DateCreated = DateTime.Now,
                LastSaved = DateTime.Now
            };

            var note22 = new Note
            {
                Id = Guid.NewGuid(),
                Title = "Effective Email Campaigns",
                Body = "Creating email campaigns that convert subscribers into customers.",
                FolderId = subFolder21.Id,
                NotebookId = notebook3.Id,
                UserId = user2Id,
                DateCreated = DateTime.Now,
                LastSaved = DateTime.Now
            };

            var notebook4 = new Notebook
            {
                Id = Guid.NewGuid(),
                Title = "Art Portfolio",
                Description = "Artworks, sketches, and inspirations.",
                UserId = user2Id
            };

            // Folders for "Art Portfolio"
            var folder12 = new Folder
            {
                Id = Guid.NewGuid(),
                Title = "Sketches",
                Description = "Initial sketches and drawing concepts.",
                NotebookId = notebook4.Id,
                UserId = user2Id
            };

            var folder13 = new Folder
            {
                Id = Guid.NewGuid(),
                Title = "Digital Art",
                Description = "Artworks created with digital tools.",
                NotebookId = notebook4.Id,
                UserId = user2Id
            };

            var folder14 = new Folder
            {
                Id = Guid.NewGuid(),
                Title = "Inspirations",
                Description = "Artistic inspirations and reference images.",
                NotebookId = notebook4.Id,
                UserId = user2Id
            };

            // Subfolders for "Sketches"
            var subFolder23 = new Folder
            {
                Id = Guid.NewGuid(),
                Title = "Landscape Sketches",
                Description = "Initial sketches of landscape designs.",
                ParentFolderId = folder12.Id,
                NotebookId = notebook4.Id,
                UserId = user2Id
            };

            var subFolder24 = new Folder
            {
                Id = Guid.NewGuid(),
                Title = "Character Designs",
                Description = "Sketches of characters for various projects.",
                ParentFolderId = folder12.Id,
                NotebookId = notebook4.Id,
                UserId = user2Id
            };

            // Subfolders for "Digital Art"
            var subFolder25 = new Folder
            {
                Id = Guid.NewGuid(),
                Title = "Portraits",
                Description = "Digital portrait artworks.",
                ParentFolderId = folder13.Id,
                NotebookId = notebook4.Id,
                UserId = user2Id
            };

            var subFolder26 = new Folder
            {
                Id = Guid.NewGuid(),
                Title = "Abstract Art",
                Description = "Abstract digital art creations.",
                ParentFolderId = folder13.Id,
                NotebookId = notebook4.Id,
                UserId = user2Id
            };

            // Subfolders for "Inspirations"
            var subFolder27 = new Folder
            {
                Id = Guid.NewGuid(),
                Title = "Nature Photography",
                Description = "Photography references for nature-themed artworks.",
                ParentFolderId = folder14.Id,
                NotebookId = notebook4.Id,
                UserId = user2Id
            };

            var subFolder28 = new Folder
            {
                Id = Guid.NewGuid(),
                Title = "Fantasy Art",
                Description = "Inspirations from fantasy and sci-fi art.",
                ParentFolderId = folder14.Id,
                NotebookId = notebook4.Id,
                UserId = user2Id
            };

            // Notes for "Sketches"
            var note23 = new Note
            {
                Id = Guid.NewGuid(),
                Title = "Mountain Landscape",
                Body = "Sketch of a mountain landscape with a river running through it.",
                FolderId = folder12.Id,
                NotebookId = notebook4.Id,
                UserId = user2Id,
                DateCreated = DateTime.Now,
                LastSaved = DateTime.Now
            };

            var note24 = new Note
            {
                Id = Guid.NewGuid(),
                Title = "Fantasy Character",
                Body = "Initial sketch of a fantasy character with magical abilities.",
                FolderId = subFolder23.Id,
                NotebookId = notebook4.Id,
                UserId = user2Id,
                DateCreated = DateTime.Now,
                LastSaved = DateTime.Now
            };

            // Notes for "Digital Art"
            var note25 = new Note
            {
                Id = Guid.NewGuid(),
                Title = "Digital Portrait",
                Body = "A digital painting of a woman with flowing hair.",
                FolderId = folder13.Id,
                NotebookId = notebook4.Id,
                UserId = user2Id,
                DateCreated = DateTime.Now,
                LastSaved = DateTime.Now
            };

            var note26 = new Note
            {
                Id = Guid.NewGuid(),
                Title = "Abstract Geometric Art",
                Body = "Creating abstract geometric shapes using digital tools.",
                FolderId = subFolder25.Id,
                NotebookId = notebook4.Id,
                UserId = user2Id,
                DateCreated = DateTime.Now,
                LastSaved = DateTime.Now
            };

            // Notes for "Inspirations"
            var note27 = new Note
            {
                Id = Guid.NewGuid(),
                Title = "Nature Art References",
                Body = "Images of forests, mountains, and rivers to inspire landscape art.",
                FolderId = folder14.Id,
                NotebookId = notebook4.Id,
                UserId = user2Id,
                DateCreated = DateTime.Now,
                LastSaved = DateTime.Now
            };

            var note28 = new Note
            {
                Id = Guid.NewGuid(),
                Title = "Sci-Fi Concept Art",
                Body = "Inspirations from sci-fi concept art for a futuristic city.",
                FolderId = subFolder27.Id,
                NotebookId = notebook4.Id,
                UserId = user2Id,
                DateCreated = DateTime.Now,
                LastSaved = DateTime.Now
            };

            note1.TagId = tag1.Id;
            note2.TagId = tag3.Id;
            note3.TagId = tag4.Id;
            note4.TagId = tag5.Id;
            note5.TagId = tag3.Id;
            note6.TagId = tag2.Id;
            note7.TagId = tag4.Id;
            note8.TagId = tag1.Id;
            note9.TagId = tag5.Id;
            note10.TagId = tag2.Id;
            note11.TagId = tag1.Id;
            note12.TagId = tag2.Id;
            note13.TagId = tag3.Id;
            note14.TagId = tag4.Id;
            note15.TagId = tag5.Id;
            note16.TagId = tag5.Id;
            note17.TagId = tag6.Id;
            note18.TagId = tag7.Id;
            note19.TagId = tag8.Id;
            note20.TagId = tag9.Id;
            note21.TagId = tag10.Id;
            note22.TagId = tag6.Id;
            note23.TagId = tag7.Id;
            note24.TagId = tag8.Id;
            note25.TagId = tag9.Id;
            note26.TagId = tag10.Id;
            note27.TagId = tag6.Id;
            note28.TagId = tag7.Id;

            // Save the notebooks, folders, notes, and tags to the database
            builder.Entity<Notebook>().HasData(notebook1, notebook2, notebook3, notebook4);
            builder.Entity<Folder>().HasData(
                folder1, folder2, folder3, folder4, folder5,
                subFolder1, subFolder2, subFolder3, subFolder4, subFolder5, subFolder6,
                subFolder7, subFolder8, subFolder9, subFolder10,
                folder6, folder7, folder8,
                subFolder11, subFolder12, subFolder13, subFolder14, subFolder15, subFolder16,
                folder9, folder10, folder11,
                subFolder17, subFolder18, subFolder19, subFolder20, subFolder21, subFolder22,
                folder12, folder13, folder14,
                subFolder23, subFolder24, subFolder25, subFolder26, subFolder27, subFolder28
            );
            builder.Entity<Note>().HasData(
                note1, note2, note3, note4, note5, note6,
                note7, note8, note9, note10, note11, note12,
                note13, note14, note15, note16, note17, note18,
                note19, note20, note21, note22, note23, note24,
                note25, note26, note27, note28
            );
        }
    }
}
