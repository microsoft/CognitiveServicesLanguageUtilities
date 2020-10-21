// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
﻿namespace Microsoft.CogSLanguageUtilities.Definitions.Exceptions.Parser
{
    public class UnsupportedFileTypeException : CliException
    {
        public UnsupportedFileTypeException(string fileName, string fileType, string[] validTypes)
            : base(ConstructMessage(fileName, fileType, validTypes))
        { }

        public static string ConstructMessage(string fileName, string fileType, string[] validTypes)
        {
            return $"Unsupported file type {fileType} for file {fileName} (Supported types are {string.Join(", ", validTypes)})";
        }

    }
}
