
using System.Collections.Generic;

namespace CustomSkillsetFunction.ViewModels.Request
{
    public class RequestRecord
    {
        public string RecordId { get; set; }
        public Dictionary<string, object> Data { get; set; } = new Dictionary<string, object>();
    }
}