using CliTool.Configs;
using CliTool.Services.Configuration.Models;
using Newtonsoft.Json;
using System.IO;

namespace CliTool.Services.Configuration
{
    class ConfigurationService : IConfigurationService
    {
        readonly ConfigModel _configModel;
        public ConfigurationService() {
            var configsFile = File.ReadAllText(Constants.ConfigsFileDir);
            _configModel = JsonConvert.DeserializeObject<ConfigModel>(configsFile);
        }
        public StorageConfigModel GetSourceStorageConfigModel() {
            return new StorageConfigModel
            {
                StorageType = _configModel.SourceStorageConnectionType,
                Directory = _configModel.SourceStorageConnectionType == StorageType.Local ? _configModel.LocalSourceFolder : _configModel.BlobSourceContainer,
                ConnectionString = _configModel.BlobStorageConnectionString
            };
        }

        public StorageConfigModel GetDestinationStorageConfigModel()
        {
            return new StorageConfigModel
            {
                StorageType = _configModel.DestinationStorageConnectionType,
                Directory = _configModel.DestinationStorageConnectionType == StorageType.Local ? _configModel.LocalDestinationFolder : _configModel.BlobDestinationContainer,
                ConnectionString = _configModel.BlobStorageConnectionString
            };
        }

        public MSReadConfigModel GetMSReadConfigModel()
        {
            return new MSReadConfigModel
            {
                CognitiveServiceEndPoint = _configModel.CognitiveServiceEndPoint,
                CongnitiveServiceKey = _configModel.CongnitiveServiceKey
            };
        }

    }
}
