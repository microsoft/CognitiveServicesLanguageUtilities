using Newtonsoft.Json;

namespace Microsoft.CogSLanguageUtilities.Definitions.Models.Configs.Parser
{
    public class ParserConfigModel
    {
        [JsonProperty("MSRead")]
        public MSReadConfigModel MsRead { get; set; }

        public ParserConfigModel()
        {
            MsRead = new MSReadConfigModel();
        }
    }
}
