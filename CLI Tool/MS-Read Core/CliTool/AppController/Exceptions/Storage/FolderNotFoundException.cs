namespace CliTool.Exceptions.Storage
{
    class FolderNotFoundException : CliException
    {
        public FolderNotFoundException(string folderPath)
            : base(ConstructMessage(folderPath))
        { }

        public static string ConstructMessage(string folderPath)
        {
            return "Directory Not Found: " + folderPath;
        }
    }
}
