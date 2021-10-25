using Microsoft.CogSLanguageUtilities.Definitions.Exceptions;

namespace Microsoft.CognitiveSearchIntegration.Definitions.Exceptions.Serialization
{
    public class JsonSerializationException : CliException
    {
        public JsonSerializationException(string fileName) : base(ConstructMessage(fileName))
        { }

        private static string ConstructMessage(string fileName)
        {
            return $"Invalid Json File: {fileName}";
        }
    }
}
