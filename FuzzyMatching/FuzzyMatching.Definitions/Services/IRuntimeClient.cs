using FuzzyMatching.Definitions.Models;
using System.Collections.Generic;

namespace FuzzyMatching.Definitions.Services
{
    public interface IRuntimeClient
    {
        public MatchingResult MatchSentence(string sentence, ProcessedDataset processedDataset, List<string> dataset, int ngramsLength);
    }
}
