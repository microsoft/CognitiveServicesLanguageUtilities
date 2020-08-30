using Newtonsoft.Json.Converters;
using System.Text.Json.Serialization;

namespace  Microsoft.CustomTextCliUtils.ApplicationLayer.Modeling.Enums.Prediction
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
