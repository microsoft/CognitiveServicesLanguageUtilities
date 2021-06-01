using FuzzyMatching.Definitions.Models;
using FuzzyMatching.Definitions.Models.Enums;
using FuzzyMatching.Definitions.Services;
using Microsoft.CogSLanguageUtilities.Core.Services.Storage;
using System;

namespace FuzzyMatching.Core.Factories
{
    public static class StorageFactory
    {
        public static IStorageService create(StorageOptions storageOptions)
        {
            switch (storageOptions.StorageType)
            {
                case StorageType.Local:
                    return new LocalStorageService(storageOptions.BaseDirectory);
                case StorageType.Blob:
                    return new BlobStorageService(storageOptions.ConnectionString, storageOptions.ContainerName);
                default:
                    throw new Exception("storage option not supported!");
            }
        }
    }
}
