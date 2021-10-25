// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using MathNet.Numerics.LinearAlgebra;
using System.Linq;

namespace Microsoft.Azure.CognitiveServices.Language.LUIS.Utilities.FuzzyMatching.Core.Helpers.MatrixOperations
{

    public static class MultiplicationCalculator
    {
        public static float[] MultiplyVectorsByCell(float[] vector1, float[] vector2)
        {
            var v1 = Vector<float>.Build.Dense(vector1);
            var v2 = Vector<float>.Build.Dense(vector2);

            var res = v1.PointwiseMultiply(v2);
            var result = res.ToArray();
            return result;
        }

        public static float[][] MultiplyVectorsByCellBatch(float[][] vectorsList, float[] vector2)
        {
            var task = vectorsList.AsParallel().Select(vector1 => MultiplyVectorsByCell(vector1, vector2)).ToArray();
            return task;
        }
    }
}
