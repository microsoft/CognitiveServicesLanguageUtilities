using CustomTextCliUtils.AppController.Exceptions;
using CustomTextCliUtils.AppController.Factories.Storage;
using CustomTextCliUtils.AppController.Services.Logger;
using CustomTextCliUtils.AppController.Services.Parser;
using CustomTextCliUtils.AppController.Services.Storage;
using CustomTextCliUtils.Configs;
using CustomTextCliUtils.Configs.Models.Enums;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CustomTextCliUtils.AppController.ServiceControllers.Controllers
{
    class ParserServiceController : IParserServiceController
    {
        readonly IConfigsLoader _configurationService;
        readonly IStorageFactoryFactory _storageFactoryFactory;
        readonly IParserService _parserService;
        readonly IStorageService _sourceStorageService;
        readonly IStorageService _destinationStorageService;
        readonly ILoggerService _loggerService;

        public ParserServiceController(IConfigsLoader configurationService, IStorageFactoryFactory storageFactoryFactory, 
            IParserService parserService, ILoggerService loggerService, StorageType sourceStorageType, StorageType destinationStorageType)
        {
            _configurationService = configurationService;
            _storageFactoryFactory = storageFactoryFactory;
            _parserService = parserService;
            _loggerService = loggerService;
            IStorageFactory sourceFactory = _storageFactoryFactory.CreateStorageFactory(TargetStorage.Source);
            IStorageFactory destinationFactory = _storageFactoryFactory.CreateStorageFactory(TargetStorage.Destination);
            var storageConfigModel = _configurationService.GetStorageConfigModel();
            _sourceStorageService = sourceFactory.CreateStorageService(sourceStorageType, storageConfigModel);
            _destinationStorageService = destinationFactory.CreateStorageService(destinationStorageType, storageConfigModel);
        }

        public async Task ExtractText()
        {
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
                    string text = await _parserService.ExtractText(file, fileName);
                    _loggerService.LogOperation(OperationType.StoringResult, fileName);
                    _destinationStorageService.StoreData(text, Path.ChangeExtension(fileName, "txt"));
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
