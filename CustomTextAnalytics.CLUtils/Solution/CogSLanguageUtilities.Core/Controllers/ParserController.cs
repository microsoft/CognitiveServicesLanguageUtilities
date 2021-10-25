// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
﻿using Microsoft.CogSLanguageUtilities.Definitions.APIs.Configs;
using Microsoft.CogSLanguageUtilities.Definitions.APIs.Controllers;
using Microsoft.CogSLanguageUtilities.Definitions.APIs.Factories.Parser;
using Microsoft.CogSLanguageUtilities.Definitions.APIs.Factories.Storage;
using Microsoft.CogSLanguageUtilities.Definitions.APIs.Services;
using Microsoft.CogSLanguageUtilities.Definitions.Exceptions;
using Microsoft.CogSLanguageUtilities.Definitions.Helpers;
using Microsoft.CogSLanguageUtilities.Definitions.Models.Enums.Chunker;
using Microsoft.CogSLanguageUtilities.Definitions.Models.Enums.Logger;
using Microsoft.CogSLanguageUtilities.Definitions.Models.Enums.Storage;
using System.Collections.Concurrent;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.CogSLanguageUtilities.Core.Controllers
{
    public class ParserController : IParserController
    {
        private readonly IConfigsLoader _configurationService;
        private readonly IStorageFactoryFactory _storageFactoryFactory;
        private readonly IParserPoolManager _parserPoolManager;
        private IStorageService _sourceStorageService;
        private IStorageService _destinationStorageService;
        private readonly ILoggerService _loggerService;
        private readonly IChunkerService _chunkerService;

        public ParserController(
            IConfigsLoader configurationService,
            IStorageFactoryFactory storageFactoryFactory,
            IParserPoolManager parserPoolManager,
            ILoggerService loggerService,
            IChunkerService chunkerService)
        {
            _configurationService = configurationService;
            _storageFactoryFactory = storageFactoryFactory;
            _parserPoolManager = parserPoolManager;
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
            var chunkLevel = _configurationService.GetChunkerConfigModel().ChunkSectionLevel;
            var convertedFiles = new ConcurrentBag<string>();
            var failedFiles = new ConcurrentDictionary<string, string>();

            // read files from source storage
            var fileNames = await _sourceStorageService.ListFilesAsync();
            // parse files
            var tasks = fileNames.Select(async fileName =>
            {
                try
                {
                    // select parser according to type
                    var fileType = Path.GetExtension(fileName);
                    var parsingService = _parserPoolManager.GetParser(fileType, fileName);
                    // read file
                    _loggerService.LogOperation(OperationType.ReadingFile, fileName);
                    var file = await _sourceStorageService.ReadFileAsync(fileName);
                    // parse file
                    _loggerService.LogOperation(OperationType.ParsingFile, fileName);
                    var parseResult = await parsingService.ParseFile(file);
                    // chunk file
                    _loggerService.LogOperation(OperationType.ChunkingFile, fileName);
                    var chunkedText = _chunkerService.Chunk(parseResult, chunkType, charLimit, chunkLevel);
                    // store file
                    _loggerService.LogOperation(OperationType.StoringResult, fileName);
                    foreach (var item in chunkedText.Select((value, i) => (value, i)))
                    {
                        var newFileName = ChunkInfoHelper.GetChunkFileName(fileName, item.i);
                        await _destinationStorageService.StoreDataAsync(item.value.Text, newFileName);
                    }
                    convertedFiles.Add(fileName);
                }
                catch (CliException e)
                {
                    failedFiles[fileName] = e.Message;
                    _loggerService.LogError(e);
                }
            });
            await Task.WhenAll(tasks);
            _loggerService.LogParsingResult(convertedFiles, failedFiles);
        }


    }
}
