using LunarApp.Web.Common;
using System.ComponentModel.DataAnnotations;

namespace LunarApp.Web.Models
{
    public class NoteCreateViewModel
    {
        [Required]
        [MaxLength(ValidationConstants.NoteTitleMaxLength)]
        public string Title { get; set; } = string.Empty;

        public string Body { get; set; } = string.Empty;

        [Required]
        public Guid NotebookId { get; set; }

        [Required]
        public Guid FolderId { get; set; }
    }
}
