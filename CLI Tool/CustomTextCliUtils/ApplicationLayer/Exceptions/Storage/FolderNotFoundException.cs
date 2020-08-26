using CustomTextCliUtils.ApplicationLayer.Modeling.Exceptions;

namespace CustomTextCliUtils.ApplicationLayer.Exceptions.Storage
{
    public class FolderNotFoundException : CliException
    {
        public FolderNotFoundException(string folderPath)
            : base(CliExceptionCode.FolderNotFound, ConstructMessage(folderPath))
        { }

        public static string ConstructMessage(string folderPath)
        {
            return "Directory Not Found: " + folderPath;
        }
    }
}
