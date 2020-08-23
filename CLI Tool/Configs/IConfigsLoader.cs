

using CustomTextCliUtils.AppController.Models;
using CustomTextCliUtils.AppController.Models.Chunker;
using CustomTextCliUtils.AppController.Models.Parser;
using CustomTextCliUtils.AppController.Models.Storage;

namespace CustomTextCliUtils.Configs
{
    interface IConfigsLoader
    {
        public BlobStorageConfigModel GetBlobConfigModel();

        public LocalStorageConfigModel GetLocalConfigModel();

        public MSReadConfigModel GetMSReadConfigModel();

        public StorageConfigModel GetStorageConfigModel();
        public ChunkerConfigModel GetChunkerConfigModel();
    }
}
