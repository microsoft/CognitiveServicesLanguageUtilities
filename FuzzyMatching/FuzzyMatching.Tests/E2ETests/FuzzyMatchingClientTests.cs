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
    public class FuzzyMatchingClientTests
    {
        public static TheoryData FuzzyMatchingClientTestData()
        {
            // prepare input
            var datasetLocation = @".\TestData\NewsHeadlines.csv";
            var dataset = ReadDatasetFromCSV(datasetLocation);
            var randomSentenceIndex = 5;
            var sentenceToMatch = dataset[randomSentenceIndex];
            var storageOptions = new StorageOptions
            {
                StorageType = StorageType.Blob,
                BaseDirectory = @".",
                ConnectionString = Environment.GetEnvironmentVariable("CONNECTION_STRING_BLOB", EnvironmentVariableTarget.Machine),
                ContainerName = "container"
            };

            // expected
            var expected = new MatchingResult
            {
                ClosestSentence = dataset[randomSentenceIndex],
                MatchingIndex = randomSentenceIndex
            };


            return new TheoryData<List<string>, string, StorageOptions, MatchingResult>
            {
                {
                    dataset,
                    sentenceToMatch,
                    storageOptions,
                    expected
                }
            };
        }

        [Theory]
        [MemberData(nameof(FuzzyMatchingClientTestData))]
        public async Task FuzzyMatchingClientTestAsync(List<string> dataset, string sentenceToMatch, StorageOptions storageOptions, MatchingResult expected)
        {
            // create client
            var fuzzyMatchingClient = new FuzzyMatchingClient(storageOptions);

            // process dataset
            var datasetName = "someDataset";
           await fuzzyMatchingClient.PreprocessDatasetAsync(dataset, datasetName);

            // runtime
            var result = fuzzyMatchingClient.MatchSentenceAsync(sentenceToMatch, datasetName).GetAwaiter().GetResult();

            // assert
            Assert.Equal(result.ClosestSentence, expected.ClosestSentence);
            Assert.Equal(result.MatchingIndex, expected.MatchingIndex);

            // print result

            Console.WriteLine("sentence to match : {0}", sentenceToMatch);
            Console.WriteLine("Matched Sentence : {0}", result.ClosestSentence);
            Console.WriteLine("Matched Sentence Score : {0}", result.MatchingScore);
            Console.WriteLine("Matched Sentence Index : {0}", result.MatchingIndex);
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