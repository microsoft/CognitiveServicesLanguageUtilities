using CognitiveSearchIntegration.Runners.Services.Validation.ConfigsValidation;
using CognitiveSearchIntegration.Tests.Configs;
using System;
using System.Threading.Tasks;
using Xunit;

namespace CognitiveSearchIntegration.Tests.Runners.Validations
{
    public class BlobValidationTest
    {
        public static TheoryData BlobValidationServiceAsyncTestData()
        {

            return new TheoryData<string, string, bool>
            {
                {
                    Secrets.Instance.BlobConnectionString,
                    Secrets.Instance.BlobContainerName,
                    true
                },
                {
                    Secrets.Instance.BlobConnectionString,
                    "magedtest2",
                    false
                },
                {
                    "DefaultEndpointsProtocol=https;AccountName=cognitivesearchdatastore;AccountKey=1puRDYQWE+JEPq2D2nr9yFZBJwYqIxTJ4IPr7DOBTuFZ8wL/gqLnCNvl56TEcl+3s9I0x12QUizv7XwRnJ9aRg==;EndpointSuffix=core.windows.net",
                    Secrets.Instance.BlobContainerName,
                    false
                }
            };
        }

        [Theory]
        [MemberData(nameof(BlobValidationServiceAsyncTestData))]
        public async Task BlobValidationServiceAsyncTest(string connectionString, string containerName, bool isValidCredentials)
        {
            var validationService = new BlobStorageValidationService();
            if (isValidCredentials)
            {
                await validationService.ValidateBlobConfigsAsync(connectionString, containerName);
            }
            else
            {
                await Assert.ThrowsAsync<Exception>(() => validationService.ValidateBlobConfigsAsync(connectionString, containerName));
            }
        }
    }
}
