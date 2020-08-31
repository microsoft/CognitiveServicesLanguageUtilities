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
        readonly ILoggerService _loggerService;
        readonly IStorageService _storageService;
        ConfigModel _configModel;

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
            if (!String.IsNullOrEmpty(cognitiveServicesKey))
            {
                _configModel.Parser.MsRead.CongnitiveServiceKey = cognitiveServicesKey;
            }
            if (!String.IsNullOrEmpty(endpointUrl))
            {
                _configModel.Parser.MsRead.CognitiveServiceEndPoint = endpointUrl;
            }
            StoreConfigsModel();
            _loggerService.Log("Updated MsRead configs");
        }

        public void SetBlobStorageConfigs(string connectionString, string sourceContainer, string destinationContainer)
        {
            if (!String.IsNullOrEmpty(connectionString))
            {
                _configModel.Storage.Blob.ConnectionString = connectionString;
            }
            if (!String.IsNullOrEmpty(sourceContainer))
            {
                _configModel.Storage.Blob.SourceContainer = sourceContainer;
            }
            if (!String.IsNullOrEmpty(destinationContainer))
            {
                _configModel.Storage.Blob.DestinationContainer = destinationContainer;
            }
            StoreConfigsModel();
            _loggerService.Log("Updated Blob Storage configs");
        }

        public void SetLocalStorageConfigs(string sourceDirectory, string destinationDirectory)
        {
            if (!String.IsNullOrEmpty(sourceDirectory))
            {
                _configModel.Storage.Local.SourceDirectory = sourceDirectory;
            }
            if (!String.IsNullOrEmpty(destinationDirectory))
            {
                _configModel.Storage.Local.DestinationDirectory = destinationDirectory;
            }
            StoreConfigsModel();
            _loggerService.Log("Updated Local Storage configs");
        }

        public void SetPredictionConfigs(string customTextKey, string endpointUrl, string appId, string versionId)
        {
            if (!String.IsNullOrEmpty(customTextKey))
            {
                _configModel.Prediction.CustomTextKey = customTextKey;
            }
            if (!String.IsNullOrEmpty(endpointUrl))
            {
                _configModel.Prediction.EndpointUrl = endpointUrl;
            }
            if (!String.IsNullOrEmpty(appId))
            {
                _configModel.Prediction.AppId = appId;
            }
            if (!String.IsNullOrEmpty(versionId))
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
