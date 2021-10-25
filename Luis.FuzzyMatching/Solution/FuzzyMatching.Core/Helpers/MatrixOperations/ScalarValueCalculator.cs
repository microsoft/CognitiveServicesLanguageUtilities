// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System;
using System.Linq;

namespace Microsoft.Azure.CognitiveServices.Language.LUIS.Utilities.FuzzyMatching.Core.Helpers.MatrixOperations
{
    public class ScalarValueCalculator
    {
        public static float GetVectorAbsoluteValue(float[] v)
        {
            var sum = v.Select(value => value * value).Sum();
            return (float)Math.Sqrt(sum);
        }

        public static float[] GetVectorAbsoluteValueBatch(float[][] vectorList)
        {
            var task = vectorList.AsParallel().Select(vectorA => GetVectorAbsoluteValue(vectorA)).ToArray();
            return task;
        }

        public static float[] CalculateVectorAbsoluteValueBatch(float[][] matrix)
        {
            return matrix.AsParallel().Select(row => GetVectorAbsoluteValue(row)).ToArray();
        }
    }
}
