

namespace CustomTextCliUtils.Configs.Consts
{
    class ConfigKeys
    {
        // msread
        public const string MSReadCognitiveServicesKey = "cognitive-services-key";
        public const string MSReadCognitiveServicesEndpoint = "cognitive-services-endpoint";

        // local storage
        public const string LocalStorageSourceDir = "source-dir";
        public const string LocalStorageDestinationDir = "destination-dir";

        // blob storage
        public const string BlobStorageSourceContainer = "source-container";
        public const string BlobStorageDestinationContainer = "destination-container";
        public const string BlobStorageConnectionstring = "connection-string";

        // chunker
        public const string ChunkerCharLimit = "char-limit";

        // prediction
        public const string PredictionCustomTextKey = "customtext-key";
        public const string PredictionEndpointUrl = "customtext-endpoint";
        public const string PredictionAppId = "app-id";
        public const string PredictionVersionId = "version-id";

    }
}
