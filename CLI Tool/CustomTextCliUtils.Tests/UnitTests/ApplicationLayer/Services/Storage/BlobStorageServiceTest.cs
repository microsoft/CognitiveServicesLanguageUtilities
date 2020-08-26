﻿using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using CustomTextCliUtils.ApplicationLayer.Exceptions.Storage;
using CustomTextCliUtils.ApplicationLayer.Services.Storage;
using System;
using System.IO;
using System.Threading.Tasks;
using Xunit;

namespace CustomTextCliUtils.Tests.UnitTests.ApplicationLayer.Services.Storage
{
    public class BlobStorageServiceTest : IDisposable
    {
        private const string _connectionString = "DefaultEndpointsProtocol=https;AccountName=nourdocuments;AccountKey=5UvtQ8CiXwDXg63QyEgtReW3E31KTXMvT5UfjnX1XgAW1DU390nKAlkCeBn7DUyDgaaQdm5TZt3iB7DfdUlD5A==;EndpointSuffix=core.windows.net";// Environment.GetEnvironmentVariable("TestBlobStorageConnectionString");
        private const string _testContainer = "containertest";
        private readonly BlobContainerClient _blobContainerClient;

        public BlobStorageServiceTest()
        {
            BlobServiceClient blobServiceClient = new BlobServiceClient(_connectionString);
            _blobContainerClient = blobServiceClient.GetBlobContainerClient(_testContainer);
            if (!_blobContainerClient.Exists())
            {
                _blobContainerClient = blobServiceClient.CreateBlobContainer(_testContainer);
            }
            else
            {
                Parallel.ForEach(_blobContainerClient.GetBlobs(), x => _blobContainerClient.DeleteBlob(x.Name));
            }
        }

        public void Dispose()
        {
            DeleteAllBlobs();
        }

        public static TheoryData<string, string, Type> BlobStorageConnectionTestData()
        {
            return new TheoryData<string, string, Type>
            {
                {
                    _connectionString,
                    _testContainer,
                    null
                },
                {
                    "DefaultEndpointsProtocol=https;AccountName=nourdocuments;AccountKey=5UvtQ8CiXwDXg63QyEgtReW3s31KTXMvT5UfjnX1XgAW1DU390nKAlkCeBn7DUyDgaaQdm5TZt3iB7DfdUlD5A==;EndpointSuffix=core.windows.net",
                    _testContainer,
                    typeof(InvalidBlobStorageConnectionStringException)
                },
                {
                    _connectionString,
                    "nonexistentcontainer",
                    typeof(BlobContainerNotFoundException)
                }
            };
        }

        [Theory]
        [MemberData(nameof(BlobStorageConnectionTestData))]
        public void BlobStorageConnectionTest(string connectionString, string containerName, Type exceptionType)
        {
            if (exceptionType == null)
            {
                new BlobStorageService(connectionString, containerName);
            }
            else if (exceptionType.Equals(typeof(InvalidBlobStorageConnectionStringException)))
            {
                Assert.Throws<InvalidBlobStorageConnectionStringException>(() => { new BlobStorageService(connectionString, containerName); });
            }
            else if (exceptionType.Equals(typeof(BlobContainerNotFoundException)))
            {
                Assert.Throws<BlobContainerNotFoundException>(() => { new BlobStorageService(connectionString, containerName); });
            }
        }

        [Fact]
        public void StoreDataTest()
        {
            string fileName = "storageTest.txt";
            string expected = "StoreDataTest text for testing";
            string actual = "";
            IStorageService storageService = new BlobStorageService(_connectionString, _testContainer);
            storageService.StoreData(expected, fileName);
            BlobClient blobClient = _blobContainerClient.GetBlobClient(fileName);
            BlobDownloadInfo download = blobClient.Download();
            using (StreamReader sr = new StreamReader(download.Content))
            {
                actual = sr.ReadToEnd();
            }
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task ReadFileTest()
        {
            // Write file to blob store
            string fileName = "storageTest.txt";
            string expected = "ReadFileTest text for testing";
            string actual = "";
            UploadFileHelper(fileName, expected);
            IStorageService storageService = new BlobStorageService(_connectionString, _testContainer);
            Stream file = await storageService.ReadFile(fileName);
            using (StreamReader sr = new StreamReader(file))
            {
                actual = sr.ReadToEnd();
            }
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ReadFileAsStringTest()
        {
            // Write file to blob store
            string fileName = "storageTest.txt";
            string expected = "ReadFileAsStringTest text for testing";
            UploadFileHelper(fileName, expected);
            IStorageService storageService = new BlobStorageService(_connectionString, _testContainer);
            string actual = storageService.ReadFileAsString(fileName);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ListFilesTest()
        {
            string[] expectedFiles = new string[] { "file1", "file2", "file3" };
            Array.ForEach(expectedFiles, fileName =>
            {
                UploadFileHelper(fileName, "text");
            });
            IStorageService storageService = new BlobStorageService(_connectionString, _testContainer);
            string[] actualFiles = storageService.ListFiles();
            Assert.Equal(expectedFiles, actualFiles);
        }

        private void UploadFileHelper(string fileName, string expected)
        {
            BlobClient blobClient = _blobContainerClient.GetBlobClient(fileName);
            using (MemoryStream stream = new MemoryStream())
            {
                using (StreamWriter sw = new StreamWriter(stream))
                {
                    sw.Write(expected);
                    sw.Flush();
                    stream.Position = 0;
                    blobClient.Upload(stream, overwrite: true);
                }
            }
        }

        private void DeleteAllBlobs()
        {
            Parallel.ForEach(_blobContainerClient.GetBlobs(), x => _blobContainerClient.DeleteBlob(x.Name));
        }
    }
}