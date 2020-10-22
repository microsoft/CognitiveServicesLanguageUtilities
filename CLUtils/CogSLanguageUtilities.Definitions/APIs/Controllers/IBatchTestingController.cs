using Microsoft.CogSLanguageUtilities.Definitions.Models.Enums.Storage;
using System.Threading.Tasks;

namespace Microsoft.CogSLanguageUtilities.Definitions.APIs.Controllers
{
    public interface IBatchTestingController
    {
        Task EvaluateCustomTextAppAsync(StorageType sourceStorageType, StorageType destinationStorageType);
    }
}