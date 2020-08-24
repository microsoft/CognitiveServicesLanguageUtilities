using CustomTextCliUtils.ApplicationLayer.Exceptions;
using CustomTextCliUtils.ApplicationLayer.Factories.Storage;
using CustomTextCliUtils.ApplicationLayer.Services.Logger;
using CustomTextCliUtils.ApplicationLayer.Services.Parser;
using CustomTextCliUtils.ApplicationLayer.Services.Storage;
using CustomTextCliUtils.Configs;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CustomTextCliUtils.ApplicationLayer.Services.Chunker;
using Newtonsoft.Json;
using CustomTextCliUtils.ApplicationLayer.Services.Prediction;
using CustomTextCliUtils.ApplicationLayer.Modeling.Enums.Misc;
using CustomTextCliUtils.ApplicationLayer.Modeling.Enums.Logger;
using CustomTextCliUtils.ApplicationLayer.Modeling.Models.Prediction;
using CustomTextCliUtils.ApplicationLayer.Modeling.Models.Parser;

namespace CustomTextCliUtils.ApplicationLayer.Controllers
{
    class PredictionServiceController
    {
        readonly IConfigsLoader _configurationService;
        readonly IStorageFactoryFactory _storageFactoryFactory;
        readonly IParserService _parserService;
        IStorageService _sourceStorageService;
        IStorageService _destinationStorageService;
        readonly ILoggerService _loggerService;
        readonly IChunkerService _chunkerService;
        readonly IPredictionService _predictionService;

        public PredictionServiceController(IConfigsLoader configurationService, IStorageFactoryFactory storageFactoryFactory,
            IParserService parserService, ILoggerService loggerService, IChunkerService chunkerService, IPredictionService predictionService)
        {
            _configurationService = configurationService;
            _storageFactoryFactory = storageFactoryFactory;
            _parserService = parserService;
            _loggerService = loggerService;
            _chunkerService = chunkerService;
            _predictionService = predictionService;
        }

        private void InitializeStorage(StorageType sourceStorageType, StorageType destinationStorageType)
        {
            IStorageFactory sourceFactory = _storageFactoryFactory.CreateStorageFactory(TargetStorage.Source);
            IStorageFactory destinationFactory = _storageFactoryFactory.CreateStorageFactory(TargetStorage.Destination);
            var storageConfigModel = _configurationService.GetStorageConfigModel();
            _sourceStorageService = sourceFactory.CreateStorageService(sourceStorageType, storageConfigModel);
            _destinationStorageService = destinationFactory.CreateStorageService(destinationStorageType, storageConfigModel);
        }

        public async Task Predict(StorageType sourceStorageType, StorageType destinationStorageType, string fileName, ChunkMethod chunkType)
        {
            // initialize storage
            InitializeStorage(sourceStorageType, destinationStorageType);
            var charLimit = _configurationService.GetChunkerConfigModel().CharLimit;

            // prediction task
            try
            {
                // validate type
                _parserService.ValidateFileType(Path.GetExtension(fileName));
                // read file
                _loggerService.LogOperation(OperationType.ReadingFile, fileName);
                Stream file = await _sourceStorageService.ReadFile(fileName);
                // parse file
                _loggerService.LogOperation(OperationType.ParsingFile, fileName);
                ParseResult parseResult = await _parserService.ParseFile(file);
                // chunk file
                _loggerService.LogOperation(OperationType.ChunkingFile, fileName);
                List<string> chunkedText = _chunkerService.Chunk(parseResult, chunkType, charLimit);
                // run prediction
                var chunkPredictionResults = new List<CustomTextPredictionResponse>();
                foreach (var item in chunkedText.Select((value, i) => (value, i)))
                {
                    var chunkPredictionResult = await _predictionService.PredictAsync(item.value);
                    chunkPredictionResults.Add(chunkPredictionResult);
                }
                // store or display result
                var concatenatedResult = JsonConvert.SerializeObject(chunkPredictionResults, Formatting.Indented);
                _loggerService.LogOperation(OperationType.StoringResult, fileName);
                _loggerService.Log(concatenatedResult);
                //_destinationStorageService.StoreData(item.value, newFileName);
            }
            catch (CliException e)
            {
                _loggerService.LogError(e);
            }
        }


    }
}
