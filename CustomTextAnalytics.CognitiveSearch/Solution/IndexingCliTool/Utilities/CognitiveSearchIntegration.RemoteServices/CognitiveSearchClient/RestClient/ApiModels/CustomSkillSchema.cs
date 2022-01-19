using Newtonsoft.Json;
using System.Collections.Generic;

namespace Microsoft.CognitiveSearchIntegration.Utilities.Nugets.CognitiveSearchClient.RestClient.ApiModels
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
        // resource credentials
        [JsonProperty("resourceEndpointUrl")]
        public string CustomTextResourceEndpointHeader { get; set; }
        [JsonProperty("resourceKey")]
        public string CustomTextResourceKeyHeader { get; set; }

        // entity recognition project
        [JsonProperty("entityRecognitionProjectName", NullValueHandling = NullValueHandling.Ignore)]
        public string EntityRecognitionProjectNameHeader { get; set; }
        [JsonProperty("entityRecognitionDeploymentName", NullValueHandling = NullValueHandling.Ignore)]
        public string EntityRecognitionDeploymentNameHeader { get; set; }

        // single classification project
        [JsonProperty("singleClassificationProjectName", NullValueHandling = NullValueHandling.Ignore)]
        public string SingleClassificationProjectNameHeader { get; set; }
        [JsonProperty("singleClassificationDeploymentName", NullValueHandling = NullValueHandling.Ignore)]
        public string SingleClassificationDeploymentNameHeader { get; set; }

        // multi classification project
        [JsonProperty("multiClassificationProjectName", NullValueHandling = NullValueHandling.Ignore)]
        public string MultiClassificationProjectNameHeader { get; set; }
        [JsonProperty("multiClassificationDeploymentName", NullValueHandling = NullValueHandling.Ignore)]
        public string MultiClassificationDeploymentNameHeader { get; set; }
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
