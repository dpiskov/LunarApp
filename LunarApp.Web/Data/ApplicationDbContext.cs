using LunarApp.Web.Data.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace LunarApp.Web.Data
{

    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext(options)
    {
        public DbSet<Notebook> Notebooks { get; set; }
        public DbSet<Folder> Folders { get; set; }
        public DbSet<Note> Notes { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Notebook to Folder relationship (cascade delete)
            builder.Entity<Folder>()
                .HasOne(f => f.Notebook)  // A Folder belongs to one Notebook
                .WithMany(n => n.Folders)  // A Notebook has many Folders
                .HasForeignKey(f => f.NotebookId)  // Foreign key from Folder to Notebook
                .OnDelete(DeleteBehavior.Cascade);  // Cascade delete when Notebook is deleted

            // Folder to Folder relationship (self-referencing) (cascade delete)
            builder.Entity<Folder>()
                .HasOne(f => f.ParentFolder)  // A Folder has one ParentFolder
                .WithMany(f => f.ChildrenFolders)  // A ParentFolder has many ChildrenFolders
                .HasForeignKey(f => f.ParentFolderId)  // Foreign key for the self-referencing relation
                .OnDelete(DeleteBehavior.Restrict);  // Cascade delete when ParentFolder is deleted

            // Folder to Note relationship: Remove cascade delete to handle it programmatically
            builder.Entity<Note>()
                .HasOne(n => n.Folder)  // A Note belongs to one Folder
                .WithMany(f => f.Notes)  // A Folder has many Notes
                .HasForeignKey(n => n.FolderId)  // Foreign key from Note to Folder
                .OnDelete(DeleteBehavior.Restrict); // Use Restrict to prevent automatic cascade delete

            // Notebook to Note relationship (cascade delete)
            builder.Entity<Note>()
                .HasOne(n => n.Notebook)  // A Note belongs to one Notebook
                .WithMany(b => b.Notes)  // A Notebook has many Notes
                .HasForeignKey(n => n.NotebookId)  // Foreign key from Note to Notebook
                .OnDelete(DeleteBehavior.Cascade);  // Cascade delete when Notebook is deleted
        }
    }

    //public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext(options)
    //{
    //    public DbSet<Note> Notes { get; set; }
    //    public DbSet<Folder> Folders { get; set; }
    //    public DbSet<Notebook> Notebooks { get; set; }
    //}
}
