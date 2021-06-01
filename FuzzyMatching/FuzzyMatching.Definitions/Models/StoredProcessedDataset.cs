using ProtoBuf;

namespace FuzzyMatching.Definitions.Models
{
    [ProtoContract]
    public class StoredProcessedDataset
    {
        [ProtoMember(1)]
        public float[] TFIDFMatrix { get; set; }
        [ProtoMember(2)]
        public float[] TFIDFMatrixAbsoluteValues { get; set; }
        [ProtoMember(3)]
        public float[] IDFVector { get; set; }
        [ProtoMember(4)]
        public string[] UniqueNGramsVector { get; set; }
        [ProtoMember(5)]
        public int Height { get; set; }
        [ProtoMember(6)]
        public int Width { get; set; }
    }
}
