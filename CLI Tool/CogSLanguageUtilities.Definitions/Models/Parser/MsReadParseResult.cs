using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;
using System.Collections.Generic;

namespace Microsoft.CogSLanguageUtilities.Definitions.Models.Parser
{
    public class MsReadParseResult : ParseResult
    {
        public IList<TextRecognitionResult> RecognitionResults { get; set; }

        public MsReadParseResult(IList<TextRecognitionResult> recognitionResults)
        {
            RecognitionResults = recognitionResults;
        }
    }
}
