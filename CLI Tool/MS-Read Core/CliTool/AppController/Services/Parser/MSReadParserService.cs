using CliTool.Configs.Consts;
using CliTool.Configs.Models.Enums;
using CliTool.Exceptions.Parser;
using CliTool.Services.Logger;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace CliTool.Services.Parser
{
    class MSReadParserService : IParserService
    {
        ComputerVisionClient _client;
        HashSet<string> _validTypesSet;
        ILoggerService _loggerService;

        public MSReadParserService(ILoggerService loggerService, string cognitiveServiceEndPoint, string congnitiveServiceKey) {
            _loggerService = loggerService;
            _client = new ComputerVisionClient(new ApiKeyServiceClientCredentials(congnitiveServiceKey))
            { Endpoint = cognitiveServiceEndPoint };
            Task.Run(() => this.TestConnectionAsync(cognitiveServiceEndPoint, congnitiveServiceKey)).Wait();
            _validTypesSet = new HashSet<string>(Constants.ValidTypes, StringComparer.OrdinalIgnoreCase);
        }

        private async Task TestConnectionAsync(string cognitiveServiceEndPoint, string congnitiveServiceKey)
        {
            _loggerService.Log("testing connection to " + _client.Endpoint);
            try
            {
                var file = new MemoryStream();
                var response = await _client.BatchReadFileInStreamAsync(file);
            }
            catch (ComputerVisionErrorException e)
            {
                if (e.Message.Contains("Unauthorized"))
                {
                    throw new MsReadUnauthorizedException(congnitiveServiceKey, cognitiveServiceEndPoint);
                }
            }
        }

        public async Task<string> ExtractText(Stream file, string fileName)
        {
            _loggerService.LogOperation(OperationType.ParsingFile, fileName + " using MsRead service");
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
                    finalText.Append($"{l.Text} ");
                }
            }
            return finalText.ToString();
        }

        public void ValidateFileType(string fileName)
        {
            if (!_validTypesSet.Contains(Path.GetExtension(fileName)))
            {
                throw new UnsupportedFileTypeException(fileName, Path.GetExtension(fileName));
            }
        }
    }
}
