using System.ComponentModel.DataAnnotations;
using static LunarApp.Common.ValidationConstants.Folder;

namespace LunarApp.Web.ViewModels.Folder
{
    public class FolderDetailsViewModel
    {
        [Required]
        public required string Title { get; set; }
        [MaxLength(FolderDescriptionMaxLength)]
        public string? Description { get; set; }
        [Required]
        public Guid NotebookId { get; set; }
        public Guid? ParentFolderId { get; set; }
        public Guid FolderId { get; set; }
        public bool IsClickedDirectlyFromNotebook { get; set; }
    }
}
