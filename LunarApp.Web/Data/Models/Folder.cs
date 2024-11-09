﻿using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

using static LunarApp.Web.Common.ValidationConstants;

namespace LunarApp.Web.Data.Models
{
    public class Folder
    {
        [Key]
        [Comment("Folder Identifier")]
        public Guid Id { get; set; } = Guid.NewGuid();
        [Required]
        [MaxLength(FolderTitleMaxLength)]
        [Comment("Folder title")]
        public required string Title { get; set; }
        [Required]
        [Comment("Identifier of a notebook")]
        public Guid NotebookId { get; set; }
        [ForeignKey(nameof(NotebookId))]
        public required Notebook Notebook { get; set; }
        [Comment("Identifier of a parent folder")]
        public Guid? ParentFolderId { get; set; }
        [ForeignKey(nameof(ParentFolderId))]
        public Folder? ParentFolder { get; set; }

        public ICollection<Folder> ChildrenFolders { get; set; } = new List<Folder>();
        public ICollection<Note> Notes { get; set; } = new List<Note>();
    }
}