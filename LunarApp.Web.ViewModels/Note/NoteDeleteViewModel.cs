﻿using System.ComponentModel.DataAnnotations;

namespace LunarApp.Web.ViewModels.Note
{
    public class NoteDeleteViewModel
    {
        public Guid Id { get; set; }
        public required string Title { get; set; }
        [Required]
        public Guid NotebookId { get; set; }
        public Guid? ParentFolderId { get; set; }
        public Guid? FolderId { get; set; }
    }
}
