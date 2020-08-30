using CustomTextCliUtils.ApplicationLayer.Helpers.Models;

namespace  Microsoft.CustomTextCliUtils.ApplicationLayer.Modeling.Models.Chunker
{
    public class ChunkInfo
    {
        public int StartPage { get; set; }
        public int EndPage { get; set; }
        public string Summary { get; set; }
        public string Text { get; set; }
        public ChunkInfo(string chunkText, int startPage, int endPage)
        {
            var text = chunkText.Trim();
            Text = text;
            StartPage = startPage;
            EndPage = endPage;
            Summary = ChunkInfoHelper.GetChunksummary(text);
        }
        public ChunkInfo(string chunkText)
        {
            var text = chunkText.Trim();
            Text = text;
            Summary = ChunkInfoHelper.GetChunksummary(text);
        }
        public ChunkInfo()
        { }
    }
}
