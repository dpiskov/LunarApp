﻿using System.ComponentModel.DataAnnotations;

namespace LunarApp.Web.Models
{
    public class NotebookEditViewModel
    {
        public Guid Id { get; set; }
        public required string Title { get; set; }

    }
}
