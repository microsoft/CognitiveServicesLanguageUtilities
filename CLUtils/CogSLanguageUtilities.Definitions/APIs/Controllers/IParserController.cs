using Microsoft.CogSLanguageUtilities.Definitions.Models.Enums.Chunker;
using Microsoft.CogSLanguageUtilities.Definitions.Models.Enums.Storage;
using System.Threading.Tasks;

namespace Microsoft.CogSLanguageUtilities.Definitions.APIs.Controllers
{
    public interface IParserController
    {
        Task ExtractText(StorageType sourceStorageType, StorageType destinationStorageType, ChunkMethod chunkType);
    }
}