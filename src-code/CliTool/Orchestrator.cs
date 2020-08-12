using CliTool.Services.Configuration;
using CliTool.Services.Configuration.Models;
using CliTool.Services.Storage;
using System;
using System.Collections.Generic;
using System.Text;

namespace CliTool
{
    class Orchestrator
    {
        IConfigurationService _configurationService;
        IStorageFactory _storageFactory;
        public Orchestrator(IConfigurationService configurationService, IStorageFactory storageFactory) {
            _configurationService = configurationService;
            _storageFactory = storageFactory;
        }

        public void Run() {
            // load configs
            MSReadConfigModel configs = _configurationService.GetMSReadConfigModel();
            StorageConfigModel sourceStorageConfigModel = _configurationService.GetSourceStorageConfigModel();
            StorageConfigModel destinationStorageConfigModel = _configurationService.GetDestinationStorageConfigModel();
            // create storage service
            IStorageService sourceStorageService = _storageFactory.CreateStorageService(sourceStorageConfigModel);
            // read files from source storage
            var fileNames = sourceStorageService.ListFiles();

            // parse files

            // store results
            IStorageService destinationStorageService = _storageFactory.CreateStorageService(destinationStorageConfigModel);

        }
    }
}
