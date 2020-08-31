using Newtonsoft.Json;

namespace Microsoft.CustomTextCliUtils.ApplicationLayer.Modeling.Models.Configs
{
    public class ParserConfigModel
    {
        [JsonProperty("MSRead")]
        public MSReadConfigModel MsRead { get; set; }

        public ParserConfigModel() {
            MsRead = new MSReadConfigModel();
        }
    }
}
