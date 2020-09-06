using Microsoft.CustomTextCliUtils.ApplicationLayer.Exceptions.Storage;
using Microsoft.CustomTextCliUtils.ApplicationLayer.Services.Storage;
using System;
using System.IO;
using System.Threading.Tasks;
using Xunit;

namespace Microsoft.CustomTextCliUtils.Tests.IntegrationTests.ApplicationLayer.Services.Storage
{
    public class LocalStorageServiceTest : IDisposable
    {
        const string TestDirectory = "./testDirectory";

        public LocalStorageServiceTest()
        {
            Directory.CreateDirectory(TestDirectory);
        }

        public void Dispose()
        {
            Directory.Delete(TestDirectory, true);
        }

        public static TheoryData LocalStorageConnectionTestData()
        {
            string invalidDirectory = "folderDoesNotExist";
            return new TheoryData<string, Exception>
            {
                {
                    invalidDirectory,
                    new FolderNotFoundException(invalidDirectory)
                },
                {
                    TestDirectory,
                    null
                }
            };
        }

        [Theory]
        [MemberData(nameof(LocalStorageConnectionTestData))]
        public void LocalStorageConnectionTest(string directory, Exception excpectedException)
        {
            if (excpectedException == null)
            {
                new LocalStorageService(directory);
            }
            else
            {
                Assert.Throws(excpectedException.GetType(), () => new LocalStorageService(directory));
            }
        }

        [Fact]
        public async Task StoreDataTestAsync()
        {
            string fileName = "storageTest.txt";
            string expected = "StoreDataTest text for testing";
            IStorageService storageService = new LocalStorageService(TestDirectory);
            await storageService.StoreDataAsync(expected, fileName);
            string actual = File.ReadAllText(Path.Combine(TestDirectory, fileName));
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task ReadFileTest()
        {
            string fileName = "storageTest.txt";
            string expected = "ReadFileTest text for testing";
            File.WriteAllText(Path.Combine(TestDirectory, fileName), expected);
            string actual = "";
            IStorageService storageService = new LocalStorageService(TestDirectory);
            Stream file = await storageService.ReadFileAsync(fileName);
            using (StreamReader sr = new StreamReader(file))
            {
                actual = sr.ReadToEnd();
            }
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task ReadFileAsStringTestAsync()
        {
            string fileName = "storageTest.txt";
            string expected = "ReadFileAsStringTestAsync text for testing";
            File.WriteAllText(Path.Combine(TestDirectory, fileName), expected);
            IStorageService storageService = new LocalStorageService(TestDirectory);
            string actual = await storageService.ReadFileAsStringAsync(fileName);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task ListFilesTestAsync()
        {
            Directory.CreateDirectory(TestDirectory);
            string[] expectedFiles = new string[] { "file1", "file2", "file3" };
            Array.ForEach(expectedFiles, fileName =>
            {
                File.Create(Path.Combine(TestDirectory, fileName)).Dispose();
            });
            IStorageService storageService = new LocalStorageService(TestDirectory);
            string[] actualFiles = await storageService.ListFilesAsync();
            Assert.Equal(expectedFiles, actualFiles);
        }
    }
}
