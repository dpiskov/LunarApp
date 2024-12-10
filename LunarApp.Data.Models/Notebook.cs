using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using static LunarApp.Common.ValidationConstants.Notebook;

namespace LunarApp.Data.Models
{
    public class Notebook
    {
        [Key]
        [Comment("Notebook Identifier")]
        public Guid Id { get; set; } = Guid.NewGuid();
        [Required]
        [MaxLength(NotebookTitleMaxLength)]
        [Comment("Notebook title")]
        public required string Title { get; set; }
        [MaxLength(NotebookDescriptionMaxLength)]
        [Comment("Notebook description")]
        public string? Description { get; set; }

        [Required]
        [Comment("User Identifier")]
        public Guid UserId { get; set; }

        [ForeignKey(nameof(UserId))]
        public virtual ApplicationUser User { get; set; } = null!;
        public virtual ICollection<Folder> Folders { get; set; } = new List<Folder>();
        public virtual ICollection<Note> Notes { get; set; } = new List<Note>();
    }
}