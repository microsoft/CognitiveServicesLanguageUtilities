using CliTool.Configs;
using CliTool.Configs.Consts;
using CliTool.Services.Logger;
using CliTool.Services.Storage;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CliTool.ServiceControllers.Controllers
{
    class ConfigServiceController
    {
        ILoggerService _loggerService;
        IStorageService _storageService;
        ConfigModel _configModel;

        public ConfigServiceController(ILoggerService loggerService, IStorageService storageService)
        {
            _loggerService = loggerService;
            _storageService = storageService;
            var configsFile = storageService.ReadFileAsString(Constants.ConfigsFileName);
            _configModel = JsonConvert.DeserializeObject<ConfigModel>(configsFile);
        }

        private void StoreConfigsModel()
        {
            var configString = JsonConvert.SerializeObject(_configModel);
            _storageService.StoreData(configString, Constants.ConfigsFileName);
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
        }

        public void ShowAllConfigs()
        {
            var configString = JsonConvert.SerializeObject(_configModel);
            _loggerService.Log(configString);
        }
    }
}
