using CliTool.Services.Configuration;
using CliTool.Services.Logger;
using CliTool.Services.Parser;
using CliTool.Services.Storage;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CliTool
{
    class Orchestrator
    {
        readonly IConfigurationService _configurationService;
        readonly IStorageFactory _storageFactory;
        readonly IParserService _parserService;
        readonly IStorageService _sourceStorageService;
        readonly IStorageService _destinationStorageService;
        readonly ILoggerService _loggerService;

        public Orchestrator(IConfigurationService configurationService, IStorageFactory storageFactory, IParserService parserService, ILoggerService loggerService) {
            _configurationService = configurationService;
            _storageFactory = storageFactory;
            _parserService = parserService;
            _loggerService = loggerService;
            var sourceStorageConfigModel = _configurationService.GetSourceStorageConfigModel();
            var destinationStorageConfigModel = _configurationService.GetDestinationStorageConfigModel();
            _sourceStorageService = _storageFactory.CreateStorageService(sourceStorageConfigModel);
            _destinationStorageService = _storageFactory.CreateStorageService(destinationStorageConfigModel);
        }

        public async Task RunAsync() {
            // read files from source storage
            var fileNames = _sourceStorageService.ListFiles();
            // parse files
            var tasks = fileNames.Select(async fileName =>
            {
                _loggerService.Log("started processing " + fileName);
                Stream file = await _sourceStorageService.ReadFile(fileName);
                string text = await _parserService.ExtractText(file);
                _destinationStorageService.StoreData(text, Path.ChangeExtension(fileName, "txt"));
                _loggerService.Log("finished processing " + fileName);
            });
            await Task.WhenAll(tasks);
        }
    }
}
