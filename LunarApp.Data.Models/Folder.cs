using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using static LunarApp.Common.ValidationConstants.Folder;

namespace LunarApp.Data.Models
{
    public class Folder
    {
        [Key]
        [Comment("Folder Identifier")]
        public Guid Id { get; set; } = Guid.NewGuid();
        [Required]
        [MaxLength(FolderTitleMaxLength)]
        [Comment("Folder title")]
        public required string Title { get; set; }
        [MaxLength(FolderDescriptionMaxLength)]
        [Comment("Folder description")]
        public string? Description { get; set; }
        [Required]
        [Comment("Identifier of a notebook")]
        public Guid NotebookId { get; set; }
        [ForeignKey(nameof(NotebookId))]
        public virtual Notebook? Notebook { get; set; }
        [Comment("Identifier of a parent folder")]
        public Guid? ParentFolderId { get; set; }
        [ForeignKey(nameof(ParentFolderId))]
        public virtual Folder? ParentFolder { get; set; }

        public virtual ICollection<Folder> ChildrenFolders { get; set; } = new List<Folder>();
        public virtual ICollection<Note> Notes { get; set; } = new List<Note>();
    }
}