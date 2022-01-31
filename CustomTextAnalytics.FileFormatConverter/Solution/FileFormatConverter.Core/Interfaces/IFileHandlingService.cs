namespace FileFormatConverter.Core.Interfaces
{
    public interface IFileHandlingService
    {
        string ReadFile(string filePath);
        bool WriteFile(string filePath, string content);
    }
}
