using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.CustomTextCliUtils.Configs.Consts
{
    public class CommandOptionTemplate
    {
        // msread
        public const string MSReadCognitiveServicesKey = "--" + ConfigKeys.MSReadCognitiveServicesKey + " <COGNITIVE_SERVICES_KEY>";
        public const string MSReadCognitiveServicesEndpoint = "--" + ConfigKeys.MSReadCognitiveServicesEndpoint + " <ENDPOINT_URL>";

        // local storage
        public const string LocalStorageSourceDir = "--" + ConfigKeys.LocalStorageSourceDir + " <ABSOLUTE_PATH>";
        public const string LocalStorageDestinationDir = "--" + ConfigKeys.LocalStorageDestinationDir + " <ABSOLUTE_PATH>";

        // blob storage
        public const string BlobStorageSourceContainer = "--" + ConfigKeys.BlobStorageSourceContainer + " <CONTAINER_NAME>";
        public const string BlobStorageDestinationContainer = "--" + ConfigKeys.BlobStorageDestinationContainer + " <CONTAINER_NAME>";
        public const string BlobStorageConnectionstring = "--" + ConfigKeys.BlobStorageConnectionstring + " <CONNECTION_STRING>";

        // chunker
        public const string ChunkerCharLimit = "--" + ConfigKeys.ChunkerCharLimit + " <INTEGER>";

        // prediction
        public const string PredictionCustomTextKey = "--" + ConfigKeys.PredictionCustomTextKey + " <CUSTOM_TEXT_KEY>";
        public const string PredictionEndpointUrl = "--" + ConfigKeys.PredictionEndpointUrl + " <ENDPOINT_URL>";
        public const string PredictionAppId = "--" + ConfigKeys.PredictionAppId + " <APP_ID>";
        public const string PredictionVersionId = "--" + ConfigKeys.PredictionVersionId + " <APP_VERSION>";
    }
}