// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
namespace Microsoft.IAPUtilities.Definitions.Exceptions.Storage
{
    public class FileNotFoundException : CliException
    {
        public FileNotFoundException(string filePath)
            : base(ConstructMessage(filePath))
        { }

        public static string ConstructMessage(string filePath)
        {
            return $"File Not Found: {filePath}";
        }
    }
}
