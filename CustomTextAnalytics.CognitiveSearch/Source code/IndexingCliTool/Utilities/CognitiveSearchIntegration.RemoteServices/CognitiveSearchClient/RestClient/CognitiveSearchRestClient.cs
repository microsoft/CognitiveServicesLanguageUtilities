using Microsoft.CognitiveSearchIntegration.Utilities.Nugets.CognitiveSearchClient.RestClient.ApiModels;
using Microsoft.CognitiveSearchIntegration.Utilities.Nugets.CognitiveSearchClient.RestClient.ApiModels.Indexer;
using Microsoft.CognitiveSearchIntegration.Utilities.Nugets.Helpers;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace Microsoft.CognitiveSearchIntegration.Utilities.Nugets.CognitiveSearchClient
{
    public class CognitiveSearchRestClient
    {
        private string _endpoint;
        private string _key;
        private HttpHandler _httpHandler = new HttpHandler();

        public CognitiveSearchRestClient(string endpoint, string apiKey)
        {
            _endpoint = endpoint;
            _key = apiKey;
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
                ["api-version"] = "2020-06-30"
            };
            var response = await _httpHandler.SendJsonPutRequestAsync(requestUrl, indexer, headers, parameters);

            if (response.StatusCode != HttpStatusCode.Created && response.StatusCode != HttpStatusCode.NoContent)
            {
                throw new Exception();
            }
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
                ["api-version"] = "2020-06-30"
            };
            var response = await _httpHandler.SendJsonPutRequestAsync(requestUrl, skillset, headers, parameters);

            if (response.StatusCode != HttpStatusCode.Created && response.StatusCode != HttpStatusCode.NoContent)
            {
                throw new Exception();
            }
        }
    }
}
