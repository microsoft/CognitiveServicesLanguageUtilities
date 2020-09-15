using Microsoft.CogSLanguageUtilities.Definitions.Models.CustomText.PredictionApi.Response.Result;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Microsoft.CogSLanguageUtilities.Definitions.APIs.Services
{
    public interface ICustomTextService
    {
        public Task<CustomTextPredictionResponse> GetPredictionAsync(string query);
        public Task<List<CustomTextPredictionResponse>> GetPredictionBatchAsync(List<string> queries);
    }
}
