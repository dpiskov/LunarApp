using LunarApp.Web.ViewModels.Note;

namespace LunarApp.Web.ViewModels.Folder
{
    public class FolderNotesViewModel
    {
        public required IEnumerable<FolderInfoViewModel> Folders { get; set; }
        public required IEnumerable<NoteInfoViewModel> Notes { get; set; }
    }
}
