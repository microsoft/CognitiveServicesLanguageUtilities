using ProtoBuf;
using System.Collections.Generic;

namespace FuzzyMatching.Definitions.Models
{
    [ProtoContract]
    public class DatasetContainer
    {
        [ProtoMember(1)]
        public List<string> dataset { get; set; }
    }
}
