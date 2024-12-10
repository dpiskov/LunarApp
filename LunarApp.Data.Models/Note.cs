using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using static LunarApp.Common.ValidationConstants.Note;

namespace LunarApp.Data.Models
{
    public class Note
    {
        [Key]
        [Comment("Note Identifier")]
        public Guid Id { get; set; } = Guid.NewGuid();
        [Required]
        [MaxLength(NoteTitleMaxLength)]
        [Comment("Note title")]
        public required string Title { get; set; }
        [MaxLength(NoteBodyMaxLength)]
        [Comment("Note body")]
        public string? Body { get; set; }
        [Required]
        [Comment("Identifier of a notebook")]
        public Guid NotebookId { get; set; }
        [ForeignKey(nameof(NotebookId))]
        public virtual Notebook? Notebook { get; set; }
        //public required Notebook Notebook { get; set; }
        [Comment("Identifier of a parent folder")]
        public Guid? ParentFolderId { get; set; }
        [ForeignKey(nameof(ParentFolderId))]
        public virtual Folder? ParentFolder { get; set; }
        [Comment("Identifier of a folder")]
        public Guid? FolderId { get; set; }
        [ForeignKey(nameof(FolderId))]
        public virtual Folder? Folder { get; set; }
        [Comment("Identifier of a tag")]
        public Guid? TagId { get; set; }
        [ForeignKey(nameof(TagId))]
        public virtual Tag? Tag { get; set; }
        [Required]
        [Comment("User Identifier")]
        public Guid UserId { get; set; }

        [ForeignKey(nameof(UserId))]
        public virtual ApplicationUser User { get; set; } = null!;
        [Required]
        [Comment("The date the note was created on")]
        public DateTime DateCreated { get; set; }
        [Required]
        [Comment("The date the note was last saved on")]
        public DateTime LastSaved { get; set; }
    }
}
