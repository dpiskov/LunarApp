using System.ComponentModel.DataAnnotations;
using static LunarApp.Common.ValidationConstants.Note;
using static LunarApp.Common.EntityValidationMessages.Note;
using LunarApp.Web.ViewModels.Tag;

namespace LunarApp.Web.ViewModels.Note
{
    public class NoteEditViewModel
    {
        public Guid Id { get; set; }
        [Required(ErrorMessage = NoteTitleRequiredMessage)]
        [StringLength(NoteTitleMaxLength, MinimumLength = NoteTitleMinLength)]
        public required string Title { get; set; }
        [MaxLength(NoteBodyMaxLength)]
        public string? Body { get; set; }
        public Guid? SelectedTagId { get; set; }
        public IEnumerable<TagViewModel> Tags { get; set; } = new List<TagViewModel>();

        [Required]
        public Guid NotebookId { get; set; }
        public Guid? ParentFolderId { get; set; }
        public Guid? FolderId { get; set; }

        public DateTime DateCreated { get; set; } = DateTime.Now;
        public DateTime LastSaved { get; set; } = DateTime.Now;
    }
}
