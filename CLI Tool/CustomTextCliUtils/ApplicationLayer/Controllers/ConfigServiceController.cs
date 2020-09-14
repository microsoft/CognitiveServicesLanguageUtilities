using Microsoft.CustomTextCliUtils.ApplicationLayer.Modeling.Models.Configs;
using Microsoft.CustomTextCliUtils.ApplicationLayer.Services.Logger;
using Microsoft.CustomTextCliUtils.ApplicationLayer.Services.Storage;
using Microsoft.CustomTextCliUtils.Configs.Consts;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Threading.Tasks;

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
            try
            {
                ReadConfigsFromFile(filePath).ConfigureAwait(false).GetAwaiter().GetResult();
            }
            catch (Exceptions.Storage.FileNotFoundException)
            {
                _configModel = new ConfigModel();
                StoreConfigsModelAsync().ConfigureAwait(false).GetAwaiter().GetResult();
            }
        }

        private async Task ReadConfigsFromFile(string filePath)
        {
            var configsFile = await _storageService.ReadAsStringFromAbsolutePathAsync(filePath);
            _configModel = JsonConvert.DeserializeObject<ConfigModel>(configsFile);
        }

        public async Task LoadConfigsFromFile(string configsFilePath)
        {
            await ReadConfigsFromFile(configsFilePath);
            await StoreConfigsModelAsync();
            _loggerService.Log("Configs loaded from file");
        }

        private async Task StoreConfigsModelAsync()
        {
            var configString = JsonConvert.SerializeObject(_configModel, Formatting.Indented);
            await _storageService.StoreDataAsync(configString, Constants.ConfigsFileName);
        }

        public async Task SetChunkerConfigsAsync(int? charLimit)
        {
            if (charLimit != null)
            {
                _configModel.Chunker.CharLimit = (int)charLimit;
            }
            await StoreConfigsModelAsync();
            _loggerService.Log("Updated Chunker configs");
        }

        public async Task SetMsReadConfigsAsync(string cognitiveServicesKey, string endpointUrl)
        {
            if (!string.IsNullOrEmpty(cognitiveServicesKey))
            {
                _configModel.Parser.MsRead.CongnitiveServiceKey = cognitiveServicesKey;
            }
            if (!string.IsNullOrEmpty(endpointUrl))
            {
                _configModel.Parser.MsRead.CognitiveServiceEndPoint = endpointUrl;
            }
            await StoreConfigsModelAsync();
            _loggerService.Log("Updated MsRead configs");
        }

        public async Task SetBlobStorageConfigsAsync(string connectionString, string sourceContainer, string destinationContainer)
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
            await StoreConfigsModelAsync();
            _loggerService.Log("Updated Blob Storage configs");
        }

        public async Task SetLocalStorageConfigsAsync(string sourceDirectory, string destinationDirectory)
        {
            if (!string.IsNullOrEmpty(sourceDirectory))
            {
                _configModel.Storage.Local.SourceDirectory = sourceDirectory;
            }
            if (!string.IsNullOrEmpty(destinationDirectory))
            {
                _configModel.Storage.Local.DestinationDirectory = destinationDirectory;
            }
            await StoreConfigsModelAsync();
            _loggerService.Log("Updated Local Storage configs");
        }

        public async Task SetPredictionConfigsAsync(string customTextKey, string endpointUrl, string appId, string versionId)
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
            await StoreConfigsModelAsync();
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
