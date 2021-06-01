using ProtoBuf;

namespace FuzzyMatching.Definitions.Models
{
    [ProtoContract]
    public class ProcessedDataset
    {
        [ProtoMember(1)]
        public float[][] TFIDFMatrix { get; set; }
        [ProtoMember(2)]
        public float[] TFIDFMatrixAbsoluteValues { get; set; }
        [ProtoMember(3)]
        public float[] IDFVector { get; set; }
        [ProtoMember(4)]
        public string[] UniqueNGramsVector { get; set; }
    }
}
