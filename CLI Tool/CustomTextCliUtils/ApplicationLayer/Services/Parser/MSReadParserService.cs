using Microsoft.CustomTextCliUtils.Configs.Consts;
using Microsoft.CustomTextCliUtils.ApplicationLayer.Exceptions.Parser;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.CustomTextCliUtils.ApplicationLayer.Modeling.Models.Parser;

namespace Microsoft.CustomTextCliUtils.ApplicationLayer.Services.Parser
{
    public class MSReadParserService : IParserService
    {
        private ComputerVisionClient _client;
        private HashSet<string> _validTypesSet;

        public MSReadParserService(string cognitiveServiceEndPoint, string congnitiveServiceKey)
        {
            _client = new ComputerVisionClient(new ApiKeyServiceClientCredentials(congnitiveServiceKey))
            { Endpoint = cognitiveServiceEndPoint };
            TestConnectionAsync(cognitiveServiceEndPoint, congnitiveServiceKey);
            _validTypesSet = new HashSet<string>(Constants.MsReadValidFileTypes, StringComparer.OrdinalIgnoreCase);
        }

        private void TestConnectionAsync(string cognitiveServiceEndPoint, string congnitiveServiceKey)
        {
            try
            {
                var file = new MemoryStream();
                var response = _client.BatchReadFileInStreamAsync(file).ConfigureAwait(false).GetAwaiter().GetResult();
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
            const int NumberOfCharsInOperationId = 36;
            string operationId = response.OperationLocation.Substring(response.OperationLocation.Length - NumberOfCharsInOperationId);

            ReadOperationResult result;
            do
            {
                result = await _client.GetReadOperationResultAsync(operationId);
            }
            while (result.Status == TextOperationStatusCodes.Running || result.Status == TextOperationStatusCodes.NotStarted);
            return new MsReadParseResult(result.RecognitionResults);
        }

        public void ValidateFileType(string fileName)
        {
            if (!_validTypesSet.Contains(Path.GetExtension(fileName)))
            {
                throw new UnsupportedFileTypeException(fileName, Path.GetExtension(fileName), Constants.MsReadValidFileTypes);
            }
        }
    }
}
