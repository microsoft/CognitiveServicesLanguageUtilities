using FileFormatConverter.Runner;
using FileFormatConverter.Runner.DataStructures;
using System;
using Xunit;

namespace FileFormatConverter.Tests
{
    public class OperationRunnerTest
    {
        public static TheoryData BlobValidationServiceAsyncTestData()
        {
            return new TheoryData<string, FileType, string, FileType, bool>
            {
                {
                    @"C:\Users\mshaban\Desktop\cli tool\file samples\source-labeled_datapoints.jsonl",
                    FileType.JSONL,
                    @"C:\Users\mshaban\Desktop\cli tool\file samples\test.json",
                    FileType.CT_ENTITIES,
                    true
                }
            };
        }

        [Theory]
        [MemberData(nameof(BlobValidationServiceAsyncTestData))]
        public void TestOperationRunner(string sourceFilePath, FileType sourceFileType, string targetFilePath, FileType targetFileType, bool isValid)
        {
            if (isValid)
            {
                FileConversionOperationRunner.RunOperation(sourceFilePath, sourceFileType, targetFilePath, targetFileType);
            }
            else
            {
                Assert.Throws<Exception>(() => FileConversionOperationRunner.RunOperation(sourceFilePath, sourceFileType, targetFilePath, targetFileType));
            }
        }
    }
}
