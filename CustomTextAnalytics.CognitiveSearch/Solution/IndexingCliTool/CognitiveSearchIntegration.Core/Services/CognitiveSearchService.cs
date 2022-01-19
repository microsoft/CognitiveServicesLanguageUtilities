using Azure.Search.Documents.Indexes.Models;
using Microsoft.CognitiveSearchIntegration.Utilities.Nugets.CognitiveSearchClient;
using Microsoft.CognitiveSearchIntegration.Utilities.Nugets.CognitiveSearchClient.RestClient.ApiModels;
using Microsoft.CognitiveSearchIntegration.Utilities.Nugets.CognitiveSearchClient.RestClient.ApiModels.Indexer;
using System.Threading.Tasks;

namespace Microsoft.CognitiveSearchIntegration.Core.Services
{
    public class CognitiveSearchService
    {
        private CognitiveSearchClient _client;

        public CognitiveSearchService(string endpoint, string apiKey)
        {
            _client = new CognitiveSearchClient(endpoint, apiKey);
        }

        public async Task CreateIndexAsync(SearchIndex index)
        {
            await _client.CreateIndexAsync(index);
        }

        public async Task CreateDataSourceConnectionAsync(string dataSourceName, string containerName, string connectionString)
        {
            await _client.CreateDataSourceConnectionAsync(dataSourceName, containerName, connectionString);
        }

        public async Task CreateIndexerAsync(Indexer indexer)
        {
            await _client.CreateIndexerAsync(indexer);
        }

        public async Task CreateSkillSetAsync(SkillSet skillset)
        {
            await _client.CreateSkillSetAsync(skillset);
        }
    }
}
