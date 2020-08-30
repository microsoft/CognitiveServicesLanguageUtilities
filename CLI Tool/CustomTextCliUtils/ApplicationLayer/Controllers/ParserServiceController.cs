using Microsoft.CustomTextCliUtils.ApplicationLayer.Exceptions;
using Microsoft.CustomTextCliUtils.ApplicationLayer.Factories.Storage;
using Microsoft.CustomTextCliUtils.ApplicationLayer.Services.Logger;
using Microsoft.CustomTextCliUtils.ApplicationLayer.Services.Parser;
using Microsoft.CustomTextCliUtils.ApplicationLayer.Services.Storage;
using Microsoft.CustomTextCliUtils.Configs;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.CustomTextCliUtils.ApplicationLayer.Services.Chunker;
using Microsoft.CustomTextCliUtils.ApplicationLayer.Modeling.Enums.Misc;
using Microsoft.CustomTextCliUtils.ApplicationLayer.Modeling.Enums.Logger;
using Microsoft.CustomTextCliUtils.ApplicationLayer.Modeling.Models.Parser;
using Microsoft.CustomTextCliUtils.ApplicationLayer.Modeling.Models.Chunker;

namespace  Microsoft.CustomTextCliUtils.ApplicationLayer.Controllers
{
    class ParserServiceController
    {
        readonly IConfigsLoader _configurationService;
        readonly IStorageFactoryFactory _storageFactoryFactory;
        readonly IParserService _parserService;
        IStorageService _sourceStorageService;
        IStorageService _destinationStorageService;
        readonly ILoggerService _loggerService;
        readonly IChunkerService _chunkerService;

        public ParserServiceController(IConfigsLoader configurationService, IStorageFactoryFactory storageFactoryFactory, 
            IParserService parserService, ILoggerService loggerService, IChunkerService chunkerService)
        {
            _configurationService = configurationService;
            _storageFactoryFactory = storageFactoryFactory;
            _parserService = parserService;
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

        public async Task ExtractText(StorageType sourceStorageType, StorageType destinationStorageType, ChunkMethod chunkType)
        {
            InitializeStorage(sourceStorageType, destinationStorageType);
            var charLimit = _configurationService.GetChunkerConfigModel().CharLimit;
            List<string> convertedFiles = new List<string>();
            List<string> failedFiles = new List<string>();

            // read files from source storage
            var fileNames = _sourceStorageService.ListFiles();
            // parse files
            var tasks = fileNames.Select(async fileName =>
            {
                try
                {
                    // validate types
                    _parserService.ValidateFileType(fileName);
                    // read file
                    _loggerService.LogOperation(OperationType.ReadingFile, fileName);
                    Stream file = await _sourceStorageService.ReadFile(fileName);
                    // parse file
                    _loggerService.LogOperation(OperationType.ParsingFile, fileName);
                    ParseResult parseResult = await _parserService.ParseFile(file);
                    // chunk file
                    _loggerService.LogOperation(OperationType.ChunkingFile, fileName);
                    List<ChunkInfo> chunkedText = _chunkerService.Chunk(parseResult, chunkType, charLimit);
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
            await Task.WhenAll(tasks);
            _loggerService.LogParsingResult(convertedFiles, failedFiles);
        }


    }
}
