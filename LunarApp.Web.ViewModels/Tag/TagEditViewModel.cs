using System.ComponentModel.DataAnnotations;

using static LunarApp.Common.ValidationConstants.Tag;


namespace LunarApp.Web.ViewModels.Tag
{
    public class TagEditViewModel
    {
        public Guid Id {get; set; }
        [Required]
        [StringLength(TagNameMaxLength, MinimumLength = TagNameMinLength)]
        public required string Name { get; set; }
        public Guid? NotebookId { get; set; }
        public Guid? ParentFolderId { get; set; }
        public Guid? FolderId { get; set; }
        public Guid? NoteId { get; set; }
    }
}
