// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
﻿using Microsoft.CogSLanguageUtilities.Core.Helpers.Mappers.EvaluationNuget;
using Microsoft.CogSLanguageUtilities.Definitions.APIs.Configs;
using Microsoft.CogSLanguageUtilities.Definitions.APIs.Controllers;
using Microsoft.CogSLanguageUtilities.Definitions.APIs.Factories.Storage;
using Microsoft.CogSLanguageUtilities.Definitions.APIs.Services;
using Microsoft.CogSLanguageUtilities.Definitions.Configs.Consts;
using Microsoft.CogSLanguageUtilities.Definitions.Models.CustomText.Api.LabeledExamples.Response;
using Microsoft.CogSLanguageUtilities.Definitions.Models.Enums.Logger;
using Microsoft.CogSLanguageUtilities.Definitions.Models.Enums.Storage;
using Microsoft.LuisModelEvaluation.Models.Input;
using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Microsoft.CogSLanguageUtilities.Core.Controllers
{
    public class BatchTestingController : IBatchTestingController
    {
        private readonly IConfigsLoader _configurationService;
        private readonly IStorageFactoryFactory _storageFactoryFactory;
        private IStorageService _sourceStorageService;
        private IStorageService _destinationStorageService;
        private readonly ILoggerService _loggerService;
        private readonly ICustomTextPredictionService _customTextPredictionService;
        private readonly IBatchTestingService _batchTestingService;
        private readonly ICustomTextAuthoringService _customTextAuthoringService;

        public BatchTestingController(
            IConfigsLoader configurationService,
            IStorageFactoryFactory storageFactoryFactory,
            ILoggerService loggerService,
            ICustomTextPredictionService CustomTextPredictionService,
            ICustomTextAuthoringService customTextAuthoringService,
            IBatchTestingService batchTestingService)
        {
            _configurationService = configurationService;
            _storageFactoryFactory = storageFactoryFactory;
            _loggerService = loggerService;
            _customTextPredictionService = CustomTextPredictionService;
            _customTextAuthoringService = customTextAuthoringService;
            _batchTestingService = batchTestingService;
        }

        private void InitializeStorage(StorageType sourceStorageType, StorageType destinationStorageType)
        {
            IStorageFactory sourceFactory = _storageFactoryFactory.CreateStorageFactory(TargetStorage.Source);
            IStorageFactory destinationFactory = _storageFactoryFactory.CreateStorageFactory(TargetStorage.Destination);
            var storageConfigModel = _configurationService.GetStorageConfigModel();
            _sourceStorageService = sourceFactory.CreateStorageService(sourceStorageType, storageConfigModel);
            _destinationStorageService = destinationFactory.CreateStorageService(destinationStorageType, storageConfigModel);
        }

        public async Task EvaluateCustomTextAppAsync(StorageType sourceStorageType, StorageType destinationStorageType)
        {
            InitializeStorage(sourceStorageType, destinationStorageType);

            // init result containers
            var convertedFiles = new ConcurrentBag<string>();
            var failedFiles = new ConcurrentDictionary<string, string>();

            // map app models to nuget classes
            var customTextModels = await _customTextAuthoringService.GetApplicationModels();
            var mappedEntities = BatchTestingInputMapper.MapCustomTextAppEntityModels(customTextModels.Models, entityPrefix: string.Empty);
            var mappedClasses = BatchTestingInputMapper.MapCustomTextAppClassModels(customTextModels.Models);

            // get labeled examples
            _loggerService.LogOperation(OperationType.GeneratingTestSet);
            var labeledExamples = await _customTextAuthoringService.GetLabeledExamples();
            var mappedTestData = await CreateTestData(labeledExamples, convertedFiles, failedFiles);

            // evaluate model
            _loggerService.LogOperation(OperationType.EvaluatingResults);
            var batchTestResponse = _batchTestingService.RunBatchTest(mappedTestData, mappedEntities, mappedClasses);

            // map output data
            var mappedBatchTestResponse = BatchTestingOutputMapper.MapEvaluationOutput(batchTestResponse);

            // store file
            var outFileName = Constants.CustomTextEvaluationControllerOutputFileName;
            _loggerService.LogOperation(OperationType.StoringResult, outFileName);
            var responseAsJson = JsonConvert.SerializeObject(mappedBatchTestResponse, Formatting.Indented);
            await _destinationStorageService.StoreDataAsync(responseAsJson, outFileName);

            // log result
            _loggerService.LogParsingResult(convertedFiles, failedFiles);
        }

        private async Task<List<TestingExample>> CreateTestData(CustomTextGetLabeledExamplesResponse labeledExamples, ConcurrentBag<string> convertedFiles, ConcurrentDictionary<string, string> failedFiles)
        {
            var modelsDictionary = await _customTextAuthoringService.GetModelsDictionary();
            var testingExamples = new List<TestingExample>();
            await _destinationStorageService.CreateDirectoryAsync(Constants.EvaluationCommandPredictionOutputDirectoryName);
            foreach (var labeledExample in labeledExamples.Examples)
            {
                try
                {
                    // document text
                    var documentText = await _sourceStorageService.ReadFileAsStringAsync(labeledExample.Document.DocumentId);

                    // prediction
                    _loggerService.LogOperation(OperationType.RunningPrediction, labeledExample.Document.DocumentId);
                    var predictionResponse = await _customTextPredictionService.GetPredictionAsync(documentText);

                    // store prediction output
                    var predictionResponseString = JsonConvert.SerializeObject(predictionResponse, Formatting.Indented);
                    var jsonFileName = Path.GetFileNameWithoutExtension(labeledExample.Document.DocumentId) + ".json";
                    await _destinationStorageService.StoreDataToDirectoryAsync(predictionResponseString, Constants.EvaluationCommandPredictionOutputDirectoryName, jsonFileName);

                    // create test example
                    var testExample = BatchTestingInputMapper.CreateTestExample(documentText, labeledExample, predictionResponse, modelsDictionary);
                    testingExamples.Add(testExample);


                    convertedFiles.Add(labeledExample.Document.DocumentId);
                }
                catch (Exception ex)
                {
                    failedFiles[labeledExample.Document.DocumentId] = ex.Message;
                    _loggerService.LogError(ex);
                }
            }
            return testingExamples;
        }
    }
}
