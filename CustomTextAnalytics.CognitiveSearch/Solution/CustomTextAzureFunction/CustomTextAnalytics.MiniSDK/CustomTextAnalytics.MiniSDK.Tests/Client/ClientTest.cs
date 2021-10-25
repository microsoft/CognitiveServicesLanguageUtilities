using CustomTextAnalytics.MiniSDK.Client;
using System;
using System.Threading.Tasks;
using Xunit;

namespace CustomTextAnalytics.MiniSDK.Tests.Client
{
    public class ClientTest
    {
        private static CustomTextAnalyticsClient _customTextClient;
        private static readonly string _customTextResourceEndpointUrl = Environment.GetEnvironmentVariable("CustomTextResourceEndpointUrl");
        private static readonly string _customTextResourceKey = Environment.GetEnvironmentVariable("CustomTextResourceKey");
        private static readonly string _customTextProjectName = Environment.GetEnvironmentVariable("CustomTextProjectName");
        private static readonly string _customTextDeploymentName = Environment.GetEnvironmentVariable("CustomTextDeploymentName");

        public ClientTest()
        {
            _customTextClient = new CustomTextAnalyticsClient(_customTextResourceEndpointUrl, _customTextResourceKey);
        }
        public static TheoryData AnalyzeCustomEntitiesAsyncTestData()
        {
            var customTextProjectName = _customTextProjectName;
            var customTextDeploymentName = _customTextDeploymentName;
            var testText = "A recent report by the Government Accountability Office (GAO) found that the dramatic increase in oil and natural gas development on federal lands over the past six years has stretched the staff of the BLM to a point that it has been unable to meet its environmental protection responsibilities.";

            return new TheoryData<string, string, string>
            {
                {
                    customTextProjectName,
                    customTextDeploymentName,
                    testText
                }
            };
        }

        [Theory]
        [MemberData(nameof(AnalyzeCustomEntitiesAsyncTestData))]
        public async Task AnalyzeCustomEntitiesAsyncTestAsync(string projectName, string deploymentName, string testText)
        {
            var result = await _customTextClient.AnalyzeCustomEntitiesAsync(testText, projectName, deploymentName);
            Assert.NotNull(result);
        }

        public static TheoryData StartAnalyzeCustomEntitiesAsyncTestData()
        {
            var customTextProjectName = _customTextProjectName;
            var customTextDeploymentName = _customTextDeploymentName;
            var testText = "A recent report by the Government Accountability Office (GAO) found that the dramatic increase in oil and natural gas development on federal lands over the past six years has stretched the staff of the BLM to a point that it has been unable to meet its environmental protection responsibilities.";

            return new TheoryData<string, string, string>
            {
                {
                    customTextProjectName,
                    customTextDeploymentName,
                    testText
                }
            };
        }

        [Theory]
        [MemberData(nameof(StartAnalyzeCustomEntitiesAsyncTestData))]
        public async Task StartAnalyzeCustomEntitiesAsyncTest(string projectName, string deploymentName, string testText)
        {
            var jobId = await _customTextClient.StartAnalyzeCustomEntitiesAsync(testText, projectName, deploymentName);
            Assert.NotNull(jobId);

            var result = await _customTextClient.GetAnalyzeJobInfo(jobId);
            Assert.NotNull(result.Status);
        }
    }


}
