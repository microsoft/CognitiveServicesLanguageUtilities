
using CustomTextCliUtils.ApplicationLayer.Exceptions.Configs;
using CustomTextCliUtils.ApplicationLayer.Modeling.Models.Configs;
using CustomTextCliUtils.Configs.Consts;
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

        public ChunkerConfigModel GetChunkerConfigModel()
        {
            return _configModel.Chunker;
        }

        public LocalStorageConfigModel GetLocalConfigModel()
        {
            return _configModel.Storage.Local;
        }

        public MSReadConfigModel GetMSReadConfigModel()
        {
            return _configModel.Parser.MsRead;
        }

        public PredictionConfigModel GetPredictionConfigModel()
        {
            return _configModel.Prediction;
        }

        public StorageConfigModel GetStorageConfigModel()
        {
            return _configModel.Storage;
        }
    }
}
