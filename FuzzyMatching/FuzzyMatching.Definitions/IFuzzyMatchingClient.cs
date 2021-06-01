using FuzzyMatching.Definitions.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FuzzyMatching.Definitions
{
    public interface IFuzzyMatchingClient
    {
        public Task PreprocessDatasetAsync(List<string> dataset, string datasetName, string relativeDirectory);
        public Task<MatchingResult> MatchSentenceAsync(string sentence, string datasetName, string relativeDirectory);
        public string[] ListProcessedDatasets(string relativeDirectory);
    }
}
