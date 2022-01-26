using Azure;
using Azure.Search.Documents.Indexes;
using Azure.Search.Documents.Indexes.Models;
using Microsoft.CognitiveSearchIntegration.Utilities.Nugets.CognitiveSearchClient.RestClient.ApiModels;
using Microsoft.CognitiveSearchIntegration.Utilities.Nugets.CognitiveSearchClient.RestClient.ApiModels.Indexer;
using System;
using System.Threading.Tasks;

namespace Microsoft.CognitiveSearchIntegration.Utilities.Nugets.CognitiveSearchClient
{
    public class CognitiveSearchClient
    {
        private SearchIndexClient _searchIndexClient;
        private SearchIndexerClient _searchIndexerClient;
        private CognitiveSearchRestClient _restClient;

        public CognitiveSearchClient(string endpoint, string apiKey)
        {
            _restClient = new CognitiveSearchRestClient(endpoint, apiKey);
            Uri serviceEndpoint = new Uri(endpoint);
            AzureKeyCredential credential = new AzureKeyCredential(apiKey);
            _searchIndexClient = new SearchIndexClient(serviceEndpoint, credential);
            _searchIndexerClient = new SearchIndexerClient(serviceEndpoint, credential);
        }

        public async Task CreateDataSourceConnectionAsync(string dataSourceName, string containerName, string connectionString)
        {
            try
            {
                SearchIndexerDataContainer searchIndexerDataContainer = new SearchIndexerDataContainer(containerName);
                SearchIndexerDataSourceConnection searchIndexerDataSourceConnection = new SearchIndexerDataSourceConnection(
                    dataSourceName,
                    SearchIndexerDataSourceType.AzureBlob,
                    connectionString,
                    searchIndexerDataContainer);
                await _searchIndexerClient.CreateOrUpdateDataSourceConnectionAsync(searchIndexerDataSourceConnection);
            }
            catch (Exception)
            {
                throw new Exception("Cognitive Search APIs -> Failed To Create Data Source Connection!");
            }
        }

        public async Task CreateIndexAsync(SearchIndex index)
        {
            try
            {
                await _searchIndexClient.CreateOrUpdateIndexAsync(index);
            }
            catch (Exception)
            {
                throw new Exception("Cognitive Search APIs -> Failed To Create Index!");
            }
        }

        public async Task CreateSkillSetAsync(SkillSet skillset)
        {
            try
            {
                await _restClient.CreateSkillSetAsync(skillset);
            }
            catch (Exception)
            {
                throw new Exception("Cognitive Search APIs -> Failed To Create Skillset!");
            }
        }

        public async Task CreateIndexerAsync(Indexer indexer)
        {
            try
            {
                await _restClient.CreateIndexerAsync(indexer);
            }
            catch (Exception)
            {
                throw new Exception("Cognitive Search APIs -> Failed To Create Indexer!");
            }
        }
    }
}
