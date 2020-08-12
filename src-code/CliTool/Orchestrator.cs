using CliTool.Services.Configuration;
using CliTool.Services.Configuration.Models;
using CliTool.Services.Parser;
using CliTool.Services.Storage;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace CliTool
{
    class Orchestrator : IOrchestrator
    {
        IConfigurationService _configurationService;
        IStorageFactory _storageFactory;
        IParserService _parserService;

        public Orchestrator(IConfigurationService configurationService, IStorageFactory storageFactory, IParserService parserService) {
            _configurationService = configurationService;
            _storageFactory = storageFactory;
            _parserService = parserService;
        }

        public void Run() {
            // load configs
            StorageConfigModel sourceStorageConfigModel = _configurationService.GetSourceStorageConfigModel();
            StorageConfigModel destinationStorageConfigModel = _configurationService.GetDestinationStorageConfigModel();
            // create storage service
            IStorageService sourceStorageService = _storageFactory.CreateStorageService(sourceStorageConfigModel);
            IStorageService destinationStorageService = _storageFactory.CreateStorageService(destinationStorageConfigModel);
            // read files from source storage
            var fileNames = sourceStorageService.ListFiles();
            // parse files
            Parallel.ForEach(fileNames, async (fileName) =>
            {
                Stream file = await sourceStorageService.ReadFile(fileName);
                string text = await _parserService.ExtractText(file);
                //string text = (new StreamReader(file)).ReadToEnd();
                destinationStorageService.StoreData(text, Path.ChangeExtension(fileName, "txt"));
            });
        }
    }
}
