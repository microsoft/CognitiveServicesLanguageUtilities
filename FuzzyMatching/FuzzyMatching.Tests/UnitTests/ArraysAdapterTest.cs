using FuzzyMatching.Core.Utilities.MultiDimensionalArrays;
using Xunit;

namespace FuzzyMatching.Tests.UnitTests
{
    public class ArraysAdapterTest
    {
        public static TheoryData TestLocalStorageServiceArrays()
        {

            float[][] array2D =
{
new float[] { 0, 1, 2,3 },
new float[] { 0, 4, 5 ,0},
new float[] { 0, 0,6,0 }
};




            return new TheoryData<float[][], int, int>
            {
                {
                    array2D,
                    3,
                    4
                }
            };
        }

        [Theory]
        [MemberData(nameof(TestLocalStorageServiceArrays))]
        public void TestLocalStorageService(float[][] array2D, int height, int width)
        {
            var spreadArray = ArrayDimensionConverter.Convert2DArrayMatrixTo1DArray(array2D);
            var unrolledMatrix = ArrayDimensionConverter.Convert1DArrayTo2DArrayMatrix(spreadArray, height, width);
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    Assert.Equal(array2D[i][j], unrolledMatrix[i][j]);
                }
            }

        }
    }
}
