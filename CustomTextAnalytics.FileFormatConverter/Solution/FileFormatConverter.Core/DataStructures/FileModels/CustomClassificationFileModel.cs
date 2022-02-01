using Newtonsoft.Json;

namespace FileFormatConverter.Core.DataStructures.FileModels
{
    public class CustomClassificationFileModel
    {
        [JsonProperty("classifiers")]
        public CustomClassifier[] Classifiers { get; set; }

        [JsonProperty("documents")]
        public ClassifierDocument[] Documents { get; set; }
    }

    public class CustomClassifier
    {
        [JsonProperty("name")]
        public string Name { get; set; }
    }

    public class ClassifierDocument
    {
        [JsonProperty("location")]
        public string Location { get; set; }

        [JsonProperty("language")]
        public string Language { get; set; }

        [JsonProperty("classifiers")]
        public DocumentClassifier[] Classifiers { get; set; }
    }

    public class DocumentClassifier
    {
        [JsonProperty("classifierName")]
        public string ClassifierName { get; set; }
    }
}
