namespace FileFormatConverter.Core.Interfaces
{
    public interface IFileHandler
    {
        string ReadFile(string filePath);
        bool WriteFile(string filePath, string content);
    }
}
