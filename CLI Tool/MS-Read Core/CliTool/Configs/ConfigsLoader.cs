using CustomTextCliUtils.AppController.Exceptions.Config;
using CustomTextCliUtils.Configs.Consts;
using CustomTextCliUtils.Configs.Models;
using CustomTextCliUtils.Configs.Models.Parser;
using CustomTextCliUtils.Configs.Models.Storage;
using Newtonsoft.Json;
using System.IO;

namespace CustomTextCliUtils.Configs
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
