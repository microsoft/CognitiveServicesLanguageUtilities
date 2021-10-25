// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
using Azure.AI.TextAnalytics;
using Microsoft.IAPUtilities.Definitions.APIs.Controllers;
using Microsoft.IAPUtilities.Definitions.APIs.Services;
using Microsoft.IAPUtilities.Definitions.Models.Luis;
using Newtonsoft.Json;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.IAPUtilities.Core.Controllers
{
    public class IAPProccessController : IIAPProccessController
    {
        IStorageService _storageService;
        ITranscriptParser _transcriptParser;
        ILuisPredictionService _luisPredictionService;
        IIAPResultGenerator _resultGenerator;
        ITextAnalyticsService _textAnalyticsService;

        public IAPProccessController(
            IStorageService storageService,
            ITranscriptParser transcriptParser,
            ILuisPredictionService luisPredictionService,
            IIAPResultGenerator transcriptGenerator,
            ITextAnalyticsService textAnalyticsService)
        {
            _storageService = storageService;
            _transcriptParser = transcriptParser;
            _luisPredictionService = luisPredictionService;
            _resultGenerator = transcriptGenerator;
            _textAnalyticsService = textAnalyticsService;
        }

        public async Task Run()
        {
            // read files from dir
            var files = await _storageService.ListFilesAsync();

            // loop on files
            var fileTasks = files.Select(async file =>
            {
                //  parse file (extract utterances)
                var fileStream = await _storageService.ReadFileAsync(file);
                var transcript = await _transcriptParser.ParseTranscriptAsync(fileStream);

                var luisDictionary = new ConcurrentDictionary<long, CustomLuisResponse>();
                var textAnalyticsDictionary = new ConcurrentDictionary<long, DocumentSentiment>();
                var tasks = transcript.Utterances.Select(async utterance =>
                {
                    // run luis prediction endpoint
                    luisDictionary[utterance.Timestamp] = await _luisPredictionService.Predict(utterance.Text);
                    // run TA prediction endpoint
                    textAnalyticsDictionary[utterance.Timestamp] = await _textAnalyticsService.PredictSentimentAsync(utterance.Text, opinionMining: true);
                });
                await Task.WhenAll(tasks);

                // concatenate result
                var processedTranscript = _resultGenerator.GenerateResult(luisDictionary, textAnalyticsDictionary, transcript.Channel, transcript.Id);

                // write result file
                var outString = JsonConvert.SerializeObject(processedTranscript, Formatting.Indented);
                await _storageService.StoreDataAsync(outString, "test.json");
            });
            await Task.WhenAll(fileTasks);
        }
    }
}
