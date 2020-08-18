using CliTool.Configs;
using CliTool.Configs.Consts
    ;
using CliTool.Services.Configuration.Models;
using Newtonsoft.Json;
using System.IO;

namespace CliTool.Services.Configuration
{
    class ConfigurationService : IConfigurationService
    {
        readonly ConfigModel _configModel;
        public ConfigurationService() {
            var filePath = Path.Combine(Constants.ConfigsFileLocalDirectory, Constants.ConfigsFileName);
            if (File.Exists(filePath))
            {
                var configsFile = File.ReadAllText(filePath);
                _configModel = JsonConvert.DeserializeObject<ConfigModel>(configsFile);
            }
            else
            {
                var configModel = new ConfigModel();
                var configFile = JsonConvert.SerializeObject(configModel);
                File.WriteAllText(filePath, configFile);
            }
        }

        public BlobStorageConfigModel GetBlobConfigModel()
        {
            return _configModel.Storage.Blob;
        }

        public LocalStorageConfigModel GetLocalConfigModel()
        {
            return _configModel.Storage.Local;
        }

        public MSReadConfigModel GetMSReadConfigModel()
        {
            return _configModel.Parser.MsRead;
        }

        public StorageConfigModel GetStorageConfigModel()
        {
            return _configModel.Storage;
        }
    }
}
