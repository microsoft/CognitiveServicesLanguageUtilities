using CognitiveSearchIntegration.Runners.Services.Validation.ConfigsValidation;
using CognitiveSearchIntegration.Tests.Configs;
using System;
using System.Threading.Tasks;
using Xunit;

namespace CognitiveSearchIntegration.Tests.Runners.Validations
{
    public class CognitiveSearchValidationTest
    {
        public static TheoryData CognitiveSearchValidationServiceAsyncTestData()
        {
            return new TheoryData<string, string, bool>
            {
                {
                    Secrets.Instance.CognitiveSearchServiceEndpoint,
                    Secrets.Instance.CognitiveSearchServiceKey,
                    true
                },
                {
                    "https://kjbnkhdbfkjhbsdf.search.windows.net",
                    Secrets.Instance.CognitiveSearchServiceKey,
                    false
                },
                {
                    Secrets.Instance.CognitiveSearchServiceEndpoint,
                    "kguasikduhbiouhbasdiuuasdasadsk",
                    false
                }
            };
        }

        [Theory]
        [MemberData(nameof(CognitiveSearchValidationServiceAsyncTestData))]
        public async Task CognitiveSearchValidationServiceAsyncTest(
            string serviceEndpoint,
            string serviceKey,
            bool isValidCredentials)
        {
            var validationService = new CognitiveSearchValidationService();
            if (isValidCredentials)
            {
                await validationService.ValidateCognitiveSearchConfigs(serviceEndpoint, serviceKey);
            }
            else
            {
                await Assert.ThrowsAsync<Exception>(() => validationService.ValidateCognitiveSearchConfigs(serviceEndpoint, serviceKey));
            }
        }
    }
}
