namespace FileFormatConverter.Core.DataStructures.FileModels.AzureML.Conll
{
    public class AzureML_Conll_FileModel : BaseFileModel
    {
        public Token[] Tokens;
    }

    public class Token
    {
        public string Text { get; set; }
        public Label Label { get; set; }
        public string RawLine { get; set; }
    }

    public class Label
    {
        public TokenType TokenType { get; set; }
        public string Text { get; set; }
    }

    public enum TokenType
    {
        B, // indicates beginning of token of a label
        I // indicates continuing token of label
    }
}
