﻿using System.ComponentModel.DataAnnotations;
using static LunarApp.Common.ValidationConstants.Notebook;
using static LunarApp.Common.EntityValidationMessages.Notebook;

namespace LunarApp.Web.ViewModels.Notebook
{
    public class NotebookCreateViewModel
    {
        [Required(ErrorMessage = NotebookTitleRequiredMessage)]
        [StringLength(NotebookTitleMaxLength, MinimumLength = NotebookTitleMinLength)]
        public required string Title { get; set; }
    }
}