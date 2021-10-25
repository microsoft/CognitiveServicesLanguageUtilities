// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

namespace Microsoft.Azure.CognitiveServices.Language.LUIS.Utilities.FuzzyMatching.Definitions.Models
{
    public class ProcessedDataset
    {
        public float[][] TFIDFMatrix { get; set; }
        public float[] TFIDFMatrixAbsoluteValues { get; set; }
        public float[] IDFVector { get; set; }
        public string[] UniqueNGramsVector { get; set; }
        public int MaximumWordCount { get; set; }
    }
}
