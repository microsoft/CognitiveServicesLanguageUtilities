using CustomTextCliUtils.ApplicationLayer.Exceptions;
using CustomTextCliUtils.ApplicationLayer.Factories.Storage;
using CustomTextCliUtils.ApplicationLayer.Modeling.Enums.Logger;
using CustomTextCliUtils.ApplicationLayer.Modeling.Enums.Misc;
using CustomTextCliUtils.ApplicationLayer.Modeling.Models.Chunker;
using CustomTextCliUtils.ApplicationLayer.Services.Chunker;
using CustomTextCliUtils.ApplicationLayer.Services.Logger;
using CustomTextCliUtils.ApplicationLayer.Services.Storage;
using CustomTextCliUtils.Configs;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CustomTextCliUtils.ApplicationLayer.Controllers
{
    public class ChunkerServiceController
    {
        readonly IConfigsLoader _configurationService;
        readonly IStorageFactoryFactory _storageFactoryFactory;
        IStorageService _sourceStorageService;
        IStorageService _destinationStorageService;
        readonly ILoggerService _loggerService;
        readonly IChunkerService _chunkerService;

        public ChunkerServiceController(IConfigsLoader configurationService, IStorageFactoryFactory storageFactoryFactory,
            ILoggerService loggerService, IChunkerService chunkerService)
        {
            _configurationService = configurationService;
            _storageFactoryFactory = storageFactoryFactory;
            _loggerService = loggerService;
            _chunkerService = chunkerService;
        }

        private void InitializeStorage(StorageType sourceStorageType, StorageType destinationStorageType)
        {
            IStorageFactory sourceFactory = _storageFactoryFactory.CreateStorageFactory(TargetStorage.Source);
            IStorageFactory destinationFactory = _storageFactoryFactory.CreateStorageFactory(TargetStorage.Destination);
            var storageConfigModel = _configurationService.GetStorageConfigModel();
            _sourceStorageService = sourceFactory.CreateStorageService(sourceStorageType, storageConfigModel);
            _destinationStorageService = destinationFactory.CreateStorageService(destinationStorageType, storageConfigModel);
        }

        public void ChunkText(StorageType sourceStorageType, StorageType destinationStorageType)
        {
            InitializeStorage(sourceStorageType, destinationStorageType);
            var charLimit = _configurationService.GetChunkerConfigModel().CharLimit;
            List<string> convertedFiles = new List<string>();
            List<string> failedFiles = new List<string>();

            // read files from source storage
            var fileNames = _sourceStorageService.ListFiles();
            // chunk files
            Parallel.ForEach(fileNames, fileName =>
            {
                try
                {
                    // validate types
                    this._chunkerService.ValidateFileType(fileName);
                    // read file
                    _loggerService.LogOperation(OperationType.ReadingFile, fileName);
                    string file = _sourceStorageService.ReadFileAsString(fileName);
                    // chunk file
                    _loggerService.LogOperation(OperationType.ChunkingFile, fileName);
                    List<ChunkInfo> chunkedText = this._chunkerService.Chunk(file, charLimit);
                    // store file
                    _loggerService.LogOperation(OperationType.StoringResult, fileName);
                    foreach (var item in chunkedText.Select((value, i) => (value, i)))
                    {
                        var newFileName = $"{Path.GetFileNameWithoutExtension(fileName)}_{item.i + 1}.txt";
                        _destinationStorageService.StoreData(item.value.Text, newFileName);
                    }
                    convertedFiles.Add(fileName);
                }
                catch (CliException e)
                {
                    failedFiles.Add(fileName);
                    _loggerService.LogError(e);
                }
            });
            _loggerService.LogParsingResult(convertedFiles, failedFiles);
        }
    }
}
