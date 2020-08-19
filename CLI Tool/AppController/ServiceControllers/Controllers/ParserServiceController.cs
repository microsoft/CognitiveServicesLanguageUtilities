using CustomTextCliUtils.AppController.Exceptions;
using CustomTextCliUtils.AppController.Factories.Storage;
using CustomTextCliUtils.AppController.Services.Logger;
using CustomTextCliUtils.AppController.Services.Parser;
using CustomTextCliUtils.AppController.Services.Storage;
using CustomTextCliUtils.Configs;
using CustomTextCliUtils.AppController.Models.Enums;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CustomTextCliUtils.AppController.Services.Chunker;

namespace CustomTextCliUtils.AppController.ServiceControllers.Controllers
{
    class ParserServiceController : IParserServiceController
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
            List<string> convertedFiles = new List<string>();
            List<string> failedFiles = new List<string>();

            // read files from source storage
            var fileNames = _sourceStorageService.ListFiles();
            // parse files
            var tasks = fileNames.Select(async fileName =>
            {
                try
                {
                    _parserService.ValidateFileType(Path.GetExtension(fileName));
                    _loggerService.LogOperation(OperationType.ReadingFile, fileName);
                    Stream file = await _sourceStorageService.ReadFile(fileName);
                    _loggerService.LogOperation(OperationType.ParsingFile, fileName);
                    ParseResult parseResult = await _parserService.ParseFile(file);
                    _loggerService.LogOperation(OperationType.ChunkingFile, fileName);
                    List<string> chunkedText = _chunkerService.Chunk(parseResult, chunkType);
                    _loggerService.LogOperation(OperationType.StoringResult, fileName);
                    chunkedText.Select((t, i) =>
                    {
                        var newFileName = Path.GetFileNameWithoutExtension(fileName) + ".txt";
                        _destinationStorageService.StoreData(chunkedText[0], newFileName);
                        return newFileName;
                    });
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
