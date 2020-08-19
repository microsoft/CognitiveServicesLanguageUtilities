using CustomTextCliUtils.AppController.Models.Enums;
using System.Threading.Tasks;

namespace CustomTextCliUtils.AppController.ServiceControllers
{
    interface IParserServiceController
    {
        public Task ExtractText(StorageType sourceStorageType, StorageType destinationStorageType, ChunkMethod chunkType);
    }
}
