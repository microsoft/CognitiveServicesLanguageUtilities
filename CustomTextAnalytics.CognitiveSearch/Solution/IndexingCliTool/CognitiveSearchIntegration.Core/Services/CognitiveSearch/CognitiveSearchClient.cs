using Azure;
using Azure.Search.Documents.Indexes;
using Azure.Search.Documents.Indexes.Models;
using Microsoft.CognitiveSearchIntegration.Core.Helpers;
using Microsoft.CognitiveSearchIntegration.Definitions.APIs.Helpers;
using Microsoft.CognitiveSearchIntegration.Definitions.APIs.Services;
using Microsoft.CognitiveSearchIntegration.Definitions.Consts;
using Microsoft.CognitiveSearchIntegration.Definitions.Models.CognitiveSearch.Api;
using Microsoft.CognitiveSearchIntegration.Definitions.Models.CognitiveSearch.Api.Error;
using Microsoft.CognitiveSearchIntegration.Definitions.Models.CognitiveSearch.Api.Indexer;
using Microsoft.CogSLanguageUtilities.Definitions.Exceptions;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Microsoft.CognitiveSearchIntegration.Core.Services.CognitiveSearch
{
    public class CognitiveSearchClient : ICognitiveSearchClient
    {
        string _endpoint;
        string _key;
        private IHttpHandler _httpHandler;
        private SearchIndexClient _searchIndexClient;
        private SearchIndexerClient _searchIndexerClient;

        public CognitiveSearchClient(IHttpHandler httpHandler, string endpoint, string apiKey)
        {
            _httpHandler = httpHandler;
            _endpoint = endpoint;
            _key = apiKey;
            Uri serviceEndpoint = new Uri(endpoint);
            AzureKeyCredential credential = new AzureKeyCredential(apiKey);
            _searchIndexClient = new SearchIndexClient(serviceEndpoint, credential);
            _searchIndexerClient = new SearchIndexerClient(serviceEndpoint, credential);
        }

        public async Task CreateIndexAsync(SearchIndex index)
        {
            try
            {
                await _searchIndexClient.CreateOrUpdateIndexAsync(index);
            }
            catch (RequestFailedException ex)
            {
                HandleSDKException(ex);
            }
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
            catch (RequestFailedException ex)
            {
                HandleSDKException(ex);
            }
        }

        public async Task CreateIndexerAsync(Indexer indexer)
        {
            var requestUrl = $"{_endpoint}/indexers/{indexer.Name}";
            var headers = new Dictionary<string, string>
            {
                ["api-key"] = _key
            };
            var parameters = new Dictionary<string, string>
            {
                [Constants.CognitiveSearchApiVersionHeader] = Constants.CognitiveSearchApiVersion
            };
            var response = await _httpHandler.SendJsonPutRequestAsync(requestUrl, indexer, headers, parameters);
            await HandleApiResponse(response);
        }

        public async Task CreateSkillSetAsync(SkillSet skillset)
        {
            var requestUrl = $"{_endpoint}/skillsets/{skillset.Name}";
            var headers = new Dictionary<string, string>
            {
                ["api-key"] = _key
            };
            var parameters = new Dictionary<string, string>
            {
                [Constants.CognitiveSearchApiVersionHeader] = Constants.CognitiveSearchApiVersion
            };
            var response = await _httpHandler.SendJsonPutRequestAsync(requestUrl, skillset, headers, parameters);
            await HandleApiResponse(response);
        }

        private async Task HandleApiResponse(HttpResponseMessage response)
        {
            if (response.StatusCode != HttpStatusCode.Created && response.StatusCode != HttpStatusCode.NoContent)
            {
                var errorObject = JsonHandler.DeserializeObject<CognitiveSearchErrorResponse>(await response.Content.ReadAsStringAsync());
                throw new CliException(errorObject.Error.Message);
            }
        }

        private void HandleSDKException(RequestFailedException ex)
        {
            // api morons didn't provide useful message. they just stringify the entire response object
            var msg = ex.Message;
            var newMsg = msg.Substring(0, msg.IndexOf("\r\n"));
            throw new CliException(newMsg);
        }
    }
}
