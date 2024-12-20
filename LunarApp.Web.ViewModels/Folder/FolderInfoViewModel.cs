﻿namespace LunarApp.Web.ViewModels.Folder
{
    public class FolderInfoViewModel
    {
        public Guid Id { get; set; }
        public required string Title { get; set; }
        public Guid NotebookId { get; set; }
        public Guid? ParentFolderId { get; set; }
    }
}
