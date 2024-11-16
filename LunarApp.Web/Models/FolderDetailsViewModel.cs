using System.ComponentModel.DataAnnotations;

using static LunarApp.Web.Common.ValidationConstants;

namespace LunarApp.Web.Models
{
    public class FolderDetailsViewModel
    {
        [MaxLength(FolderDescriptionMaxLength)]
        public string? Description { get; set; }
        [Required]
        public Guid NotebookId { get; set; }
        [Required]
        public Guid ParentFolderId { get; set; }
    }
}
