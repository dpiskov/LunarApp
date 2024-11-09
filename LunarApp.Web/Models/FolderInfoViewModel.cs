using LunarApp.Web.Data.Models;
using System.ComponentModel.DataAnnotations;

namespace LunarApp.Web.Models
{
    public class FolderInfoViewModel
    {
        public Guid Id { get; set; }
        public required string Title { get; set; }
        public Guid NotebookId { get; set; }
        public Guid? ParentFolderId { get; set; }

        public ICollection<Folder> ChildrenFolders { get; set; } = new List<Folder>();
        public ICollection<Note> Notes { get; set; } = new List<Note>();
    }
}
