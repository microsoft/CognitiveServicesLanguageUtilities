namespace Microsoft.CogSLanguageUtilities.Definitions.Exceptions.Storage
{
    public class FolderNotFoundException : CliException
    {
        public FolderNotFoundException(string folderPath)
            : base(ConstructMessage(folderPath))
        { }

        public static string ConstructMessage(string folderPath)
        {
            return $"Directory Not Found: {folderPath}";
        }
    }
}
