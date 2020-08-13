using CliTool.Configs;
using CliTool.Services.Configuration.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
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

        public StorageConfigModel GetSourceStorageConfigModel()
        {
            return _configModel.Storage.Source;
        }

        public StorageConfigModel GetDestinationStorageConfigModel()
        {
            return _configModel.Storage.Destination;
        }

        public MSReadConfigModel GetMSReadConfigModel()
        {
            return _configModel.Parser.MsRead;
        }

    }
}
