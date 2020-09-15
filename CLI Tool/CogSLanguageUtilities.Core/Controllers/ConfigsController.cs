using Microsoft.CogSLanguageUtilities.Definitions.APIs.Controllers;
using Microsoft.CogSLanguageUtilities.Definitions.APIs.Services;
using Microsoft.CogSLanguageUtilities.Definitions.Configs.Consts;
using Microsoft.CogSLanguageUtilities.Definitions.Models.Configs;
using Newtonsoft.Json;
using System.IO;
using System.Threading.Tasks;

namespace Microsoft.CogSLanguageUtilities.Core.Controllers
{
    public class ConfigsController : IConfigsController
    {
        private readonly ILoggerService _loggerService;
        private readonly IStorageService _storageService;
        private ConfigModel _configModel;

        public ConfigsController(ILoggerService loggerService, IStorageService storageService)
        {
            _loggerService = loggerService;
            _storageService = storageService;
            var filePath = Path.Combine(Constants.ConfigsFileLocalDirectory, Constants.ConfigsFileName);
            try
            {
                ReadConfigsFromFile(filePath).ConfigureAwait(false).GetAwaiter().GetResult();
            }
            catch (Definitions.Exceptions.Storage.FileNotFoundException)
            {
                _configModel = new ConfigModel();
                StoreConfigsModelAsync().ConfigureAwait(false).GetAwaiter().GetResult();
            }
        }

        public void ShowEvaluationConfigs()
        {
            var configString = JsonConvert.SerializeObject(_configModel.Evaluation.LabeledExamplesApp, Formatting.Indented);
            _loggerService.Log(configString);
        }

        public void ShowTextAnalyticsConfigs()
        {
            var configString = JsonConvert.SerializeObject(_configModel.TextAnalytics, Formatting.Indented);
            _loggerService.Log(configString);
        }

        public async Task SetEvaluationConfigsAsync(string azureResourceKey, string azureResourceEndpoint, string appId)
        {
            _configModel.Evaluation.LabeledExamplesApp.AzureResourceKey = azureResourceKey ?? _configModel.Evaluation.LabeledExamplesApp.AzureResourceKey;
            _configModel.Evaluation.LabeledExamplesApp.AzureResourceEndpoint = azureResourceEndpoint ?? _configModel.Evaluation.LabeledExamplesApp.AzureResourceEndpoint;
            _configModel.Evaluation.LabeledExamplesApp.AppId = appId ?? _configModel.Evaluation.LabeledExamplesApp.AppId;

            await StoreConfigsModelAsync();
            _loggerService.Log("Updated Custom Text prediction configs");
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

        public async Task SetTextAnalyticsConfigsAsync(string azureResourceKey, string azureResourceEndpoint, string defaultLanguage, bool? enableSentimentByDefault, bool? enableNerByDefault, bool? enableKeyphraseByDefault)
        {
            _configModel.TextAnalytics.AzureResourceKey = azureResourceKey ?? _configModel.TextAnalytics.AzureResourceKey;
            _configModel.TextAnalytics.AzureResourceEndpoint = azureResourceEndpoint ?? _configModel.TextAnalytics.AzureResourceEndpoint;
            _configModel.TextAnalytics.DefaultLanguage = defaultLanguage ?? _configModel.TextAnalytics.DefaultLanguage;

            _configModel.TextAnalytics.DefaultOperations.Sentiment = enableSentimentByDefault ?? _configModel.TextAnalytics.DefaultOperations.Sentiment;
            _configModel.TextAnalytics.DefaultOperations.Ner = enableNerByDefault ?? _configModel.TextAnalytics.DefaultOperations.Ner;
            _configModel.TextAnalytics.DefaultOperations.Keyphrase = enableKeyphraseByDefault ?? _configModel.TextAnalytics.DefaultOperations.Keyphrase;

            await StoreConfigsModelAsync();
            _loggerService.Log("Updated Text Analytics configs");
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

        public async Task SetPredictionConfigsAsync(string customTextKey, string endpointUrl, string appId)
        {
            if (!string.IsNullOrEmpty(customTextKey))
            {
                _configModel.CustomText.CustomTextKey = customTextKey;
            }
            if (!string.IsNullOrEmpty(endpointUrl))
            {
                _configModel.CustomText.EndpointUrl = endpointUrl;
            }
            if (!string.IsNullOrEmpty(appId))
            {
                _configModel.CustomText.AppId = appId;
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
            var configString = JsonConvert.SerializeObject(_configModel.CustomText, Formatting.Indented);
            _loggerService.Log(configString);
        }
    }
}
