using LunarApp.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LunarApp.Data.Configuration
{
    public class NoteConfiguration : IEntityTypeConfiguration<Note>
    {
        public void Configure(EntityTypeBuilder<Note> builder)
        {
            // Folder to Note relationship: Remove cascade delete to handle it programmatically
            builder.HasOne(n => n.Folder)  // A Note belongs to one Folder
                .WithMany(f => f.Notes)  // A Folder has many Notes
                .HasForeignKey(n => n.FolderId)  // Foreign key from Note to Folder
                .OnDelete(DeleteBehavior.Restrict); // Use Restrict to prevent automatic cascade delete

            // Notebook to Note relationship (cascade delete)
            builder.HasOne(n => n.Notebook)  // A Note belongs to one Notebook
                .WithMany(nb => nb.Notes)  // A Notebook has many Notes
                .HasForeignKey(n => n.NotebookId)  // Foreign key from Note to Notebook
                .OnDelete(DeleteBehavior.Cascade);  // Cascade delete when Notebook is deleted
        }
    }
}
