using Microsoft.CogSLanguageUtilities.Definitions.Models.Enums.Chunker;
using Microsoft.CogSLanguageUtilities.Definitions.Models.Enums.Prediction;
using Microsoft.CogSLanguageUtilities.Definitions.Models.Enums.Storage;
using System.Threading.Tasks;

namespace Microsoft.CogSLanguageUtilities.Definitions.APIs.Controllers
{
    public interface IPredictionController
    {
        public Task Predict(StorageType sourceStorageType, StorageType destinationStorageType, ChunkMethod chunkType, CognitiveServiceType service);
    }
}
