using FileFormatConverter.Runner;
using FileFormatConverter.Runner.DataStructures;
using Xunit;

namespace FileFormatConverter.Tests
{
    public class OperationRunnerTest
    {
        public static TheoryData BlobValidationServiceAsyncTestData()
        {
            return new TheoryData<string, FileType, string, FileType>
            {
                {
                    @"C:\Users\mshaban\Desktop\cli tool\file samples\source-labeled_datapoints.jsonl",
                    FileType.JSONL,
                    @"C:\Users\mshaban\Desktop\cli tool\file samples\test.json",
                    FileType.CT_ENTITIES
                }
            };
        }

        [Theory]
        [MemberData(nameof(BlobValidationServiceAsyncTestData))]
        public void TestOperationRunner(string sourceFilePath, FileType sourceFileType, string targetFilePath, FileType targetFileType)
        {


            FileConversionOperationRunner.RunOperation(sourceFilePath, sourceFileType, targetFilePath, targetFileType);
        }
    }
}
