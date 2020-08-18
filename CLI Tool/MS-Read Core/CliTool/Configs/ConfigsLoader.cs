using CliTool.AppController.Exceptions.Config;
using CliTool.Configs;
using CliTool.Configs.Consts;
using CliTool.Services.Configuration.Models;
using Newtonsoft.Json;
using System.IO;

namespace CliTool.Services.Configuration
{
    class ConfigsLoader : IConfigsLoader
    {
        readonly ConfigModel _configModel;

        public ConfigsLoader() {
            var filePath = Path.Combine(Constants.ConfigsFileLocalDirectory, Constants.ConfigsFileName);
            if (File.Exists(filePath))
            {
                var configsFile = File.ReadAllText(filePath);
                _configModel = JsonConvert.DeserializeObject<ConfigModel>(configsFile);
            }
            else
            {
                throw new MissingConfigsException();
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
