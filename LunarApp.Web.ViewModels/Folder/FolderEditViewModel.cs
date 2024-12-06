using System.ComponentModel.DataAnnotations;
using static LunarApp.Common.ValidationConstants.Folder;
using static LunarApp.Common.EntityValidationMessages.Folder;

namespace LunarApp.Web.ViewModels.Folder
{
    public class FolderEditViewModel
    {

        [Required(ErrorMessage = FolderTitleRequiredMessage)]
        [StringLength(FolderTitleMaxLength, MinimumLength = FolderTitleMinLength)]
        public required string Title { get; set; }

        [Required]
        public Guid NotebookId { get; set; }
        public Guid? ParentFolderId { get; set; }
        public Guid FolderId { get; set; }
        public bool IsAccessedDirectlyFromNotebook { get; set; }
    }
}
