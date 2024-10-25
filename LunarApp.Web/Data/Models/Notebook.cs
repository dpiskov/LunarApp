﻿using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using static LunarApp.Web.Common.ValidationConstants;

namespace LunarApp.Web.Data.Models
{
    public class Notebook
    {
        [Required]
        [Comment("Notebook Identifier")]
        public Guid Id { get; set; }
        [Required]
        [MaxLength(NotebookTitleMaxLength)]
        [Comment("Notebook title")]
        public required string Title { get; set; }
        public ICollection<Folder> Folders { get; set; }
        public ICollection<Note> Notes { get; set; }
    }
}