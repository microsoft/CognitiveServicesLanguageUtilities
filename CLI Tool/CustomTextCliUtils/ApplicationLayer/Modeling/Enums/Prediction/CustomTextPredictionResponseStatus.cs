using Newtonsoft.Json.Converters;
using System.Text.Json.Serialization;

namespace  Microsoft.CustomTextCliUtils.ApplicationLayer.Modeling.Enums.Prediction
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum CustomTextPredictionResponseStatus
    {
        succeeded,
        notstarted,
        running,
        failed,
        unknown
    }
}
