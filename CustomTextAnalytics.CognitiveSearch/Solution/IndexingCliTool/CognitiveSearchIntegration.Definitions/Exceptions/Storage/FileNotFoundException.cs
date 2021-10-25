using Microsoft.CogSLanguageUtilities.Definitions.Exceptions;

namespace Microsoft.CognitiveSearchIntegration.Definitions.Exceptions.Storage
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
