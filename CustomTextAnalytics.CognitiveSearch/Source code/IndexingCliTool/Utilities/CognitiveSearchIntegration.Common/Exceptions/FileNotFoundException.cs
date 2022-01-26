namespace CognitiveSearchIntegration.Common.Exceptions
{
    public class FileNotFoundException : CliException
    {
        public FileNotFoundException(string filePath) : base(ConstructMessage(filePath))
        { }

        private static string ConstructMessage(string filePath)
        {
            return $"file not found : {filePath}";
        }
    }
}
