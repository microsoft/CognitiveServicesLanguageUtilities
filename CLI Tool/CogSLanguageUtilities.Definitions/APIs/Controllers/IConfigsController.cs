using System.Threading.Tasks;

namespace Microsoft.CogSLanguageUtilities.Definitions.APIs.Controllers
{
    public interface IConfigsController
    {
        Task LoadConfigsFromFile(string configsFilePath);
        Task SetBlobStorageConfigsAsync(string connectionString, string sourceContainer, string destinationContainer);
        Task SetChunkerConfigsAsync(int? charLimit);
        Task SetLocalStorageConfigsAsync(string sourceDirectory, string destinationDirectory);
        Task SetMsReadConfigsAsync(string cognitiveServicesKey, string endpointUrl);
        Task SetPredictionConfigsAsync(string customTextKey, string endpointUrl, string appId);
        Task SetTextAnalyticsConfigsAsync(string azureResourceKey, string azureResourceEndpoint, string defaultLanguage, bool? enableSentimentByDefault, bool? enableNerByDefault, bool? enableKeyphraseByDefault);
        void ShowAllConfigs();
        void ShowChunkerConfigs();
        void ShowParserConfigs();
        void ShowParserMsReadConfigs();
        void ShowPredictionConfigs();
        void ShowStorageBlobConfigs();
        void ShowStorageConfigs();
        void ShowStorageLocalConfigs();
        void ShowTextAnalyticsConfigs();
    }
}