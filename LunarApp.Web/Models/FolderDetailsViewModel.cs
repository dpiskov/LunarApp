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
        public Guid? ParentFolderId { get; set; }
        public Guid FolderId { get; set; }
        public bool IsClickedDirectlyFromNotebook { get; set; }
    }
}
