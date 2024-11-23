using System.ComponentModel.DataAnnotations;

namespace LunarApp.Web.Models
{
    public class FolderDeleteViewModel
    {
        public required string Title { get; set; }
        [Required]
        public Guid NotebookId { get; set; }
        public Guid? ParentFolderId { get; set; }
        public Guid FolderId { get; set; }
    }
}
