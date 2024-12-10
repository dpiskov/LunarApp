using LunarApp.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LunarApp.Data.Configuration
{
    public class NoteConfiguration : IEntityTypeConfiguration<Note>
    {
        public void Configure(EntityTypeBuilder<Note> builder)
        {
            builder.HasOne(n => n.Folder)
                .WithMany(f => f.Notes)
                .HasForeignKey(n => n.FolderId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(n => n.Notebook)
                .WithMany(nb => nb.Notes)
                .HasForeignKey(n => n.NotebookId)
                .OnDelete(DeleteBehavior.NoAction);


            builder.HasOne(n => n.User)
                .WithMany()
                .HasForeignKey(n => n.UserId)
                .OnDelete(DeleteBehavior.Cascade);


            builder.HasOne(n => n.ParentFolder)
                .WithMany()
                .HasForeignKey(n => n.ParentFolderId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(n => n.Tag)
                .WithMany()
                .HasForeignKey(n => n.TagId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
