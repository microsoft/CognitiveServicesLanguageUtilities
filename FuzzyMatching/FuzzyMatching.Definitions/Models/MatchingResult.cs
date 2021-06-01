namespace FuzzyMatching.Definitions.Models
{
    public class MatchingResult
    {
        public string ClosestSentence { get; set; }
        public float MatchingScore { get; set; }
        public int MatchingIndex { get; set; }
    }
}
