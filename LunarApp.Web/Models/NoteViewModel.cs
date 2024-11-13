using static LunarApp.Web.Common.ValidationConstants;
using System.ComponentModel.DataAnnotations;

namespace LunarApp.Web.Models
{
    public class NoteViewModel
    {
        [Required]
        [MinLength(NoteTitleMinLength)]
        [MaxLength(NoteTitleMaxLength)]
        public string Title { get; set; }

        [MinLength(NoteBodyMinLength)]
        [MaxLength(NoteBodyMaxLength)]
        public string? Body { get; set; }

        [Required]
        public Guid NotebookId { get; set; }

        //[Required]
        public Guid? FolderId { get; set; }
    }
}
