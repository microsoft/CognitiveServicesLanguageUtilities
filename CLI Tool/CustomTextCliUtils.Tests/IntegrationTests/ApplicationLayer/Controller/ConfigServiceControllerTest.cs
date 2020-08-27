using CustomTextCliUtils.ApplicationLayer.Controllers;
using CustomTextCliUtils.ApplicationLayer.Modeling.Models.Configs;
using CustomTextCliUtils.ApplicationLayer.Services.Logger;
using CustomTextCliUtils.ApplicationLayer.Services.Storage;
using CustomTextCliUtils.Configs.Consts;
using Newtonsoft.Json;
using System;
using System.IO;
using Xunit;

namespace CustomTextCliUtils.Tests.IntegrationTests.ApplicationLayer.Controller
{
    public class ConfigServiceControllerTest
    {
        StringWriter _stringWriter;
        ConfigServiceController _controller;
        IStorageService _storageService;

        public ConfigServiceControllerTest()
        {
            // arrange
            _stringWriter = new StringWriter();
            Console.SetOut(_stringWriter);
            var loggerService = new ConsoleLoggerService();
            _storageService = new LocalStorageService(Constants.ConfigsFileLocalDirectory);

            // act
            _controller = new ConfigServiceController(loggerService, _storageService);
        }

        [Fact]
        public void AllConfigShowTest()
        {
            // act
            _controller.ShowAllConfigs();

            // assert
            var configsFile = _storageService.ReadFileAsString(Constants.ConfigsFileName);
            var configModel = JsonConvert.DeserializeObject<ConfigModel>(configsFile);
            var expectedString = JsonConvert.SerializeObject(configModel, Formatting.Indented);
            Assert.Equal(expectedString, _stringWriter.ToString().Trim());
        }

        [Fact]
        public void ParserConfigShowTest()
        {
            // act
            _controller.ShowParserConfigs();

            // assert
            var configsFile = _storageService.ReadFileAsString(Constants.ConfigsFileName);
            var configModel = JsonConvert.DeserializeObject<ConfigModel>(configsFile);
            var expectedString = JsonConvert.SerializeObject(configModel.Parser, Formatting.Indented);
            Assert.Equal(expectedString, _stringWriter.ToString().Trim());
        }

        [Fact]
        public void MsReadConfigShowTest()
        {
            // act
            _controller.ShowParserMsReadConfigs();

            // assert
            var configsFile = _storageService.ReadFileAsString(Constants.ConfigsFileName);
            var configModel = JsonConvert.DeserializeObject<ConfigModel>(configsFile);
            var expectedString = JsonConvert.SerializeObject(configModel.Parser.MsRead, Formatting.Indented);
            Assert.Equal(expectedString, _stringWriter.ToString().Trim());
        }

        [Fact]
        public void StorageConfigShowTest()
        {
            // act
            _controller.ShowStorageConfigs();

            // assert
            var configsFile = _storageService.ReadFileAsString(Constants.ConfigsFileName);
            var configModel = JsonConvert.DeserializeObject<ConfigModel>(configsFile);
            var expectedString = JsonConvert.SerializeObject(configModel.Storage, Formatting.Indented);
            Assert.Equal(expectedString, _stringWriter.ToString().Trim());
        }

        [Fact]
        public void LocalStorageConfigShowTest()
        {
            // act
            _controller.ShowStorageLocalConfigs();

            // assert
            var configsFile = _storageService.ReadFileAsString(Constants.ConfigsFileName);
            var configModel = JsonConvert.DeserializeObject<ConfigModel>(configsFile);
            var expectedString = JsonConvert.SerializeObject(configModel.Storage.Local, Formatting.Indented);
            Assert.Equal(expectedString, _stringWriter.ToString().Trim());
        }

        [Fact]
        public void BlobStorageConfigShowTest()
        {
            // act
            _controller.ShowStorageBlobConfigs();

            // assert
            var configsFile = _storageService.ReadFileAsString(Constants.ConfigsFileName);
            var configModel = JsonConvert.DeserializeObject<ConfigModel>(configsFile);
            var expectedString = JsonConvert.SerializeObject(configModel.Storage.Blob, Formatting.Indented);
            Assert.Equal(expectedString, _stringWriter.ToString().Trim());
        }

        [Fact]
        public void PredictionConfigShowTest()
        {
            // act
            _controller.ShowPredictionConfigs();

            // assert
            var configsFile = _storageService.ReadFileAsString(Constants.ConfigsFileName);
            var configModel = JsonConvert.DeserializeObject<ConfigModel>(configsFile);
            var expectedString = JsonConvert.SerializeObject(configModel.Prediction, Formatting.Indented);
            Assert.Equal(expectedString, _stringWriter.ToString().Trim());
        }

        [Fact]
        public void ChunkerConfigShowTest()
        {
            // act
            _controller.ShowChunkerConfigs();

            // assert
            var configsFile = _storageService.ReadFileAsString(Constants.ConfigsFileName);
            var configModel = JsonConvert.DeserializeObject<ConfigModel>(configsFile);
            var expectedString = JsonConvert.SerializeObject(configModel.Chunker, Formatting.Indented);
            Assert.Equal(expectedString, _stringWriter.ToString().Trim());
        }

        public static TheoryData MsReadConfigSetTestData()
        {
            return new TheoryData<string, string>
            {
                {
                "testKey",
                "testEndpoint"
                }
            };
        }

        [Theory]
        [MemberData(nameof(MsReadConfigSetTestData))]
        public void MsReadConfigSetTest(string cognitiveServicesKey, string cognitiveServicesEndpoint)
        {
            _controller.SetMsReadConfigs(cognitiveServicesKey, cognitiveServicesEndpoint);
            _controller.SetMsReadConfigs(null, null); // Value not affected if user doesn't pass it

            // assert
            var configsFile = _storageService.ReadFileAsString(Constants.ConfigsFileName);
            var configModel = JsonConvert.DeserializeObject<ConfigModel>(configsFile);
            Assert.Equal(cognitiveServicesKey, configModel.Parser.MsRead.CongnitiveServiceKey);
            Assert.Equal(cognitiveServicesEndpoint, configModel.Parser.MsRead.CognitiveServiceEndPoint);
        }

        public static TheoryData LocalStorageConfigSetTestData()
        {
            return new TheoryData<string, string>
            {
                {
                "testSource",
                "testDestination"
                }
            };
        }

        [Theory]
        [MemberData(nameof(LocalStorageConfigSetTestData))]
        public void LocalStorageConfigSetTest(string sourceDir, string destinationDir)
        {
            _controller.SetLocalStorageConfigs(sourceDir, destinationDir);
            _controller.SetLocalStorageConfigs(null, null); // Value not affected if user doesn't pass it

            // assert
            var configsFile = _storageService.ReadFileAsString(Constants.ConfigsFileName);
            var configModel = JsonConvert.DeserializeObject<ConfigModel>(configsFile);
            Assert.Equal(sourceDir, configModel.Storage.Local.SourceDirectory);
            Assert.Equal(destinationDir, configModel.Storage.Local.DestinationDirectory);
        }

        public static TheoryData BlobStorageConfigSetTestData()
        {
            return new TheoryData<string, string, string>
            {
                {
                    "testConnectionString",
                    "testSource",
                    "testDestination"
                }
            };
        }

        [Theory]
        [MemberData(nameof(BlobStorageConfigSetTestData))]
        public void BlobStorageConfigSetTest(string connectionString, string sourceContainer, string destinationContainer)
        {
            _controller.SetBlobStorageConfigs(connectionString, sourceContainer, destinationContainer);
            _controller.SetBlobStorageConfigs(null, null, null); // Value not affected if user doesn't pass it

            // assert
            var configsFile = _storageService.ReadFileAsString(Constants.ConfigsFileName);
            var configModel = JsonConvert.DeserializeObject<ConfigModel>(configsFile);
            Assert.Equal(sourceContainer, configModel.Storage.Blob.SourceContainer);
            Assert.Equal(destinationContainer, configModel.Storage.Blob.DestinationContainer);
            Assert.Equal(connectionString, configModel.Storage.Blob.ConnectionString);
        }

        public static TheoryData PredictionConfigSetTestData()
        {
            return new TheoryData<string, string, string, string>
            {
                {
                    "testKey",
                    "testEndpoint",
                    "testAppId",
                    "testVersionId"
                }
            };
        }

        [Theory]
        [MemberData(nameof(PredictionConfigSetTestData))]
        public void PredictionConfigSetTest(string customTextKey, string customTextEndpoint, string appId, string versionId)
        {
            _controller.SetPredictionConfigs(customTextKey, customTextEndpoint, appId, versionId);
            _controller.SetPredictionConfigs(null, null, null, null); // Value not affected if user doesn't pass it

            // assert
            var configsFile = _storageService.ReadFileAsString(Constants.ConfigsFileName);
            var configModel = JsonConvert.DeserializeObject<ConfigModel>(configsFile);
            Assert.Equal(customTextKey, configModel.Prediction.CustomTextKey);
            Assert.Equal(customTextEndpoint, configModel.Prediction.EndpointUrl);
            Assert.Equal(appId, configModel.Prediction.AppId);
            Assert.Equal(versionId, configModel.Prediction.VersionId);
        }

        public static TheoryData ChunkerConfigSetTestData()
        {
            return new TheoryData<int>
            {
                {
                    200
                }
            };
        }

        [Theory]
        [MemberData(nameof(ChunkerConfigSetTestData))]
        public void ChunkerConfigSetTest(int charLimit)
        {
            _controller.SetChunkerConfigs(charLimit);
            _controller.SetChunkerConfigs(null); // Value not affected if user doesn't pass it

            // assert
            var configsFile = _storageService.ReadFileAsString(Constants.ConfigsFileName);
            var configModel = JsonConvert.DeserializeObject<ConfigModel>(configsFile);
            Assert.Equal(charLimit, configModel.Chunker.CharLimit);
        }
    }
}
