using System.ComponentModel.DataAnnotations;
using LunarApp.Web.ViewModels.Tag;
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
