using Azure.Search.Documents.Indexes.Models;
using Microsoft.CognitiveSearchIntegration.Definitions.Models.CognitiveSearch.Api;
using Microsoft.CognitiveSearchIntegration.Definitions.Models.CognitiveSearch.Api.Indexer;
using System.Threading.Tasks;

namespace Microsoft.CognitiveSearchIntegration.Definitions.APIs.Services
{
    public interface ICognitiveSearchClient
    {
        public Task CreateIndexAsync(SearchIndex index);
        public Task CreateSkillSetAsync(SkillSet skillset);
        public Task CreateIndexerAsync(Indexer indexer);
        public Task CreateDataSourceConnectionAsync(string indexName, string containerName, string connectionString);
    }
}