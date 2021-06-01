using FuzzyMatching.Definitions.Models;
using System.Collections.Generic;

namespace FuzzyMatching.Definitions.Services
{
    public interface IPreprocessorClient
    {
        public ProcessedDataset PreprocessDataset(List<string> dataset);
    }
}
