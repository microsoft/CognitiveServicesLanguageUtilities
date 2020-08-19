

using CustomTextCliUtils.Configs.Models;
using CustomTextCliUtils.Configs.Models.Parser;
using CustomTextCliUtils.Configs.Models.Storage;

namespace CustomTextCliUtils.Configs
{
    interface IConfigsLoader
    {
        public BlobStorageConfigModel GetBlobConfigModel();

        public LocalStorageConfigModel GetLocalConfigModel();

        public MSReadConfigModel GetMSReadConfigModel();

        public StorageConfigModel GetStorageConfigModel();
    }
}
