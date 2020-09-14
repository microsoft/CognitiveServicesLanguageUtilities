using Microsoft.CustomTextCliUtils.ApplicationLayer.Modeling.Models.Chunker;
using Microsoft.CustomTextCliUtils.ApplicationLayer.Modeling.Models.Prediction.CustomTextResponse;
using Newtonsoft.Json;

namespace Microsoft.CustomTextCliUtils.ApplicationLayer.Modeling.Models.Prediction
{
    public class CustomTextPredictionChunkInfo
    {
        public ChunkInfo ChunkInfo;
        public CustomTextPredictionResponse CustomTextPredictionResponse { get; set; }
    }
}
