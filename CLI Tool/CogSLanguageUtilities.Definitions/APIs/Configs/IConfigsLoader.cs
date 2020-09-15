using Microsoft.CogSLanguageUtilities.Definitions.Models.Configs.Chunker;
using Microsoft.CogSLanguageUtilities.Definitions.Models.Configs.CustomText;
using Microsoft.CogSLanguageUtilities.Definitions.Models.Configs.Parser;
using Microsoft.CogSLanguageUtilities.Definitions.Models.Configs.Storage;
using Microsoft.CogSLanguageUtilities.Definitions.Models.Configs.TextAnalytics;

namespace Microsoft.CogSLanguageUtilities.Definitions.APIs.Configs
{
    public interface IConfigsLoader
    {
        BlobStorageConfigModel GetBlobConfigModel();
        ChunkerConfigModel GetChunkerConfigModel();
        LocalStorageConfigModel GetLocalConfigModel();
        MSReadConfigModel GetMSReadConfigModel();
        CustomTextConfigModel GetPredictionConfigModel();
        StorageConfigModel GetStorageConfigModel();
        TextAnalyticsConfigModel GetTextAnalyticsConfigModel();
    }
}