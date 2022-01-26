
using CustomSkillsetFunction.ViewModels.Request;
using CustomSkillsetFunction.ViewModels.Response;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

namespace Microsoft.CognitiveSearch.CustomSkillset.CustomText.Helpers
{
    public static class ViewModelMapper
    {
        public static IEnumerable<RequestRecord> GetRequestRecords(HttpRequest httpRequest)
        {
            var requestBodyJson = new StreamReader(httpRequest.Body).ReadToEnd();
            var docs = JsonConvert.DeserializeObject<CustomSkillsetRequest>(requestBodyJson);
            var result = docs.Values;
            if (result == null || result.Count == 0)
            {
                return null;
            }
            return result;
        }

        public static CustomSkillsetResponse CreateResponseData(List<ResponseRecord> responseRecords)
        {
            return new CustomSkillsetResponse() { Values = responseRecords };
        }
    }
}