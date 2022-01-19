using Newtonsoft.Json;
using System;

namespace Microsoft.CognitiveSearchIntegration.Utilities.Nugets.CustomTextAuthoringClient.RestClient.ApiModels
{
    public class ProjectFileExportJobStatus
    {
        [JsonProperty("resultUrl")]
        public Uri ResultUrl { get; set; }

        [JsonProperty("jobId")]
        public string JobId { get; set; }

        [JsonProperty("createdDateTime")]
        public DateTimeOffset CreatedDateTime { get; set; }

        [JsonProperty("lastUpdatedDateTime")]
        public DateTimeOffset LastUpdatedDateTime { get; set; }

        [JsonProperty("expirationDateTime")]
        public DateTimeOffset ExpirationDateTime { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }
    }
}
