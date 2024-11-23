using System.ComponentModel.DataAnnotations;
using static LunarApp.Web.Common.ValidationConstants;

namespace LunarApp.Web.Models
{
    public class FolderCreateViewModel
    {
        [Required]
        [StringLength(FolderTitleMaxLength, MinimumLength = FolderTitleMinLength)]
        public required string Title { get; set; }

        [Required]
        public Guid NotebookId { get; set; }
        public Guid? ParentFolderId { get; set; }
        public Guid? FolderId { get; set; }
        public bool IsMadeDirectlyFromNotebook { get; set; }
    }
}