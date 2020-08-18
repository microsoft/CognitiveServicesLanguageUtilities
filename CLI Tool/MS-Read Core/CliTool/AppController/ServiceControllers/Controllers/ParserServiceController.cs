using CliTool.AppController.Factories.Storage;
using CliTool.Configs.Models.Enums;
using CliTool.Exceptions;
using CliTool.Services.Configuration;
using CliTool.Services.Logger;
using CliTool.Services.Parser;
using CliTool.Services.Storage;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CliTool.ServiceControllers.Controllers
{
    class ParserServiceController : IParserServiceController
    {
        readonly IConfigurationService _configurationService;
        readonly IStorageFactoryFactory _storageFactoryFactory;
        readonly IParserService _parserService;
        readonly IStorageService _sourceStorageService;
        readonly IStorageService _destinationStorageService;
        readonly ILoggerService _loggerService;

        public ParserServiceController(IConfigurationService configurationService, IStorageFactoryFactory storageFactoryFactory, 
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
