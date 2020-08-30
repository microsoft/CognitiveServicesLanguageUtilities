using Microsoft.CustomTextCliUtils.ApplicationLayer.Modeling.Models.Prediction;
using System.Threading.Tasks;

namespace  Microsoft.CustomTextCliUtils.ApplicationLayer.Services.Prediction
{
    public interface IPredictionService
    {
        public CustomTextPredictionResponse GetPrediction(string query);
    }
}
