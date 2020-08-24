using CustomTextCliUtils.ApplicationLayer.Modeling.Models.Prediction;
using System.Threading.Tasks;

namespace CustomTextCliUtils.ApplicationLayer.Services.Prediction
{
    interface IPredictionService
    {
        public Task<CustomTextPredictionResponse> PredictAsync(string query);
    }
}
