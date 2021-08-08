using FuzzyMatching.Definitions.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FuzzyMatching.Definitions
{
    public interface IFuzzyMatchingClient
    {
        public Task PreprocessDatasetAsync(List<string> dataset, string datasetName, string relativeDirectory);
        public Task<List<MatchingResult>> MatchSentenceAsync(string sentence, string datasetName, float similarityThreshold, string relativeDirectory);
        public string[] ListProcessedDatasets(string relativeDirectory);
    }
}
