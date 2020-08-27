namespace CustomTextCliUtils.ApplicationLayer.Modeling.Models.Chunker
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
            Summary = string.Format("{0} ... {1}", text.Substring(0, text.IndexOf(' ')), text.Substring(text.LastIndexOf(' ') + 1));
        }
        public ChunkInfo(string chunkText)
        {
            var text = chunkText.Trim();
            Text = text;
            // TODO: check if no spaces
            Summary = string.Format("{0} ... {1}", text.Substring(0, text.IndexOf(' ')), text.Substring(text.LastIndexOf(' ') + 1));
        }
        public ChunkInfo()
        { }
    }
}
