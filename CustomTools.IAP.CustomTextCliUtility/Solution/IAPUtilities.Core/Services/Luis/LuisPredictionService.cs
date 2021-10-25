using Microsoft.Azure.CognitiveServices.Language.LUIS.Runtime;
using Microsoft.Azure.CognitiveServices.Language.LUIS.Runtime.Models;
using Microsoft.IAPUtilities.Core.Helpers.Mappers.Luis;
using Microsoft.IAPUtilities.Definitions.APIs.Services;
using Microsoft.IAPUtilities.Definitions.Models.Luis;
using System;
using System.Threading.Tasks;

namespace Microsoft.IAPUtilities.Core.Services.Luis
{
    public class LuisPredictionService : ILuisPredictionService
    {
        LUISRuntimeClient _client;
        Guid _appId;

        public LuisPredictionService(string endpoint, string key, string appId)
        {
            _appId = Guid.Parse(appId);
            var credentials = new ApiKeyServiceClientCredentials(key);
            _client = new LUISRuntimeClient(credentials) { Endpoint = endpoint };
        }

        public async Task<CustomLuisResponse> Predict(string query)
        {
            var request = new PredictionRequest { Query = query };
            var response = await _client.Prediction.GetSlotPredictionAsync(_appId, "Production", request, verbose: true);
            return LuisOutputMapper.MapToCustomLuisRespone(response);
        }
    }
}
