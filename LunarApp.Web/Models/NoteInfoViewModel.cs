namespace LunarApp.Web.Models
{
    public class NoteInfoViewModel
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string BodyPreview { get; set; }
        public Guid? FolderId { get; set; }
    }
}
