using CliTool.Services.Configuration.Models;

namespace CliTool.Services.Configuration
{
    interface IConfigurationService
    {
        public StorageConfigModel GetSourceStorageConfigModel();

        public StorageConfigModel GetDestinationStorageConfigModel();

        public MSReadConfigModel GetMSReadConfigModel();
    }
}
