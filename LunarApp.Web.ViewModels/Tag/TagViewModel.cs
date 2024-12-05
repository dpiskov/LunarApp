namespace LunarApp.Web.ViewModels.Tag
{
    public class TagViewModel
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public Guid? NotebookId { get; set; }
        public Guid? ParentFolderId { get; set; }
        public Guid? FolderId { get; set; }
        public Guid? NoteId { get; set; }
    }
}
