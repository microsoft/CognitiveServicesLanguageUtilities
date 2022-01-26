
using System.Collections.Generic;

namespace CustomSkillsetFunction.ViewModels.Response
{
    public class ResponseRecord
    {
        public string RecordId { get; set; }
        public Dictionary<string, object> Data { get; set; } = new Dictionary<string, object>();
        public List<ErrorWarning> Errors { get; set; } = new List<ErrorWarning>();
        public List<ErrorWarning> Warnings { get; set; } = new List<ErrorWarning>();
    }
}