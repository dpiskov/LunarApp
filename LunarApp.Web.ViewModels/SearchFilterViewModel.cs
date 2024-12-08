using LunarApp.Web.ViewModels.Folder;
using LunarApp.Web.ViewModels.Notebook;

namespace LunarApp.Web.ViewModels
{
    public class SearchFilterViewModel
    {
        public IEnumerable<NotebookInfoViewModel>? Notebooks { get; set; }

        public FolderNotesViewModel? FolderNotes { get; set; }

        public string? SearchQuery { get; set; }
        public string? TagFilter { get; set; }
        public IEnumerable<string>? AllTags { get; set; }

    }
}
