// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
namespace Microsoft.IAPUtilities.Definitions.Exceptions.Storage
{
    public class FolderNotFoundException : CliException
    {
        public FolderNotFoundException(string folderPath)
            : base(ConstructMessage(folderPath))
        { }

        public static string ConstructMessage(string folderPath)
        {
            return $"Directory Not Found: {folderPath}";
        }
    }
}
