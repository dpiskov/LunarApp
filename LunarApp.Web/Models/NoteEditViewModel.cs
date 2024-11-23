using System.ComponentModel.DataAnnotations;

namespace LunarApp.Web.Models
{
    public class NoteEditViewModel
    {
        public Guid Id { get; set; }
        public required string Title { get; set; }
        public string? Body { get; set; }
        [Required]
        public Guid NotebookId { get; set; }
        public Guid? ParentFolderId { get; set; }
        public Guid? FolderId { get; set; }
        public DateTime LastSaved { get; set; }
    }
}
