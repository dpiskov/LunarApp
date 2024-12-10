using LunarApp.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LunarApp.Data.Configuration
{
    public class FolderConfiguration : IEntityTypeConfiguration<Folder>
    {
        public void Configure(EntityTypeBuilder<Folder> builder)
        {
            builder.HasOne(f => f.Notebook)
                .WithMany(nb => nb.Folders)
                .HasForeignKey(f => f.NotebookId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(f => f.ParentFolder)
                .WithMany(pf => pf.ChildrenFolders)
                .HasForeignKey(f => f.ParentFolderId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(f => f.User)
                .WithMany()
                .HasForeignKey(f => f.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(f => f.Notes)
                .WithOne(n => n.Folder)
                .HasForeignKey(n => n.FolderId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasMany(f => f.ChildrenFolders)
                .WithOne(cf => cf.ParentFolder)
                .HasForeignKey(cf => cf.ParentFolderId)
                .OnDelete(DeleteBehavior.Restrict);

        }
    }
}
