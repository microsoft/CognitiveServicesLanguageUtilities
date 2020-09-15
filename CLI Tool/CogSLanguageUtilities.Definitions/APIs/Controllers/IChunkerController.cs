using Microsoft.CogSLanguageUtilities.Definitions.Models.Enums.Storage;
using System.Threading.Tasks;

namespace Microsoft.CogSLanguageUtilities.Definitions.APIs.Controllers
{
    public interface IChunkerController
    {
        Task ChunkTextAsync(StorageType sourceStorageType, StorageType destinationStorageType);
    }
}