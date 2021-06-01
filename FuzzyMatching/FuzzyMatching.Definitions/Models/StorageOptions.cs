using FuzzyMatching.Definitions.Models.Enums;

namespace FuzzyMatching.Definitions.Models
{
    public class StorageOptions
    {
        public StorageType StorageType { get; set; }
        public string ConnectionString { get; set; }
        public string ContainerName { get; set; }
        public string BaseDirectory { get; set; }
    }
}
