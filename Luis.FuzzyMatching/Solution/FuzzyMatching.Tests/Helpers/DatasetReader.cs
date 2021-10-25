// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using Microsoft.VisualBasic.FileIO;
using System.Collections.Generic;

namespace Microsoft.Azure.CognitiveServices.Language.LUIS.Utilities.FuzzyMatching.Tests.Helpers
{
    internal static class DatasetReader
    {
        internal static List<string> ReadDatasetFromCSV(string filePath, int limit = 30000)
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
