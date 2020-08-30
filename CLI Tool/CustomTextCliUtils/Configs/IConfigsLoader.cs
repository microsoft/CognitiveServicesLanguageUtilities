

using Microsoft.CustomTextCliUtils.ApplicationLayer.Modeling.Models.Configs;

namespace  Microsoft.CustomTextCliUtils.Configs
{
    public interface IConfigsLoader
    {
        public BlobStorageConfigModel GetBlobConfigModel();
        public LocalStorageConfigModel GetLocalConfigModel();
        public MSReadConfigModel GetMSReadConfigModel();
        public StorageConfigModel GetStorageConfigModel();
        public ChunkerConfigModel GetChunkerConfigModel();
        public PredictionConfigModel GetPredictionConfigModel();
    }
}
