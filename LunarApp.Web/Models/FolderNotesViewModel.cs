namespace LunarApp.Web.Models
{
    public class FolderNotesViewModel
    {
        public required IEnumerable<FolderInfoViewModel> Folders { get; set; }
        public required IEnumerable<NoteInfoViewModel> Notes { get; set; }
    }
}
