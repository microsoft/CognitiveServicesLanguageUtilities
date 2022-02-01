using Newtonsoft.Json;

namespace FileFormatConverter.Models.Input.Jsonl
{
    public class JsonlFileModel
    {
        public SingleLineContent[] lines;
    }

    public class SingleLineContent
    {
        [JsonProperty("image_url")]
        public string ImageUrl { get; set; }

        [JsonProperty("label")]
        public Label[] Label { get; set; }

        [JsonProperty("label_confidence")]
        public long[] LabelConfidence { get; set; }
    }

    public class Label
    {
        [JsonProperty("label")]
        public string Text { get; set; }

        [JsonProperty("offsetStart")]
        public long OffsetStart { get; set; }

        [JsonProperty("offsetEnd")]
        public long OffsetEnd { get; set; }
    }
}
