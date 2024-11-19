using System.ComponentModel.DataAnnotations;

using static LunarApp.Web.Common.ValidationConstants;

namespace LunarApp.Web.Models
{
    public class NotebookDetailsViewModel
    {
        public Guid Id { get; set; }
        [MaxLength(NotebookDescriptionMaxLength)]
        public string? Description { get; set; }
    }
}
