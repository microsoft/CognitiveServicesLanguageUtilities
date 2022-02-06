namespace FileFormatConverter.Core.DataStructures.FileModels.IntermediateEntitiesModel
{
    public class IntermediateEntitiesModel : BaseFileModel
    {
        public string[] EntityNames { get; set; }

        public EntityDocument[] Documents { get; set; }
    }

    public class EntityDocument
    {
        public CustomEntity[] Entities { get; set; }

        public string Culture { get; set; } = "en-US";

        public string Location { get; set; }
    }

    public class CustomEntity
    {
        public long RegionStart { get; set; }

        public long RegionLength { get; set; }

        public CustomLabel[] Labels { get; set; }
    }

    public class CustomLabel
    {
        public long Entity { get; set; }

        public long Start { get; set; }

        public long Length { get; set; }
    }
}
