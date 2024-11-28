using System.ComponentModel.DataAnnotations;

using static LunarApp.Common.ValidationConstants.Note;
using static LunarApp.Common.EntityValidationMessages.Note;

namespace LunarApp.Web.Models
{
    public class NoteEditViewModel
    {
        public Guid Id { get; set; }
        [Required(ErrorMessage = NoteTitleRequiredMessage)]
        [StringLength(NoteTitleMaxLength, MinimumLength = NoteTitleMinLength)]
        public required string Title { get; set; }
        [MaxLength(NoteBodyMaxLength)]
        public string? Body { get; set; }
        [Required]
        public Guid NotebookId { get; set; }
        public Guid? ParentFolderId { get; set; }
        public Guid? FolderId { get; set; }
        public DateTime DateCreated { get; set; } = DateTime.Now;
        public DateTime LastSaved { get; set; } = DateTime.Now;
    }
}
