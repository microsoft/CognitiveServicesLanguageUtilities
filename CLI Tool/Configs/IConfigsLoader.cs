

using CustomTextCliUtils.AppController.Models;
using CustomTextCliUtils.AppController.Models.Config.Chunker;
using CustomTextCliUtils.AppController.Models.Config.Parser;
using CustomTextCliUtils.AppController.Models.Config.Prediction;
using CustomTextCliUtils.AppController.Models.Config.Storage;

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
