namespace FileFormatConverter.Core.Interfaces.Services
{
    public interface IFileHandler
    {
        string ReadFileAsString(string filePath);
        bool WriteFileAsString(string filePath, string content);
    }
}
