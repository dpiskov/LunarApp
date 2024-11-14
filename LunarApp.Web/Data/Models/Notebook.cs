using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using static LunarApp.Web.Common.ValidationConstants;

namespace LunarApp.Web.Data.Models
{
    public class Notebook
    {
        [Key]
        [Comment("Notebook Identifier")]
        public Guid Id { get; set; }
        [Required]
        [MaxLength(NotebookTitleMaxLength)]
        [Comment("Notebook title")]
        public required string Title { get; set; }
        [MaxLength(NotebookDescriptionMaxLength)]
        [Comment("Notebook description")]
        public string? Description { get; set; }
        public ICollection<Folder> Folders { get; set; } = new List<Folder>();
        public ICollection<Note> Notes { get; set; } = new List<Note>();
    }
}