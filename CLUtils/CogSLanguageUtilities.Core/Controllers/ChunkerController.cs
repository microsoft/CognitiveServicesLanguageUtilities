// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
﻿using Microsoft.CogSLanguageUtilities.Definitions.APIs.Configs;
using Microsoft.CogSLanguageUtilities.Definitions.APIs.Controllers;
using Microsoft.CogSLanguageUtilities.Definitions.APIs.Factories.Storage;
using Microsoft.CogSLanguageUtilities.Definitions.APIs.Services;
using Microsoft.CogSLanguageUtilities.Definitions.Enums.Parser;
using Microsoft.CogSLanguageUtilities.Definitions.Exceptions;
using Microsoft.CogSLanguageUtilities.Definitions.Helpers;
using Microsoft.CogSLanguageUtilities.Definitions.Models.Chunker;
using Microsoft.CogSLanguageUtilities.Definitions.Models.Enums.Chunker;
using Microsoft.CogSLanguageUtilities.Definitions.Models.Enums.Logger;
using Microsoft.CogSLanguageUtilities.Definitions.Models.Enums.Storage;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.CogSLanguageUtilities.Core.Controllers
{
    public class ChunkerController : IChunkerController
    {
        private readonly IConfigsLoader _configurationService;
        private readonly IStorageFactoryFactory _storageFactoryFactory;
        private IStorageService _sourceStorageService;
        private IStorageService _destinationStorageService;
        private readonly ILoggerService _loggerService;
        private readonly IChunkerService _chunkerService;
        private readonly IParserService _parserService;

        public ChunkerController(
            IConfigsLoader configurationService,
            IStorageFactoryFactory storageFactoryFactory,
            ILoggerService loggerService,
            IChunkerService chunkerService,
            IParserService parserService)
        {
            _configurationService = configurationService;
            _storageFactoryFactory = storageFactoryFactory;
            _loggerService = loggerService;
            _chunkerService = chunkerService;
            _parserService = parserService;
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
            var fileNames = await _sourceStorageService.ListFilesAsync();
            // chunk files
            var tasks = fileNames.Select(async fileName =>
            {
                try
                {
                    // validate types
                    _parserService.ValidateFileType(fileName);
                    // read file
                    _loggerService.LogOperation(OperationType.ReadingFile, fileName);
                    var file = await _sourceStorageService.ReadFileAsync(fileName);
                    // parse file
                    var parsedFile = await _parserService.ParseFile(file);
                    // chunk file
                    _loggerService.LogOperation(OperationType.ChunkingFile, fileName);
                    List<ChunkInfo> chunkedText = _chunkerService.Chunk(parsedFile, ChunkMethod.Char, charLimit, ElementType.Other);
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
