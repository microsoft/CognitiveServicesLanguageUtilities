using Newtonsoft.Json;

namespace FileFormatConverter.Core.DataStructures.FileModels.CustomText.Entities
{
    public class CustomText_Entities_FileModel : BaseFileModel
    {
        [JsonProperty("extractors")]
        public CustomExtractorInfo[] Extractors { get; set; }

        [JsonProperty("documents")]
        public CustomDocument[] Documents { get; set; }
    }

    public class CustomExtractorInfo
    {
        [JsonProperty("name")]
        public string Name { get; set; }
    }

    public class CustomDocument
    {
        [JsonProperty("extractors")]
        public CustomExtractor[] Extractors { get; set; }

        [JsonProperty("language")]
        public string Language { get; set; } = "en-US";

        [JsonProperty("location")]
        public string Location { get; set; }
    }

    public class CustomExtractor
    {
        [JsonProperty("regionOffset")]
        public long RegionOffset { get; set; }

        [JsonProperty("regionLength")]
        public long RegionLength { get; set; }

        [JsonProperty("labels")]
        public CustomLabel[] Labels { get; set; }
    }

    public class CustomLabel
    {
        [JsonProperty("extractorName")]
        public string ExtractorName { get; set; }

        [JsonProperty("offset")]
        public long Offset { get; set; }

        [JsonProperty("length")]
        public long Length { get; set; }
    }
}
