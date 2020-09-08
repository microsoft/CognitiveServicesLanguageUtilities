using Microsoft.CustomTextCliUtils.ApplicationLayer.Exceptions;
using Microsoft.CustomTextCliUtils.ApplicationLayer.Exceptions.Prediction;
using Microsoft.CustomTextCliUtils.ApplicationLayer.Services.Prediction;
using Microsoft.CustomTextCliUtils.ApplicationLayer.Helpers.HttpHandler;
using Xunit;
using Microsoft.CustomTextCliUtils.Tests.Configs;
using System.Threading.Tasks;

namespace CustomTextCliUtils.Tests.IntegrationTests.ApplicationLayer.Services.Prediction
{
    public class CustomTextPredictionServiceTest
    {
        // Test Prediction Mapping
        // ######################################################################
        public static TheoryData TestParsingData()
        {
            var httpHandler = new HttpHandler();
            var customTextKey = Secrets.CustomTextKey;
            var invalidCustomTextKey = "cc0c5afc3ddc123e96cbdcdd4fceef40";
            var customTextEndpoint = Secrets.CustomTextEndpoint;
            var appId = Secrets.CustomTextAppId;

            return new TheoryData<string, string, string, IHttpHandler, string, CliException>
            {
                {
                    customTextKey,
                    customTextEndpoint,
                    appId,
                    httpHandler,
                    "asdasdasd",
                    null
                },
                {
                    invalidCustomTextKey,
                    customTextEndpoint,
                    appId,
                    httpHandler,
                    "asdasdasd",
                    new UnauthorizedRequestException("asd", "asd")
                }
            };
        }

        [Theory]
        [MemberData(nameof(TestParsingData))]
        public async Task TestPredictionAsync(string customTextKey, string endpointUrl, string appId, IHttpHandler httpHandler, string inputText, CliException expectedException)
        {
            /* TEST NOTES
             * *************
             * we only care about prediction result
             * i.e. prediction results maps to our object correctly
             * 
             * we don't care about the actual values in the object
             * because the service provider (in this case CustomText team)
             * may optimize their engine
             * rendering the values in our "ExpectedResult" object in correct
             * */
            // act
            if (expectedException == null)
            {
                var predictionService = new CustomTextPredictionService(httpHandler, customTextKey, endpointUrl, appId);
                var actualResult = await predictionService.GetPredictionAsync(inputText);
                // validate object values aren't null
                Assert.NotNull(actualResult.Prediction.PositiveClassifiers);
                Assert.NotNull(actualResult.Prediction.Classifiers);
                Assert.NotNull(actualResult.Prediction.Extractors);
            }
            else
            {
                await Assert.ThrowsAsync(expectedException.GetType(), async () => {
                    var predictionService = new CustomTextPredictionService(httpHandler, customTextKey, endpointUrl, appId);
                    await predictionService.GetPredictionAsync(inputText);
                });
            }
        }
    }
}
