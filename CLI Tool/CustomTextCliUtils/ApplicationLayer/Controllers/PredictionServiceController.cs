using Microsoft.CustomTextCliUtils.ApplicationLayer.Exceptions;
using Microsoft.CustomTextCliUtils.ApplicationLayer.Factories.Storage;
using Microsoft.CustomTextCliUtils.ApplicationLayer.Services.Logger;
using Microsoft.CustomTextCliUtils.ApplicationLayer.Services.Parser;
using Microsoft.CustomTextCliUtils.ApplicationLayer.Services.Storage;
using Microsoft.CustomTextCliUtils.Configs;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.CustomTextCliUtils.ApplicationLayer.Services.Chunker;
using Newtonsoft.Json;
using Microsoft.CustomTextCliUtils.ApplicationLayer.Services.Prediction;
using Microsoft.CustomTextCliUtils.ApplicationLayer.Modeling.Enums.Misc;
using Microsoft.CustomTextCliUtils.ApplicationLayer.Modeling.Enums.Logger;
using Microsoft.CustomTextCliUtils.ApplicationLayer.Modeling.Models.Prediction;
using Microsoft.CustomTextCliUtils.ApplicationLayer.Modeling.Models.Parser;
using Microsoft.CustomTextCliUtils.ApplicationLayer.Modeling.Models.Chunker;

namespace Microsoft.CustomTextCliUtils.ApplicationLayer.Controllers
{
    public class PredictionServiceController
    {
        private readonly IConfigsLoader _configurationService;
        private readonly IStorageFactoryFactory _storageFactoryFactory;
        private readonly IParserService _parserService;
        private IStorageService _sourceStorageService;
        private readonly ILoggerService _loggerService;
        private readonly IChunkerService _chunkerService;
        private readonly IPredictionService _predictionService;

        public PredictionServiceController(
            IConfigsLoader configurationService,
            IStorageFactoryFactory storageFactoryFactory,
            IParserService parserService,
            ILoggerService loggerService,
            IChunkerService chunkerService,
            IPredictionService predictionService)
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
                Stream file = await _sourceStorageService.ReadFileAsync(fileName);
                // parse file
                _loggerService.LogOperation(OperationType.ParsingFile, fileName);
                ParseResult parseResult = await _parserService.ParseFile(file);
                // chunk file
                _loggerService.LogOperation(OperationType.ChunkingFile, fileName);
                List<ChunkInfo> chunkedText = _chunkerService.Chunk(parseResult, chunkType, charLimit);
                // run prediction
                var chunkPredictionResults = new List<CustomTextPredictionChunkInfo>();
                _loggerService.LogOperation(OperationType.RunningPrediction, fileName);
                foreach (var item in chunkedText.Select((value, i) => (value, i)))
                {
                    var customTextPredictionResponse = _predictionService.GetPrediction(item.value.Text);
                    var chunkInfo = new CustomTextPredictionChunkInfo
                    {
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
