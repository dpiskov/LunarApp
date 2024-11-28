using System.ComponentModel.DataAnnotations;
using static LunarApp.Common.ValidationConstants.Notebook;

namespace LunarApp.Web.ViewModels.Notebook
{
    public class NotebookDetailsViewModel
    {
        public Guid Id { get; set; }
        [MaxLength(NotebookDescriptionMaxLength)]
        public string? Description { get; set; }
    }
}
