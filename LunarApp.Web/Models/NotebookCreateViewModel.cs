using System.ComponentModel.DataAnnotations;

using static LunarApp.Web.Common.ValidationConstants;

namespace LunarApp.Web.Models
{
    public class NotebookCreateViewModel
    {
        [Required]
        [StringLength(NotebookTitleMaxLength, MinimumLength = NotebookTitleMinLength)]
        public required string Title { get; set; }
    }
}
