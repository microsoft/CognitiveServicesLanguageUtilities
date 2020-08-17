namespace CliTool.Exceptions.Storage
{
    class FolderNotFoundException : CliException
    {
        public FolderNotFoundException(string folderPath)
        {
            CustomMessage = "Directory Not Found: " + folderPath;
        }
    }
}
