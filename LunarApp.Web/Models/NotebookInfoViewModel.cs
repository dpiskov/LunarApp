using LunarApp.Web.Data.Models;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace LunarApp.Web.Models
{
    public class NotebookInfoViewModel
    {
        public Guid Id { get; set; }
        public required string Title { get; set; }
        public ICollection<Folder> Folders { get; set; } = new List<Folder>();
        public ICollection<Note> Notes { get; set; } = new List<Note>();

    }
}
