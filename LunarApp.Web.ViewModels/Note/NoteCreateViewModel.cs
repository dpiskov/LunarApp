using System.ComponentModel.DataAnnotations;
using static LunarApp.Common.ValidationConstants.Note;
using static LunarApp.Common.EntityValidationMessages.Note;

namespace LunarApp.Web.ViewModels.Note
{
    public class NoteCreateViewModel
    {
        [Required(ErrorMessage = NoteTitleRequiredMessage)]
        [MinLength(NoteTitleMinLength)]
        [MaxLength(NoteTitleMaxLength)]
        public required string Title { get; set; }

        //[MinLength(NoteBodyMinLength)]
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
