using MathNet.Numerics.LinearAlgebra;
using System;
using System.Linq;

namespace FuzzyMatching.Core.Utilities.MatrixOperations
{
    public static class DotProductCalculator
    {
        public static float[] CalculateDotProduct(float[] vector, float vectorAbs, float[][] matrix, float[] matrixAbs)
        {
            return matrix.AsParallel().Select((row, index) => CalculateDotProduct(vector, row, vectorAbs, matrixAbs[index])).ToArray();
        }

        public static float[] CalculateVectorAbsoluteValueBatch(float[][] matrix)
        {
            return matrix.AsParallel().Select(row => GetVectorAbsoluteValue(row)).ToArray();
        }
        private static float CalculateDotProduct(float[] v1, float[] v2, float v1Abs, float v2Abs)
        {
            var multiplicationSum = GetMultiplicationSum(v1, v2);
            return v1Abs * v2Abs / multiplicationSum;
        }
        private static float GetMultiplicationSum(float[] v1, float[] v2)
        {
            var vA = Vector<float>.Build.Dense(v1);
            var vB = Vector<float>.Build.Dense(v2);
            var result2 = vA.DotProduct(vB);
            return result2;
        }

        public static float GetVectorAbsoluteValue(float[] v)
        {
            var sum = v.Select(value => value * value).Sum();
            return (float)Math.Sqrt(sum);
        }
    }
}
