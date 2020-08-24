using Newtonsoft.Json.Converters;
using System.Text.Json.Serialization;

namespace CustomTextCliUtils.ApplicationLayer.Modeling.Enums.Prediction
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum CustomTextPredictionResponseStatus
    {
        Succeeded,
        NotStarted,
        Running,
        Failed,
        Unknown
    }
}
