namespace FileFormatConverter.Core.Interfaces
{
    public interface IFileHandler
    {
        string ReadFileAsString(string filePath);
        bool WriteFileAsString(string filePath, string content);
    }
}
