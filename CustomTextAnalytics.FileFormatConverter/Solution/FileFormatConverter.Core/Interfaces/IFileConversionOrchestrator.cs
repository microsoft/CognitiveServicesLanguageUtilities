namespace FileFormatConverter.Core.Interfaces
{
    public interface IFileConversionOrchestrator<TSourceModel, TTargetModel>
    {
        void ConvertFile(string inputFilePath, string targetFilePath);
    }
}
