using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;
using System;
using System.Collections.Generic;

namespace CustomTextCliUtils.ApplicationLayer.Modeling.Models.Parser
{
    class MsReadParseResult : ParseResult
    {
        public IList<TextRecognitionResult> RecognitionResults { get; set; }

        public MsReadParseResult(IList<TextRecognitionResult> recognitionResults)
        {
            RecognitionResults = recognitionResults;
        }
    }
}
