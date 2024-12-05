using System.ComponentModel.DataAnnotations;

namespace LunarApp.Web.ViewModels.Tag
{
    public class TagRemoveViewModel
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public Guid NotebookId { get; set; }
        public Guid? ParentFolderId { get; set; }
        public Guid? FolderId { get; set; }
        public Guid NoteId { get; set; }
    }
}
