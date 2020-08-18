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
            var configString = JsonConvert.SerializeObject(_configModel, Formatting.Indented);
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

        public void ShowAllConfigs()
        {
            var configString = JsonConvert.SerializeObject(_configModel, Formatting.Indented);
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
    }
}
