using Microsoft.CustomTextCliUtils.ApplicationLayer.Modeling.Models.Configs;
using Microsoft.CustomTextCliUtils.ApplicationLayer.Services.Logger;
using Microsoft.CustomTextCliUtils.ApplicationLayer.Services.Storage;
using Microsoft.CustomTextCliUtils.Configs.Consts;
using Newtonsoft.Json;
using System;
using System.IO;

namespace Microsoft.CustomTextCliUtils.ApplicationLayer.Controllers
{
    public class ConfigServiceController
    {
        private readonly ILoggerService _loggerService;
        private readonly IStorageService _storageService;
        private ConfigModel _configModel;

        public ConfigServiceController(ILoggerService loggerService, IStorageService storageService)
        {
            _loggerService = loggerService;
            _storageService = storageService;
            var filePath = Path.Combine(Constants.ConfigsFileLocalDirectory, Constants.ConfigsFileName);
            if (File.Exists(filePath))
            {
                var configsFile = storageService.ReadFileAsString(Constants.ConfigsFileName);
                _configModel = JsonConvert.DeserializeObject<ConfigModel>(configsFile);
            }
            else
            {
                _configModel = new ConfigModel();
                StoreConfigsModel();
            }
        }

        private void StoreConfigsModel()
        {
            var configString = JsonConvert.SerializeObject(_configModel, Formatting.Indented);
            _storageService.StoreData(configString, Constants.ConfigsFileName);
        }

        public void SetChunkerConfigs(int? charLimit)
        {
            if (charLimit != null)
            {
                _configModel.Chunker.CharLimit = (int)charLimit;
            }
            StoreConfigsModel();
            _loggerService.Log("Updated Chunker configs");
        }

        public void SetMsReadConfigs(string cognitiveServicesKey, string endpointUrl)
        {
            if (!string.IsNullOrEmpty(cognitiveServicesKey))
            {
                _configModel.Parser.MsRead.CongnitiveServiceKey = cognitiveServicesKey;
            }
            if (!string.IsNullOrEmpty(endpointUrl))
            {
                _configModel.Parser.MsRead.CognitiveServiceEndPoint = endpointUrl;
            }
            StoreConfigsModel();
            _loggerService.Log("Updated MsRead configs");
        }

        public void SetBlobStorageConfigs(string connectionString, string sourceContainer, string destinationContainer)
        {
            if (!string.IsNullOrEmpty(connectionString))
            {
                _configModel.Storage.Blob.ConnectionString = connectionString;
            }
            if (!string.IsNullOrEmpty(sourceContainer))
            {
                _configModel.Storage.Blob.SourceContainer = sourceContainer;
            }
            if (!string.IsNullOrEmpty(destinationContainer))
            {
                _configModel.Storage.Blob.DestinationContainer = destinationContainer;
            }
            StoreConfigsModel();
            _loggerService.Log("Updated Blob Storage configs");
        }

        public void SetLocalStorageConfigs(string sourceDirectory, string destinationDirectory)
        {
            if (!string.IsNullOrEmpty(sourceDirectory))
            {
                _configModel.Storage.Local.SourceDirectory = sourceDirectory;
            }
            if (!string.IsNullOrEmpty(destinationDirectory))
            {
                _configModel.Storage.Local.DestinationDirectory = destinationDirectory;
            }
            StoreConfigsModel();
            _loggerService.Log("Updated Local Storage configs");
        }

        public void SetPredictionConfigs(string customTextKey, string endpointUrl, string appId, string versionId)
        {
            if (!string.IsNullOrEmpty(customTextKey))
            {
                _configModel.Prediction.CustomTextKey = customTextKey;
            }
            if (!string.IsNullOrEmpty(endpointUrl))
            {
                _configModel.Prediction.EndpointUrl = endpointUrl;
            }
            if (!string.IsNullOrEmpty(appId))
            {
                _configModel.Prediction.AppId = appId;
            }
            if (!string.IsNullOrEmpty(versionId))
            {
                _configModel.Prediction.VersionId = versionId;
            }
            StoreConfigsModel();
            _loggerService.Log("Updated Custom Text prediction configs");
        }

        public void ShowAllConfigs()
        {
            var configString = JsonConvert.SerializeObject(_configModel, Formatting.Indented);
            _loggerService.Log(configString);
        }

        public void ShowChunkerConfigs()
        {
            var configString = JsonConvert.SerializeObject(_configModel.Chunker, Formatting.Indented);
            _loggerService.Log(configString);
        }

        public void ShowParserConfigs()
        {
            var configString = JsonConvert.SerializeObject(_configModel.Parser, Formatting.Indented);
            _loggerService.Log(configString);
        }

        public void ShowParserMsReadConfigs()
        {
            var configString = JsonConvert.SerializeObject(_configModel.Parser.MsRead, Formatting.Indented);
            _loggerService.Log(configString);
        }

        public void ShowStorageConfigs()
        {
            var configString = JsonConvert.SerializeObject(_configModel.Storage, Formatting.Indented);
            _loggerService.Log(configString);
        }

        public void ShowStorageLocalConfigs()
        {
            var configString = JsonConvert.SerializeObject(_configModel.Storage.Local, Formatting.Indented);
            _loggerService.Log(configString);
        }

        public void ShowStorageBlobConfigs()
        {
            var configString = JsonConvert.SerializeObject(_configModel.Storage.Blob, Formatting.Indented);
            _loggerService.Log(configString);
        }
        public void ShowPredictionConfigs()
        {
            var configString = JsonConvert.SerializeObject(_configModel.Prediction, Formatting.Indented);
            _loggerService.Log(configString);
        }
    }
}
