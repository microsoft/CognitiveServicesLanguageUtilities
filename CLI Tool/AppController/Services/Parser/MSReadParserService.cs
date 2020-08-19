using CustomTextCliUtils.Configs.Consts;
using CustomTextCliUtils.AppController.Exceptions.Parser;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using CustomTextCliUtils.AppController.Services.Chunker;

namespace CustomTextCliUtils.AppController.Services.Parser
{
    class MSReadParserService : IParserService
    {
        ComputerVisionClient _client;
        HashSet<string> _validTypesSet;

        public MSReadParserService(string cognitiveServiceEndPoint, string congnitiveServiceKey) {
            _client = new ComputerVisionClient(new ApiKeyServiceClientCredentials(congnitiveServiceKey))
            { Endpoint = cognitiveServiceEndPoint };
            Task.Run(() => this.TestConnectionAsync(cognitiveServiceEndPoint, congnitiveServiceKey)).Wait();
            _validTypesSet = new HashSet<string>(Constants.ValidTypes, StringComparer.OrdinalIgnoreCase);
        }

        private async Task TestConnectionAsync(string cognitiveServiceEndPoint, string congnitiveServiceKey)
        {
            try
            {
                var file = new MemoryStream();
                var response = await _client.BatchReadFileInStreamAsync(file);
            }
            catch (ComputerVisionErrorException e)
            {
                if (!e.Message.Contains("BadRequest"))
                {
                    throw new MsReadConnectionException(e.Message, congnitiveServiceKey, cognitiveServiceEndPoint);
                }
            }
        }

        public async Task<ParseResult> ParseFile(Stream file)
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
            return new MsReadParseResult(result.RecognitionResults);
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
