using CliTool.Services.Configuration;
using CliTool.Services.Configuration.Models;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace CliTool.Services.Parser
{
    class MSReadParserService : IParserService
    {
        ComputerVisionClient _client;

        public MSReadParserService(string cognitiveServiceEndPoint, string congnitiveServiceKey) {
            _client = new ComputerVisionClient(new ApiKeyServiceClientCredentials(congnitiveServiceKey))
            { Endpoint = cognitiveServiceEndPoint };
        }

        public async Task<string> ExtractText(Stream file)
        {
            var response = await _client.BatchReadFileInStreamAsync(file);
            const int numberOfCharsInOperationId = 36;
            string operationId = response.OperationLocation.Substring(response.OperationLocation.Length - numberOfCharsInOperationId);

            ReadOperationResult result;
            do
            {
                result = await _client.GetReadOperationResultAsync(operationId);
            }
            while ((result.Status == TextOperationStatusCodes.Running ||
                result.Status == TextOperationStatusCodes.NotStarted));
            StringBuilder finalText = new StringBuilder();
            foreach (TextRecognitionResult rr in result.RecognitionResults)
            {
                foreach (Line l in rr.Lines)
                {
                    finalText.AppendFormat($"{l.Text} ");
                }
            }
            return finalText.ToString();
        }
    }
}
