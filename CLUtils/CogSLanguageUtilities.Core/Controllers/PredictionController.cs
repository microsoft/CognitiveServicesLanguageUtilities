// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
﻿using Microsoft.CogSLanguageUtilities.Definitions.APIs.Configs;
using Microsoft.CogSLanguageUtilities.Definitions.APIs.Controllers;
using Microsoft.CogSLanguageUtilities.Definitions.APIs.Factories.Parser;
using Microsoft.CogSLanguageUtilities.Definitions.APIs.Factories.Storage;
using Microsoft.CogSLanguageUtilities.Definitions.APIs.Services;
using Microsoft.CogSLanguageUtilities.Definitions.Exceptions;
using Microsoft.CogSLanguageUtilities.Definitions.Models.Enums.Chunker;
using Microsoft.CogSLanguageUtilities.Definitions.Models.Enums.Logger;
using Microsoft.CogSLanguageUtilities.Definitions.Models.Enums.Prediction;
using Microsoft.CogSLanguageUtilities.Definitions.Models.Enums.Storage;
using Newtonsoft.Json;
using System.Collections.Concurrent;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.CogSLanguageUtilities.Core.Controllers
{
    public class PredictionController : IPredictionController
    {
        private readonly IConfigsLoader _configurationService;
        private readonly IStorageFactoryFactory _storageFactoryFactory;
        private readonly IParserPoolManager _parserPoolManager;
        private IStorageService _sourceStorageService;
        private IStorageService _destinationStorageService;
        private readonly ILoggerService _loggerService;
        private readonly IChunkerService _chunkerService;
        private readonly ITextAnalyticsService _textAnalyticsPredictionService;
        private readonly ICustomTextPredictionService _customTextPredictionService;
        private readonly IConcatenationService _concatenationService;

        public PredictionController(
            IConfigsLoader configurationService,
            IStorageFactoryFactory storageFactoryFactory,
            IParserPoolManager parserPoolManager,
            ILoggerService loggerService,
            IChunkerService chunkerService,
            ITextAnalyticsService textAnalyticsPredictionService,
            ICustomTextPredictionService CustomTextPredictionService,
            IConcatenationService concatenationService)
        {
            _configurationService = configurationService;
            _storageFactoryFactory = storageFactoryFactory;
            _parserPoolManager = parserPoolManager;
            _loggerService = loggerService;
            _chunkerService = chunkerService;
            _textAnalyticsPredictionService = textAnalyticsPredictionService;
            _customTextPredictionService = CustomTextPredictionService;
            _concatenationService = concatenationService;
        }

        private void InitializeStorage(StorageType sourceStorageType, StorageType destinationStorageType)
        {
            IStorageFactory sourceFactory = _storageFactoryFactory.CreateStorageFactory(TargetStorage.Source);
            IStorageFactory destinationFactory = _storageFactoryFactory.CreateStorageFactory(TargetStorage.Destination);
            var storageConfigModel = _configurationService.GetStorageConfigModel();
            _sourceStorageService = sourceFactory.CreateStorageService(sourceStorageType, storageConfigModel);
            _destinationStorageService = destinationFactory.CreateStorageService(destinationStorageType, storageConfigModel);
        }

        public async Task Predict(StorageType sourceStorageType, StorageType destinationStorageType, ChunkMethod chunkType, CognitiveServiceType service)
        {
            InitializeStorage(sourceStorageType, destinationStorageType);
            var charLimit = _configurationService.GetChunkerConfigModel().CharLimit;
            var chunkLevel = _configurationService.GetChunkerConfigModel().ChunkSectionLevel;
            var defaultOps = _configurationService.GetTextAnalyticsConfigModel().DefaultOperations;
            var convertedFiles = new ConcurrentBag<string>();
            var failedFiles = new ConcurrentDictionary<string, string>();
            // Check which service to run
            var runCustomText = CognitiveServiceType.CustomText.Equals(service) || CognitiveServiceType.Both.Equals(service);
            var runTextAnalytics = service == CognitiveServiceType.TextAnalytics || service == CognitiveServiceType.Both;

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
                    // prediction service
                    _loggerService.LogOperation(OperationType.RunningPrediction, fileName);
                    var queries = chunkedText.Select(r => r.Text).ToList();
                    var customTextresponse = runCustomText ? await _customTextPredictionService.GetPredictionBatchAsync(queries) : null;
                    var sentimentResponse = runTextAnalytics && defaultOps.Sentiment ? await _textAnalyticsPredictionService.PredictSentimentBatchAsync(queries) : null;
                    var nerResponse = runTextAnalytics && defaultOps.Ner ? await _textAnalyticsPredictionService.PredictNerBatchAsync(queries) : null;
                    var keyphraseResponse = runTextAnalytics && defaultOps.Keyphrase ? await _textAnalyticsPredictionService.PredictKeyphraseBatchAsync(queries) : null;
                    // concatenation service
                    var concatenatedResponse = _concatenationService.ConcatPredictionResult(chunkedText.ToArray(), customTextresponse, sentimentResponse, nerResponse, keyphraseResponse);
                    var responseAsJson = JsonConvert.SerializeObject(concatenatedResponse, Formatting.Indented);
                    // store file
                    _loggerService.LogOperation(OperationType.StoringResult, fileName);
                    var newFileName = Path.GetFileNameWithoutExtension(fileName) + ".json";
                    await _destinationStorageService.StoreDataAsync(responseAsJson, newFileName);
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