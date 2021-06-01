using FuzzyMatching.Core.Factories;
using FuzzyMatching.Core.Services;
using FuzzyMatching.Core.Utilities.ModelConverters;
using FuzzyMatching.Definitions;
using FuzzyMatching.Definitions.Models;
using FuzzyMatching.Definitions.Services;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace FuzzyMatching.Core
{
    public class FuzzyMatchingClient : IFuzzyMatchingClient
    {
        private PreprocessorClient PreprocessorClient;
        private RuntimeClient RuntimeClient;
        private IStorageService StorageService;
        private float[] temp;

        public FuzzyMatchingClient(StorageOptions storageOptions)
        {
            StorageService = StorageFactory.create(storageOptions);
            PreprocessorClient = new PreprocessorClient();
            RuntimeClient = new RuntimeClient();
        }

        public async Task PreprocessDatasetAsync(List<string> dataset, string datasetName, string relativeDirectory = "")
        {
            // create feature matrix
            var processedDataset = PreprocessorClient.PreprocessDataset(dataset);
            temp = processedDataset.IDFVector;
            var storedDataset = ProcessedDatasetModelConverter.ProcessedToStored(processedDataset);
            // store preprocessed data
            await StorageService.StoreBinaryObjectAsync(storedDataset, datasetName + "_PreProcessed", relativeDirectory);
            await StorageService.StoreBinaryObjectAsync(dataset, datasetName + "_Dataset", relativeDirectory);
        }



        public async Task<MatchingResult> MatchSentenceAsync(string sentence, string datasetName, string relativeDirectory = "")
        {
            try
            {
                // try to get the preprocessed dataset
                var storedDataset = StorageService.LoadBinaryObjectAsync<StoredProcessedDataset>(datasetName + "_PreProcessed", relativeDirectory).GetAwaiter().GetResult();
                var dataset = StorageService.LoadBinaryObjectAsync<List<string>>(datasetName + "_Dataset", relativeDirectory).GetAwaiter().GetResult();
                var processedDataset = ProcessedDatasetModelConverter.StoredToProcessed(storedDataset);
                // run matching algorithm
                return RuntimeClient.MatchSentence(sentence, processedDataset, dataset);
            }
            catch (FileNotFoundException)
            {
                try
                {
                    // load original dataset
                    var dataset = StorageService.LoadBinaryObjectAsync<List<string>>(datasetName + "_Dataset", relativeDirectory).GetAwaiter().GetResult();
                    // run preprocessing
                    await PreprocessDatasetAsync(dataset, datasetName, relativeDirectory);
                    // load preprocessed
                    var preprocessedDataset = StorageService.LoadBinaryObjectAsync<ProcessedDataset>(datasetName + "_PreProcessed", relativeDirectory).GetAwaiter().GetResult();
                    // run matching algorithm
                    return RuntimeClient.MatchSentence(sentence, preprocessedDataset, dataset);
                }
                catch (FileNotFoundException)
                {
                    // this means original dataset wasn't found!
                    throw new FileNotFoundException();
                }
            }
        }

        public string[] ListProcessedDatasets(string directory)
        {
            return StorageService.ListPreprocessedDatasetsAsync(directory).GetAwaiter().GetResult();
        }
    }
}
