using static LunarApp.Web.Common.ValidationConstants;
using System.ComponentModel.DataAnnotations;

namespace LunarApp.Web.Models
{
    public class NoteCreateViewModel
    {
        [Required]
        [MinLength(NoteTitleMinLength)]
        [MaxLength(NoteTitleMaxLength)]
        public required string Title { get; set; }

        [MinLength(NoteBodyMinLength)]
        [MaxLength(NoteBodyMaxLength)]
        public string? Body { get; set; }

        [Required]
        public Guid NotebookId { get; set; }
        public Guid? ParentFolderId { get; set; }
        public Guid? FolderId { get; set; }
    }
}
