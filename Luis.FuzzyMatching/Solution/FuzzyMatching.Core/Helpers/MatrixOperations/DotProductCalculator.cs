// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using MathNet.Numerics.LinearAlgebra;
using System.Linq;

namespace Microsoft.Azure.CognitiveServices.Language.LUIS.Utilities.FuzzyMatching.Core.Helpers.MatrixOperations
{
    public static class DotProductCalculator
    {
        public static float[][] GetDotProduct(float[][] matrix1, float[][] matrix2, float[] matrix1Abs = null, float[] matrix2Abs = null)
        {
            // need to handle case where matrix2Abs is null
            var matrix1AbsoluteValue = matrix1Abs ?? ScalarValueCalculator.GetVectorAbsoluteValueBatch(matrix1);
            var matrix2AbsoluteValue = matrix2Abs ?? ScalarValueCalculator.GetVectorAbsoluteValueBatch(matrix2);

            return matrix1.AsParallel().Select((matrix1RowVector, index) => GetDotProduct(matrix1RowVector, matrix2, vectorAbs: matrix1AbsoluteValue[index], matrixAbs: matrix2AbsoluteValue)).ToArray();
        }

        public static float[] GetDotProduct(float[] vector, float[][] matrix, float? vectorAbs = null, float[] matrixAbs = null)
        {
            // need to handle case where matrixAbs is null
            var vectorAbsoluteValue = vectorAbs == null ? ScalarValueCalculator.GetVectorAbsoluteValue(vector) : (float)vectorAbs;
            var matrixAbsoluteValue = matrixAbs ?? ScalarValueCalculator.GetVectorAbsoluteValueBatch(matrix);

            return matrix.AsParallel().Select((row, index) => CalculateDotProductInternal(vector, row, vectorAbsoluteValue, matrixAbsoluteValue[index])).ToArray();
        }

        private static float CalculateDotProductInternal(float[] v1, float[] v2, float v1Abs, float v2Abs)
        {
            var multiplicationSum = GetMultiplicationSumInternal(v1, v2);
            return multiplicationSum / (v1Abs * v2Abs);
        }

        private static float GetMultiplicationSumInternal(float[] v1, float[] v2)
        {
            var vA = Vector<float>.Build.Dense(v1);
            var vB = Vector<float>.Build.Dense(v2);
            var result2 = vA.DotProduct(vB);
            return result2;
        }
    }
}
