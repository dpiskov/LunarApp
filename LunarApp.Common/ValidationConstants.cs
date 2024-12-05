namespace LunarApp.Common
{
    public static class ValidationConstants
    {
        public static class Tag
        {
            public const int TagNameMinLength = 1;
            public const int TagNameMaxLength = 30;
        }
        public static class Note
        {
            // Note
            public const int NoteTitleMinLength = 1;
            public const int NoteTitleMaxLength = 100;

            //public const int NoteBodyMinLength = 0;
            public const int NoteBodyMaxLength = 20_000;

            public const string DateFormat = "dd.MM.yyyy HH:mm";
        }

        public static class Folder
        {
            // Folder
            public const int FolderTitleMinLength = 1;
            public const int FolderTitleMaxLength = 50;

            public const int FolderDescriptionMaxLength = 20_000;
        }

        public static class Notebook
        {
            // Notebook
            public const int NotebookTitleMinLength = 1;
            public const int NotebookTitleMaxLength = 50;

            public const int NotebookDescriptionMaxLength = 20_000;
        }

        // LastSaved date format
        public const string LastSavedDataFormat = "dd.MM.yyyy";
    }
}
