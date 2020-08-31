using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;
using System;
using System.Collections.Generic;

namespace Microsoft.CustomTextCliUtils.ApplicationLayer.Modeling.Models.Parser
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
