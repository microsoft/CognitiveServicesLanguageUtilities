using FuzzyMatching.Core.Factories;
using FuzzyMatching.Definitions.Models;
using FuzzyMatching.Definitions.Models.Enums;
using FuzzyMatching.Definitions.Services;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace FuzzyMatching.Tests.UnitTests
{
    public class BlobStorageServiceTest
    {
        public static TheoryData TestBlobStorageServiceArrays()
        {
            var array1D = new int[5] { 99, 98, 92, 97, 95 };
          
                var storageOptions = new StorageOptions
            {
                StorageType = StorageType.Blob,
                BaseDirectory = @".",
                ConnectionString = Environment.GetEnvironmentVariable("CONNECTION_STRING_BLOB", EnvironmentVariableTarget.Machine),
                ContainerName = "container"
            };
            var StorageService = StorageFactory.create(storageOptions);


            return new TheoryData<int[], IStorageService, int>
            {
                {
                    array1D,
                    StorageService,
                    array1D.Length
                }
            };
        }

        [Theory]
        [MemberData(nameof(TestBlobStorageServiceArrays))]
        public void TestBlobStorageService(int[] array1D, IStorageService StorageService, int length)
        {
            StorageService.StoreBinaryObjectAsync(array1D, "1D_array", "");
            var LoadedArray = StorageService.LoadBinaryObjectAsync<int[]>("1D_array", "").GetAwaiter().GetResult();
            for (int i = 0; i < length; i++)
            {
                Assert.Equal(array1D[i], LoadedArray[i]);
            }
        }
    }
}
