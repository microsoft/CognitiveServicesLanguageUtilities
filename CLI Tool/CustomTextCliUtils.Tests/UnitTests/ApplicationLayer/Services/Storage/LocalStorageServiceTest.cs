using CustomTextCliUtils.ApplicationLayer.Services.Storage;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;
using System.Threading.Tasks;
using Xunit;

namespace CustomTextCliUtils.Tests.UnitTests.ApplicationLayer.Services.Storage
{
    public class LocalStorageServiceTest : IDisposable
    {
        readonly string TestDirectory = "./testDirectory";

        public LocalStorageServiceTest()
        {
            Directory.CreateDirectory(TestDirectory);
        }

        public void Dispose()
        {
            Directory.Delete(TestDirectory, true);
        }

        [Fact]
        public void StoreDataTest()
        {
            string fileName = "storageTest.txt";
            string expected = "StoreDataTest text for testing";
            IStorageService storageService = new LocalStorageService(TestDirectory);
            storageService.StoreData(expected, fileName);
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
            string fileName = "storageTest.txt";
            string expected = "ReadFileAsStringTestAsync text for testing";
            File.WriteAllText(Path.Combine(TestDirectory, fileName), expected);
            IStorageService storageService = new LocalStorageService(TestDirectory);
            string actual = storageService.ReadFileAsString(fileName);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ListFilesTest()
        {
            Directory.CreateDirectory(TestDirectory);
            string[] expectedFiles = new string[] { "file1", "file2", "file3" };
            Array.ForEach(expectedFiles, fileName =>
            {
                File.Create(Path.Combine(TestDirectory, fileName)).Dispose();
            });
            IStorageService storageService = new LocalStorageService(TestDirectory);
            string[] actualFiles = storageService.ListFiles();
            Assert.Equal(expectedFiles, actualFiles);
        }
    }
}
