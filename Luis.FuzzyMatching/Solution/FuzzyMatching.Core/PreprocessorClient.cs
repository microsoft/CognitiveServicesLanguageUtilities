// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using Microsoft.Azure.CognitiveServices.Language.LUIS.Utilities.FuzzyMatching.Core.Helpers;
using Microsoft.Azure.CognitiveServices.Language.LUIS.Utilities.FuzzyMatching.Core.Helpers.MatrixOperations;
using Microsoft.Azure.CognitiveServices.Language.LUIS.Utilities.FuzzyMatching.Core.Services.TFIDFControllers;
using Microsoft.Azure.CognitiveServices.Language.LUIS.Utilities.FuzzyMatching.Definitions.Clients;
using Microsoft.Azure.CognitiveServices.Language.LUIS.Utilities.FuzzyMatching.Definitions.Models;
using System.Collections.Generic;

namespace Microsoft.Azure.CognitiveServices.Language.LUIS.Utilities.FuzzyMatching.Core
{
    public class PreprocessorClient : IPreprocessorClient
    {
        public ProcessedDataset ProcessDataset(List<string> dataset, int ngramSize = default)
        {
            // calculate TF, IDF, and TFIDF values
            TFIDFController.CalculateDatasetTFIDFValues(dataset, out string[] uniqueNGramsVector, out float[] datasetIDFVector, out float[][] datasetTFIDFMatrix, ngramSize: ngramSize);

            // get TFIDF scalar values for each sentence
            var dataseetTFIDFMatrixAbsoluteValues = ScalarValueCalculator.CalculateVectorAbsoluteValueBatch(datasetTFIDFMatrix);

            // calculate maximum number of words in a datapoint within the dataset
            var maximumWordCount = StringTokenizer.FindMaxWordCount(dataset);

            // return
            return new ProcessedDataset()
            {
                TFIDFMatrixAbsoluteValues = dataseetTFIDFMatrixAbsoluteValues,
                TFIDFMatrix = datasetTFIDFMatrix,
                IDFVector = datasetIDFVector,
                UniqueNGramsVector = uniqueNGramsVector,
                MaximumWordCount = maximumWordCount,
            };
        }
    }
}
