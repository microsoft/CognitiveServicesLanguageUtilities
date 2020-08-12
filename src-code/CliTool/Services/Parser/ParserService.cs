using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace CliTool.Services.Parser
{
    class ParserService : IParserService
    {
        static ComputerVisionClient _client;

        public ParserService(string cognitiveServiceEndPoint, string congnitiveServiceKey) {
            _client = new ComputerVisionClient(new ApiKeyServiceClientCredentials(congnitiveServiceKey))
            { Endpoint = cognitiveServiceEndPoint };
        }

        public async Task<string> ExtractText(FileStream file)
        {
            var response = await _client.BatchReadFileInStreamAsync(file);
            const int numberOfCharsInOperationId = 36;
            string operationId = response.OperationLocation.Substring(response.OperationLocation.Length - numberOfCharsInOperationId);

            ReadOperationResult result;
            do
            {
                result = await _client.getresu(Guid.Parse(operationId));
            }
            while ((result.Status == OperationStatusCodes.Running ||
                result.Status == OperationStatusCodes.NotStarted));
            StringBuilder finalText = new StringBuilder();
            foreach (ReadResult rr in result.AnalyzeResult.ReadResults)
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
