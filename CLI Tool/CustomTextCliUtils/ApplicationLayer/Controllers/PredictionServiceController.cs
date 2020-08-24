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
using CustomTextCliUtils.ApplicationLayer.Modeling.Models.Chunker;

namespace CustomTextCliUtils.ApplicationLayer.Controllers
{
    class PredictionServiceController
    {
        readonly IConfigsLoader _configurationService;
        readonly IStorageFactoryFactory _storageFactoryFactory;
        readonly IParserService _parserService;
        IStorageService _sourceStorageService;
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
                List<ChunkInfo> chunkedText = _chunkerService.Chunk(parseResult, chunkType, charLimit);
                // run prediction
                var chunkPredictionResults = new List<CustomTextPredictionChunkInfo>();
                foreach (var item in chunkedText.Select((value, i) => (value, i)))
                {
                    var customTextPredictionResponse = await _predictionService.PredictAsync(item.value.Text);
                    var chunkInfo = new CustomTextPredictionChunkInfo { 
                        ChunkNumber = item.i,
                        CharCount = item.value.Text.Length,
                        CustomTextPredictionResponse = customTextPredictionResponse,
                        InnerText = item.value.Summary,
                        StartPage = item.value.StartPage,
                        EndPage = item.value.EndPage
                    };
                    chunkPredictionResults.Add(chunkInfo);
                }
                // store or display result
                _loggerService.LogOperation(OperationType.DisplayingResult, fileName);
                var concatenatedResult = JsonConvert.SerializeObject(chunkPredictionResults, Formatting.Indented); 
                _loggerService.Log(concatenatedResult);
            }
            catch (CliException e)
            {
                _loggerService.LogError(e);
            }
        }


    }
}
