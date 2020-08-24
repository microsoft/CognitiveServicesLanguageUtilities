

using CustomTextCliUtils.ApplicationLayer.Modeling.Models.Configs;

namespace CustomTextCliUtils.Configs
{
    interface IConfigsLoader
    {
        public BlobStorageConfigModel GetBlobConfigModel();
        public LocalStorageConfigModel GetLocalConfigModel();
        public MSReadConfigModel GetMSReadConfigModel();
        public StorageConfigModel GetStorageConfigModel();
        public ChunkerConfigModel GetChunkerConfigModel();
        public PredictionConfigModel GetPredictionConfigModel();
    }
}
