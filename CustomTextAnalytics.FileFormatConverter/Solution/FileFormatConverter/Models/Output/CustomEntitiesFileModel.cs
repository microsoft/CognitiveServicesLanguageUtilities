using Newtonsoft.Json;

namespace FileFormatConverter.Models
{
    public class CustomEntitiesFileModel
    {
        [JsonProperty("entityNames")]
        public string[] EntityNames { get; set; }

        [JsonProperty("documents")]
        public EntityDocument[] Documents { get; set; }
    }

    public class EntityDocument
    {
        [JsonProperty("entities")]
        public CustomEntity[] Entities { get; set; }

        [JsonProperty("culture")]
        public string Culture { get; set; } = "en-US";

        [JsonProperty("location")]
        public string Location { get; set; }
    }

    public class CustomEntity
    {
        [JsonProperty("regionStart")]
        public long RegionStart { get; set; }

        [JsonProperty("regionLength")]
        public long RegionLength { get; set; }

        [JsonProperty("labels")]
        public CustomLabel[] Labels { get; set; }
    }

    public class CustomLabel
    {
        [JsonProperty("entity")]
        public long Entity { get; set; }

        [JsonProperty("start")]
        public long Start { get; set; }

        [JsonProperty("length")]
        public long Length { get; set; }
    }
}
