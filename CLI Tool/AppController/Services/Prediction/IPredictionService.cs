using CustomTextCliUtils.AppController.Models.HttpModels.Prediction;
using System.Threading.Tasks;

namespace CustomTextCliUtils.AppController.Services.Prediction
{
    interface IPredictionService
    {
        public Task<CustomTextPredictionResponse> PredictAsync(string query);
    }
}
