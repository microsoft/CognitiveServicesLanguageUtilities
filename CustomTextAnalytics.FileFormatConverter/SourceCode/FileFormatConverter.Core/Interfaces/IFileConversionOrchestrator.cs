namespace FileFormatConverter.Core.Interfaces
{
    public interface IFileConversionOrchestrator
    {
        void ConvertFile(string inputFilePath, string targetFilePath, string language);
    }
}
