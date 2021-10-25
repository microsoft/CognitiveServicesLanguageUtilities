using Newtonsoft.Json;
using System.Collections.Generic;

namespace Microsoft.CognitiveSearchIntegration.Definitions.Models.CognitiveSearch.Api
{
    public class SkillSet
    {
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("description")]
        public string Description { get; set; }
        [JsonProperty("skills")]
        public List<CustomSkillSchema> Skills { get; set; }
    }
}
