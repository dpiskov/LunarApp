using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using static LunarApp.Web.Common.ValidationConstants;

namespace LunarApp.Web.Data.Models
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
        public required Notebook Notebook { get; set; }
        public Notebook Notebook { get; set; }
        //[Required]
        [Comment("Identifier of a folder")]
        public Guid? FolderId { get; set; }
        [ForeignKey(nameof(FolderId))]
        public Folder Folder { get; set; }
        [Required]
        [Comment("The date the note was created on")]
        public DateTime DateCreated { get; set; }
        [Required]
        [Comment("The date the note was last saved on")]
        public DateTime LastSaved { get; set; }
    }
}
