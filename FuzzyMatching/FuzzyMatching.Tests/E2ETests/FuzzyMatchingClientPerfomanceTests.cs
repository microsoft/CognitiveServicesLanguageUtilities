using FuzzyMatching.Core;
using FuzzyMatching.Definitions.Models;
using FuzzyMatching.Definitions.Models.Enums;
using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace FuzzyMatching.Tests.E2ETests
{
    public class FuzzyMatchingClientPerfomanceTests
    {
        public static TheoryData FuzzyMatchingClientTestData()
        {
            // prepare input
            var datasetLocation = @".\TestData\largeDataset.csv";


            var sentenceToMatch = "take record";
            var storageOptions = new StorageOptions
            {
                StorageType = StorageType.Local,
                BaseDirectory = @".",
                ConnectionString = Environment.GetEnvironmentVariable("CONNECTION_STRING_BLOB", EnvironmentVariableTarget.Machine),
                ContainerName = "container"
            };



            int[] sizes = new int[6] { 10, 100, 1000, 10000, 25000, 50000 };

            return new TheoryData<int[], string, string, StorageOptions>
            {
                {
                    sizes,
                    datasetLocation,
                    sentenceToMatch,
                    storageOptions
                }
            };
        }

        [Theory]
        [MemberData(nameof(FuzzyMatchingClientTestData))]
        public async Task FuzzyMatchingClientTestAsync(int[] sizes, string datasetLocation, string sentenceToMatch, StorageOptions storageOptions)
        {
            foreach (var size in sizes)
            {
                // read dataset
                var dataset = ReadDatasetFromCSV(datasetLocation, size);
                // create client
                var fuzzyMatchingClient = new FuzzyMatchingClient(storageOptions);

                // process dataset
                var datasetName = "someDataset";
                await fuzzyMatchingClient.PreprocessDatasetAsync(dataset, datasetName);

                DateTime start = DateTime.Now;
                // runtime
                var result = fuzzyMatchingClient.MatchSentenceAsync(sentenceToMatch, datasetName).GetAwaiter().GetResult();
                DateTime end = DateTime.Now;
                TimeSpan ts = (end - start);

                // print time
                Console.WriteLine("Elapsed Time for the program with size {0} is {1} s", size, ts.TotalSeconds);
            }
        }

        private static List<string> ReadDatasetFromCSV(string filePath, int limit = 30000)
        {
            // init result
            var result = new List<string>();

            // init parser
            var parser = new TextFieldParser(filePath);
            parser.TextFieldType = FieldType.Delimited;
            parser.SetDelimiters(new string[] { "," });
            parser.ReadFields(); // skip first line -> headers

            // read data
            var counter = 1;
            while (!parser.EndOfData && counter <= limit)
            {
                var row = parser.ReadFields(); // string[]
                result.Add(row[1]);
                counter++;
            }

            return result;
        }
    }
}
