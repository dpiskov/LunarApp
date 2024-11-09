using System.ComponentModel.DataAnnotations;

using static LunarApp.Web.Common.ValidationConstants;

namespace LunarApp.Web.Models
{
    public class FolderViewModel
    {
        [Required]
        [StringLength(FolderTitleMaxLength, MinimumLength = FolderTitleMinLength)]
        public string Title { get; set; }

        [Required]
        public Guid NotebookId { get; set; }
        public Guid? ParentFolderId { get; set; }
    }
}