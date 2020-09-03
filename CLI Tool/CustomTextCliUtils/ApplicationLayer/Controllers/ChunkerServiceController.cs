using Microsoft.CustomTextCliUtils.ApplicationLayer.Helpers.Models;
using Microsoft.CustomTextCliUtils.ApplicationLayer.Exceptions;
using Microsoft.CustomTextCliUtils.ApplicationLayer.Factories.Storage;
using Microsoft.CustomTextCliUtils.ApplicationLayer.Modeling.Enums.Logger;
using Microsoft.CustomTextCliUtils.ApplicationLayer.Modeling.Enums.Misc;
using Microsoft.CustomTextCliUtils.ApplicationLayer.Modeling.Models.Chunker;
using Microsoft.CustomTextCliUtils.ApplicationLayer.Services.Chunker;
using Microsoft.CustomTextCliUtils.ApplicationLayer.Services.Logger;
using Microsoft.CustomTextCliUtils.ApplicationLayer.Services.Storage;
using Microsoft.CustomTextCliUtils.Configs;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Concurrent;

namespace Microsoft.CustomTextCliUtils.ApplicationLayer.Controllers
{
    public class ChunkerServiceController
    {
        private readonly IConfigsLoader _configurationService;
        private readonly IStorageFactoryFactory _storageFactoryFactory;
        private IStorageService _sourceStorageService;
        private IStorageService _destinationStorageService;
        private readonly ILoggerService _loggerService;
        private readonly IChunkerService _chunkerService;

        public ChunkerServiceController(
            IConfigsLoader configurationService,
            IStorageFactoryFactory storageFactoryFactory,
            ILoggerService loggerService,
            IChunkerService chunkerService)
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

        public async Task ChunkTextAsync(StorageType sourceStorageType, StorageType destinationStorageType)
        {
            InitializeStorage(sourceStorageType, destinationStorageType);
            var charLimit = _configurationService.GetChunkerConfigModel().CharLimit;
            var convertedFiles = new ConcurrentBag<string>();
            var failedFiles = new ConcurrentDictionary<string, string>();

            // read files from source storage
            var fileNames = _sourceStorageService.ListFiles();
            // chunk files
            var listOfTasks = new List<Task>();
            fileNames.ToList().ForEach(fileName =>
            {
                listOfTasks.Add(Task.Run(() => RunAsync(fileName, charLimit, convertedFiles, failedFiles)));
            });
            await Task.WhenAll(listOfTasks);
            _loggerService.LogParsingResult(convertedFiles, failedFiles);
        }

        private void RunAsync(string fileName, int charLimit, IEnumerable<string> convertedFiles, IDictionary<string, string> failedFiles)
        {
            try
            {
                // validate types
                _chunkerService.ValidateFileType(fileName);
                // read file
                _loggerService.LogOperation(OperationType.ReadingFile, fileName);
                string file = _sourceStorageService.ReadFileAsString(fileName);
                // chunk file
                _loggerService.LogOperation(OperationType.ChunkingFile, fileName);
                List<ChunkInfo> chunkedText = _chunkerService.Chunk(file, charLimit);
                // store file
                _loggerService.LogOperation(OperationType.StoringResult, fileName);
                foreach (var item in chunkedText.Select((value, i) => (value, i)))
                {
                    var newFileName = ChunkInfoHelper.GetChunkFileName(fileName, item.i);
                    _destinationStorageService.StoreData(item.value.Text, newFileName);
                }
                convertedFiles.Append(fileName);
            }
            catch (CliException e)
            {
                failedFiles[fileName] = e.Message;
                _loggerService.LogError(e);
            }
        }
    }
}
