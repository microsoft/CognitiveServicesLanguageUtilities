using Microsoft.CustomTextCliUtils.ApplicationLayer.Modeling.Models.Prediction.CustomTextResponse;
using System.Threading.Tasks;

namespace Microsoft.CustomTextCliUtils.ApplicationLayer.Services.Prediction
{
    public interface IPredictionService
    {
        public Task<CustomTextPredictionResponse> GetPredictionAsync(string query);
    }
}
