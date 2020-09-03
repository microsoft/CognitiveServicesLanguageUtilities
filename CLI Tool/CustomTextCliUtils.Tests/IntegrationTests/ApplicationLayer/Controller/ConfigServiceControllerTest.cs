using Microsoft.CustomTextCliUtils.ApplicationLayer.Controllers;
using Microsoft.CustomTextCliUtils.ApplicationLayer.Modeling.Models.Configs;
using Microsoft.CustomTextCliUtils.ApplicationLayer.Services.Logger;
using Microsoft.CustomTextCliUtils.ApplicationLayer.Services.Storage;
using Microsoft.CustomTextCliUtils.Configs.Consts;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Threading.Tasks;
using Xunit;

namespace Microsoft.CustomTextCliUtils.Tests.IntegrationTests.ApplicationLayer.Controller
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
        public async Task AllConfigShowTestAsync()
        {
            // act
            _controller.ShowAllConfigs();

            // assert
            var configsFile = await _storageService.ReadFileAsStringAsync(Constants.ConfigsFileName);
            var configModel = JsonConvert.DeserializeObject<ConfigModel>(configsFile);
            var expectedString = JsonConvert.SerializeObject(configModel, Formatting.Indented);
            Assert.Equal(expectedString, _stringWriter.ToString().Trim());
        }

        [Fact]
        public async Task ParserConfigShowTestAsync()
        {
            // act
            _controller.ShowParserConfigs();

            // assert
            var configsFile = await _storageService.ReadFileAsStringAsync(Constants.ConfigsFileName);
            var configModel = JsonConvert.DeserializeObject<ConfigModel>(configsFile);
            var expectedString = JsonConvert.SerializeObject(configModel.Parser, Formatting.Indented);
            Assert.Equal(expectedString, _stringWriter.ToString().Trim());
        }

        [Fact]
        public async Task MsReadConfigShowTestAsync()
        {
            // act
            _controller.ShowParserMsReadConfigs();

            // assert
            var configsFile = await _storageService.ReadFileAsStringAsync(Constants.ConfigsFileName);
            var configModel = JsonConvert.DeserializeObject<ConfigModel>(configsFile);
            var expectedString = JsonConvert.SerializeObject(configModel.Parser.MsRead, Formatting.Indented);
            Assert.Equal(expectedString, _stringWriter.ToString().Trim());
        }

        [Fact]
        public async Task StorageConfigShowTestAsync()
        {
            // act
            _controller.ShowStorageConfigs();

            // assert
            var configsFile = await _storageService.ReadFileAsStringAsync(Constants.ConfigsFileName);
            var configModel = JsonConvert.DeserializeObject<ConfigModel>(configsFile);
            var expectedString = JsonConvert.SerializeObject(configModel.Storage, Formatting.Indented);
            Assert.Equal(expectedString, _stringWriter.ToString().Trim());
        }

        [Fact]
        public async Task LocalStorageConfigShowTestAsync()
        {
            // act
            _controller.ShowStorageLocalConfigs();

            // assert
            var configsFile = await _storageService.ReadFileAsStringAsync(Constants.ConfigsFileName);
            var configModel = JsonConvert.DeserializeObject<ConfigModel>(configsFile);
            var expectedString = JsonConvert.SerializeObject(configModel.Storage.Local, Formatting.Indented);
            Assert.Equal(expectedString, _stringWriter.ToString().Trim());
        }

        [Fact]
        public async Task BlobStorageConfigShowTestAsync()
        {
            // act
            _controller.ShowStorageBlobConfigs();

            // assert
            var configsFile = await _storageService.ReadFileAsStringAsync(Constants.ConfigsFileName);
            var configModel = JsonConvert.DeserializeObject<ConfigModel>(configsFile);
            var expectedString = JsonConvert.SerializeObject(configModel.Storage.Blob, Formatting.Indented);
            Assert.Equal(expectedString, _stringWriter.ToString().Trim());
        }

        [Fact]
        public async Task PredictionConfigShowTestAsync()
        {
            // act
            _controller.ShowPredictionConfigs();

            // assert
            var configsFile = await _storageService.ReadFileAsStringAsync(Constants.ConfigsFileName);
            var configModel = JsonConvert.DeserializeObject<ConfigModel>(configsFile);
            var expectedString = JsonConvert.SerializeObject(configModel.Prediction, Formatting.Indented);
            Assert.Equal(expectedString, _stringWriter.ToString().Trim());
        }

        [Fact]
        public async Task ChunkerConfigShowTestAsync()
        {
            // act
            _controller.ShowChunkerConfigs();

            // assert
            var configsFile = await _storageService.ReadFileAsStringAsync(Constants.ConfigsFileName);
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
        public async Task MsReadConfigSetTestAsync(string cognitiveServicesKey, string cognitiveServicesEndpoint)
        {
            await _controller.SetMsReadConfigsAsync(cognitiveServicesKey, cognitiveServicesEndpoint);
            await _controller.SetMsReadConfigsAsync(null, null); // Value not affected if user doesn't pass it

            // assert
            var configsFile = await _storageService.ReadFileAsStringAsync(Constants.ConfigsFileName);
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
        public async Task LocalStorageConfigSetTestAsync(string sourceDir, string destinationDir)
        {
            await _controller.SetLocalStorageConfigsAsync(sourceDir, destinationDir);
            await _controller.SetLocalStorageConfigsAsync(null, null); // Value not affected if user doesn't pass it

            // assert
            var configsFile = await _storageService.ReadFileAsStringAsync(Constants.ConfigsFileName);
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
        public async Task BlobStorageConfigSetTestAsync(string connectionString, string sourceContainer, string destinationContainer)
        {
            await _controller.SetBlobStorageConfigsAsync(connectionString, sourceContainer, destinationContainer);
            await _controller.SetBlobStorageConfigsAsync(null, null, null); // Value not affected if user doesn't pass it

            // assert
            var configsFile = await _storageService.ReadFileAsStringAsync(Constants.ConfigsFileName);
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
        public async Task PredictionConfigSetTestAsync(string customTextKey, string customTextEndpoint, string appId, string versionId)
        {
            await _controller.SetPredictionConfigsAsync(customTextKey, customTextEndpoint, appId, versionId);
            await _controller.SetPredictionConfigsAsync(null, null, null, null); // Value not affected if user doesn't pass it

            // assert
            var configsFile = await _storageService.ReadFileAsStringAsync(Constants.ConfigsFileName);
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
        public async Task ChunkerConfigSetTestAsync(int charLimit)
        {
            await _controller.SetChunkerConfigsAsync(charLimit);
            await _controller.SetChunkerConfigsAsync(null); // Value not affected if user doesn't pass it

            // assert
            var configsFile = await _storageService.ReadFileAsStringAsync(Constants.ConfigsFileName);
            var configModel = JsonConvert.DeserializeObject<ConfigModel>(configsFile);
            Assert.Equal(charLimit, configModel.Chunker.CharLimit);
        }
    }
}
