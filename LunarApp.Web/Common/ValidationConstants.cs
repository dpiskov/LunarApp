namespace LunarApp.Web.Common
{
    public static class ValidationConstants
    {
        // Note
        public const int NoteTitleMinLength = 1;
        public const int NoteTitleMaxLength = 100;

        public const int NoteBodyMinLength = 0;
        public const int NoteBodyMaxLength = 20_000;

        // Folder
        public const int FolderTitleMinLength = 1;
        public const int FolderTitleMaxLength = 50;

        // Notebook
        public const int NotebookTitleMinLength = 1;
        public const int NotebookTitleMaxLength = 50;

        // LastSaved date format
        public const string LastSavedDataFormat = "dd.MM.yyyy";
    }
}
