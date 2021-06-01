using FuzzyMatching.Core.Utilities.MultiDimensionalArrays;
using FuzzyMatching.Definitions.Models;

namespace FuzzyMatching.Core.Utilities.ModelConverters
{
    public static class ProcessedDatasetModelConverter
    {
        public static StoredProcessedDataset ProcessedToStored(ProcessedDataset processedDataset)
        {
            var storedDataset = new StoredProcessedDataset();
            storedDataset.TFIDFMatrix = ArrayDimensionConverter.Convert2DArrayMatrixTo1DArray(processedDataset.TFIDFMatrix);
            storedDataset.TFIDFMatrixAbsoluteValues = processedDataset.TFIDFMatrixAbsoluteValues;
            storedDataset.IDFVector = processedDataset.IDFVector;
            storedDataset.UniqueNGramsVector = processedDataset.UniqueNGramsVector;
            storedDataset.Height = processedDataset.TFIDFMatrix.Length;
            storedDataset.Width = processedDataset.TFIDFMatrix[0].Length;
            return storedDataset;
        }
        public static ProcessedDataset StoredToProcessed(StoredProcessedDataset storedDataset)
        {
            var processedDataset = new ProcessedDataset();
            processedDataset.TFIDFMatrix = ArrayDimensionConverter.Convert1DArrayTo2DArrayMatrix(storedDataset.TFIDFMatrix, storedDataset.Height, storedDataset.Width);
            processedDataset.TFIDFMatrixAbsoluteValues = storedDataset.TFIDFMatrixAbsoluteValues;
            processedDataset.IDFVector = storedDataset.IDFVector;
            processedDataset.UniqueNGramsVector = storedDataset.UniqueNGramsVector;

            return processedDataset;
        }


    }
}
