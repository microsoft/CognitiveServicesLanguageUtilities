using Microsoft.CustomTextCliUtils.ApplicationLayer.Modeling.Models.Prediction.CustomTextResponse;

namespace Microsoft.CustomTextCliUtils.ApplicationLayer.Services.Prediction
{
    public interface IPredictionService
    {
        public CustomTextPredictionResponse GetPrediction(string query);
    }
}
