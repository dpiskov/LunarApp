namespace LunarApp.Web.Models
{
    public class FolderNotesViewModel
    {
        public IEnumerable<FolderInfoViewModel> Folders { get; set; }
        public IEnumerable<NoteInfoViewModel> Notes { get; set; }
    }
}
