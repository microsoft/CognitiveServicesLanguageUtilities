using Microsoft.CogSLanguageUtilities.Definitions.Models.CustomText.Api.Prediction.Response.Result;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Microsoft.CogSLanguageUtilities.Definitions.APIs.Services
{
    public interface ICustomTextPredictionService
    {
        public Task<CustomTextPredictionResponse> GetPredictionAsync(string query);
        public Task<List<CustomTextPredictionResponse>> GetPredictionBatchAsync(List<string> queries);
    }
}
