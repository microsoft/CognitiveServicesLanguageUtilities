using System.Collections.Generic;

namespace FuzzyMatching.Core.Utilities.MultiDimensionalArrays
{
    public static class ArrayDimensionConverter
    {
        public static float[][] Convert1DArrayTo2DArrayMatrix(float[] source1DArray, int targetHeight, int targetWidth)
        {
            float[,] output = new float[targetHeight, targetWidth];
            // i -> k/cols
            //j -> k mod cols 
            var k = 0;
            var index = 0;
            while (index < source1DArray.Length)
            {
                if (source1DArray[index] == 0)
                {
                    k += (int)source1DArray[index + 1];
                    index += 2;
                }
                else
                {
                    var i = k / targetWidth; // floor (integer division )
                    var j = k % targetWidth; // (integer )
                    output[i, j] = source1DArray[index];
                    k++;

                    index++;
                }

            }
            var result = ToJaggedArray(output);
            return result;
        }
        public static float[] Convert2DArrayMatrixTo1DArray(float[][] source2DArrayMatrix)
        {
            var rows = source2DArrayMatrix.Length;
            var columns = source2DArrayMatrix[0].Length;
            var result = new List<float>();


            // Step 2: copy 2D array elements into a 1D array.
            var i = 0;
            var j = 0;
            while (i < rows && j < columns)
            {

                if (source2DArrayMatrix[i][j] == 0)
                {
                    var counter = 1;
                    while (IncrementColumnPointer(columns, rows, ref i, ref j) && source2DArrayMatrix[i][j] == 0)
                    {
                        counter++;
                    }

                    result.Add(0);
                    result.Add(counter);
                }
                else
                {
                    result.Add(source2DArrayMatrix[i][j]);

                    if (!IncrementColumnPointer(columns, rows, ref i, ref j))
                        break;
                }


            }
            // Step 3: return the new array.

            return result.ToArray();
        }

        /// <summary>
        /// This method increments i, j (row and column pointers)
        /// when iterating through a matrix
        /// </summary>
        /// <param name="length"> number of rows or columns </param>
        /// <param name="j"> pointer i or j </param>
        /// <returns></returns>
        private static bool IncrementColumnPointer(int columns, int rows, ref int i, ref int j)
        {
            j++;
            if (j >= columns)
            {
                j = 0;
                i++;
            }
            return i < rows;

        }
        private static T[][] ToJaggedArray<T>(T[,] twoDimensionalArray)
        {
            int rowsFirstIndex = twoDimensionalArray.GetLowerBound(0);
            int rowsLastIndex = twoDimensionalArray.GetUpperBound(0);
            int numberOfRows = rowsLastIndex + 1;

            int columnsFirstIndex = twoDimensionalArray.GetLowerBound(1);
            int columnsLastIndex = twoDimensionalArray.GetUpperBound(1);
            int numberOfColumns = columnsLastIndex + 1;

            T[][] jaggedArray = new T[numberOfRows][];
            for (int i = rowsFirstIndex; i <= rowsLastIndex; i++)
            {
                jaggedArray[i] = new T[numberOfColumns];

                for (int j = columnsFirstIndex; j <= columnsLastIndex; j++)
                {
                    jaggedArray[i][j] = twoDimensionalArray[i, j];
                }
            }
            return jaggedArray;
        }
    }
}
