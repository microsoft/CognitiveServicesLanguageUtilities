using MathNet.Numerics.LinearAlgebra;
using System.Linq;

namespace FuzzyMatching.Core.Utilities.MatrixArithmetics
{

    public static class CellOperations
    {
        public static float[] MultiplyVectorCells(float[] vectorA, float[] vectorB)
        {
            var vA = Vector<float>.Build.Dense(vectorA);
            var vB = Vector<float>.Build.Dense(vectorB);

            var res = vA.PointwiseMultiply(vB);
            var result = res.ToArray();
            return result;
        }

        public static float[][] MultiplyVectorCellsBatch(float[][] vectorAsList, float[] vectorB)
        {
            var task = vectorAsList.AsParallel().Select(vectorA => MultiplyVectorCells(vectorA, vectorB)).ToArray();
            return task;
        }
    }
}
