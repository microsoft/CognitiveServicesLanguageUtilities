namespace FileFormatConverter.Core.DataStructures.FileModels.IntermediateEntitiesModel
{
    public class IntermediateEntitiesModel : BaseFileModel
    {
        public CustomExtractorInfo[] Extractors { get; set; }

        public CustomDocument[] Documents { get; set; }
    }

    public class CustomExtractorInfo
    {
        public string Name { get; set; }
    }

    public class CustomDocument
    {
        public CustomExtractor[] Extractors { get; set; }

        public string Language { get; set; } = "en-US";

        public string Location { get; set; }
    }

    public class CustomExtractor
    {
        public long RegionOffset { get; set; }

        public long RegionLength { get; set; }

        public CustomLabel[] Labels { get; set; }
    }

    public class CustomLabel
    {
        public string ExtractorName { get; set; }

        public long Offset { get; set; }

        public long Length { get; set; }
    }
}
