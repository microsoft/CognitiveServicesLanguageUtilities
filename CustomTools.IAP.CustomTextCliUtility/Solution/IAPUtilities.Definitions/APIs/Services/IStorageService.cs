// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
using System.IO;
using System.Threading.Tasks;

namespace Microsoft.IAPUtilities.Definitions.APIs.Services
{
    public interface IStorageService
    {
        /// <summary>
        /// Lists file names in the default source path
        /// </summary>
        /// <returns></returns>
        public Task<string[]> ListFilesAsync();
        /// <summary>
        /// Reads file from default source path
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public Task<Stream> ReadFileAsync(string fileName);
        /// <summary>
        /// Writes file to default destination path
        /// </summary>
        /// <param name="data"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public Task StoreDataAsync(string data, string fileName);
        /// <summary>
        /// Checks if file exists in default source path
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public Task<bool> FileExists(string fileName);
    }
}
