using LunarApp.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LunarApp.Data.Configuration
{
    public class NotebookConfiguration : IEntityTypeConfiguration<Notebook>
    {
        public void Configure(EntityTypeBuilder<Notebook> builder)
        {
            builder.HasOne(n => n.User)
                .WithMany()
                .HasForeignKey(n => n.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(n => n.Folders)
                .WithOne(f => f.Notebook)
                .HasForeignKey(f => f.NotebookId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasMany(n => n.Notes)
                .WithOne(note => note.Notebook)
                .HasForeignKey(note => note.NotebookId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
