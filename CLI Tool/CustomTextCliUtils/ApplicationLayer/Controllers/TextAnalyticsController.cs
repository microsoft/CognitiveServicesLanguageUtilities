using Microsoft.CustomTextCliUtils.ApplicationLayer.Exceptions;
using Microsoft.CustomTextCliUtils.ApplicationLayer.Factories.Storage;
using Microsoft.CustomTextCliUtils.ApplicationLayer.Services.Logger;
using Microsoft.CustomTextCliUtils.ApplicationLayer.Services.Parser;
using Microsoft.CustomTextCliUtils.ApplicationLayer.Services.Storage;
using Microsoft.CustomTextCliUtils.Configs;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.CustomTextCliUtils.ApplicationLayer.Services.Chunker;
using Newtonsoft.Json;
using Microsoft.CustomTextCliUtils.ApplicationLayer.Modeling.Enums.Misc;
using Microsoft.CustomTextCliUtils.ApplicationLayer.Modeling.Enums.Logger;
using Microsoft.CustomTextCliUtils.ApplicationLayer.Services.TextAnalytics;
using System.Collections.Concurrent;
using Microsoft.CustomTextCliUtils.ApplicationLayer.Services.Concatenation;
using System.IO;

namespace Microsoft.CustomTextCliUtils.ApplicationLayer.Controllers
{
    public class TextAnalyticsController
    {
        private readonly IConfigsLoader _configurationService;
        private readonly IStorageFactoryFactory _storageFactoryFactory;
        private readonly IParserService _parserService;
        private IStorageService _sourceStorageService;
        private IStorageService _destinationStorageService;
        private readonly ILoggerService _loggerService;
        private readonly IChunkerService _chunkerService;
        private readonly ITextAnalyticsPredictionService _textAnalyticsPredictionService;
        private readonly IConcatenationService _concatenationService;

        public TextAnalyticsController(
            IConfigsLoader configurationService,
            IStorageFactoryFactory storageFactoryFactory,
            IParserService parserService,
            ILoggerService loggerService,
            IChunkerService chunkerService,
            ITextAnalyticsPredictionService textAnalyticsPredictionService,
            IConcatenationService concatenationService)
        {
            _configurationService = configurationService;
            _storageFactoryFactory = storageFactoryFactory;
            _parserService = parserService;
            _loggerService = loggerService;
            _chunkerService = chunkerService;
            _textAnalyticsPredictionService = textAnalyticsPredictionService;
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

        public async Task Predict(StorageType sourceStorageType, StorageType destinationStorageType, ChunkMethod chunkType)
        {
            // initialize storage
            InitializeStorage(sourceStorageType, destinationStorageType);
            var charLimit = _configurationService.GetChunkerConfigModel().CharLimit;
            var defaultOps = _configurationService.GetTextAnalyticsConfigModel().DefaultOperations;
            var convertedFiles = new ConcurrentBag<string>();
            var failedFiles = new ConcurrentDictionary<string, string>();

            // read files from source storage
            var fileNames = await _sourceStorageService.ListFilesAsync();
            // parse files
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
                    _loggerService.LogOperation(OperationType.ParsingFile, fileName);
                    var parseResult = await _parserService.ParseFile(file);
                    // chunk file
                    _loggerService.LogOperation(OperationType.ChunkingFile, fileName);
                    var chunkedText = _chunkerService.Chunk(parseResult, chunkType, charLimit);
                    // prediction service
                    _loggerService.LogOperation(OperationType.RunningPrediction, fileName);
                    var queries = chunkedText.Select(r => r.Text).ToList();
                    var sentimentResponse = defaultOps.Sentiment ? await _textAnalyticsPredictionService.PredictSentimentBatchAsync(queries) : null;
                    var nerResponse = defaultOps.Ner ? await _textAnalyticsPredictionService.PredictNerBatchAsync(queries) : null;
                    var keyphraseResponse = defaultOps.Keyphrase ? await _textAnalyticsPredictionService.PredictKeyphraseBatchAsync(queries) : null;
                    // concatenation service
                    var concatenatedResponse = _concatenationService.ConcatTextAnalytics(chunkedText.ToArray(), sentimentResponse, nerResponse, keyphraseResponse);
                    var responseAsJson = JsonConvert.SerializeObject(concatenatedResponse, Formatting.Indented);
                    // store file
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
