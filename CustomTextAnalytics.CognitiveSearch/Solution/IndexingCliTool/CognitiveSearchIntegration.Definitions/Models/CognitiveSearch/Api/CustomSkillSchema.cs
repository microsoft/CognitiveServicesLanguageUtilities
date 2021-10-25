using Newtonsoft.Json;
using System.Collections.Generic;

namespace Microsoft.CognitiveSearchIntegration.Definitions.Models.CognitiveSearch.Api
{

    public class CustomSkillSchema
    {
        [JsonProperty("@odata.type")]
        public string odatatype { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public string context { get; set; }
        public string uri { get; set; }
        public string httpMethod { get; set; }
        public string timeout { get; set; }
        public int batchSize { get; set; }
        public object degreeOfParallelism { get; set; }
        public List<Input> inputs { get; set; }
        public List<Output> outputs { get; set; }
        [JsonProperty("httpHeaders")]
        public HttpHeaders HttpHeaders { get; set; }

        public CustomSkillSchema()
        {
            odatatype = "#Microsoft.Skills.Custom.WebApiSkill";
            name = "Extract from LUIS-D";
            description = "Calls an Azure function, which in turn calls LUIS-D prediction endpoint";
            context = "/document/content";
            uri = "[URL to the function hosting the LUISExtractor solution]";
            httpMethod = "POST";
            timeout = "PT30S";
            batchSize = 1000;
            inputs = new List<Input>() { new Input() { name = "text", source = "/document/content" } };
            outputs = new List<Output>();
        }
    }

    public class HttpHeaders
    {
        [JsonProperty("endpointUrl")]
        public string EndpointUrl { get; set; }

        [JsonProperty("apiKey")]
        public string ApiKey { get; set; }

        [JsonProperty("projectName")]
        public string ProjectName { get; set; }

        [JsonProperty("deploymentName")]
        public string DeploymentName { get; set; }
    }

    public class Input
    {
        public string name { get; set; }
        public string source { get; set; }
    }

    public class Output
    {
        public string name { get; set; }
        public string targetName { get; set; }
    }

}
