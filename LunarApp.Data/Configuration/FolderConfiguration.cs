using LunarApp.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LunarApp.Data.Configuration
{
    public class FolderConfiguration : IEntityTypeConfiguration<Folder>
    {
        public void Configure(EntityTypeBuilder<Folder> builder)
        {
            // Notebook to Folder relationship (cascade delete)
            builder.HasOne(f => f.Notebook)  // A Folder belongs to one Notebook
                .WithMany(nb => nb.Folders)  // A Notebook has many Folders
                .HasForeignKey(f => f.NotebookId)  // Foreign key from Folder to Notebook
                .OnDelete(DeleteBehavior.Cascade);  // Cascade delete when Notebook is deleted

            // Folder to Folder relationship (self-referencing) (cascade delete)
            builder.HasOne(f => f.ParentFolder)  // A Folder has one ParentFolder
                .WithMany(pf => pf.ChildrenFolders)  // A ParentFolder has many ChildrenFolders
                .HasForeignKey(f => f.ParentFolderId)  // Foreign key for the self-referencing relation
                .OnDelete(DeleteBehavior.Restrict);  // Use Restrict to prevent automatic cascade delete
        }
    }
}
