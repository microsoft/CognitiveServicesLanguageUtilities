using CliTool.Configs;
using CliTool.Services.Configuration.Models;

namespace CliTool.Services.Configuration
{
    interface IConfigurationService
    {
        public BlobStorageConfigModel GetBlobConfigModel();

        public LocalStorageConfigModel GetLocalConfigModel();

        public MSReadConfigModel GetMSReadConfigModel();

        public StorageConfigModel GetStorageConfigModel();
    }
}
