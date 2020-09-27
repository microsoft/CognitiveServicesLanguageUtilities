using System.Threading.Tasks;

namespace Microsoft.CogSLanguageUtilities.Definitions.APIs.Controllers
{
    public interface IConfigsController
    {
        Task LoadConfigsFromFile(string configsFilePath);
        Task SetBlobStorageConfigsAsync(string connectionString, string sourceContainer, string destinationContainer);
        Task SetChunkerConfigsAsync(int? charLimit);
        Task SetCustomTextAuthoringConfigsAsync(string customTextKey, string endpointUrl, string appId);
        Task SetCustomTextPredictionConfigsAsync(string customTextKey, string endpointUrl, string appId);
        Task SetLocalStorageConfigsAsync(string sourceDirectory, string destinationDirectory);
        Task SetMsReadConfigsAsync(string cognitiveServicesKey, string endpointUrl);
        Task SetTextAnalyticsConfigsAsync(string azureResourceKey, string azureResourceEndpoint, string defaultLanguage, bool? enableSentimentByDefault, bool? enableNerByDefault, bool? enableKeyphraseByDefault);
        void ShowAllConfigs();
        void ShowChunkerConfigs();
        void ShowCustomTextAuthoringConfigs();
        void ShowCustomTextConfigs();
        void ShowCustomTextPredictionConfigs();
        void ShowParserConfigs();
        void ShowParserMsReadConfigs();
        void ShowStorageBlobConfigs();
        void ShowStorageConfigs();
        void ShowStorageLocalConfigs();
        void ShowTextAnalyticsConfigs();
    }
}