using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;
using System;
using System.Collections.Generic;

namespace CustomTextCliUtils.AppController.Models.ServiceModels.Parser
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
