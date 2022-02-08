using FileFormatConverter.Core.DataStructures.FileModels.AzureML.Conll;
using FileFormatConverter.Core.Interfaces.Services;
using System;
using System.Linq;

namespace FileFormatConverter.Core.Services.ModelSerializingServices
{
    public class AzureML_Conll_ModelSerializerService : IModelSerializer<AzureML_Conll_FileModel>
    {
        private readonly string NoLabelIdentifier = "O";
        private readonly string BeginLabelIdentifier = "B";
        private readonly string ContinueLabelIdentifier = "I";

        private readonly string EmptyTokenText = " ";
        private readonly char NewLineWindowsChar = '\n';
        private readonly char NewLineCharMacOs = '\r';
        private readonly char SpaceChar = ' ';
        private readonly char DashChar = '-';
        private readonly char TabChar = '\t';
        public AzureML_Conll_FileModel Deserialize(string content)
        {
            try
            {
                return ParseFile(content);
            }
            catch (Exception)
            {
                throw new Exception("Invalid Conll file format");
            }
        }

        public string Serialize(AzureML_Conll_FileModel model)
        {
            throw new NotImplementedException();
        }

        private AzureML_Conll_FileModel ParseFile(string content)
        {
            var lines = content.Split(new char[] { NewLineWindowsChar, NewLineCharMacOs });
            var tokens = lines.Select(line => ParseLine(line));

            return new AzureML_Conll_FileModel()
            {
                Tokens = tokens.ToArray(),
            };
        }

        private Token ParseLine(string line)
        {
            try
            {
                var lineData = line.Split(new char[] { SpaceChar, TabChar }); // split line by space/tab
                var token = new Token()
                {
                    RawLine = line
                };

                // case 1: space token
                if (line.Length == 0 || string.IsNullOrEmpty(lineData[0]) || string.IsNullOrWhiteSpace(lineData[0]))
                {
                    token.Text = EmptyTokenText;
                }
                // case 2: no labels
                else if (IsLineNotLabeled(lineData))
                {
                    token.Text = lineData[0];
                }
                // case 3: labeled text
                else
                {
                    token.Text = lineData[0];
                    var labelData = lineData.Last().Split(new char[] { DashChar }); // extract label name and type removing 'dash' char
                    token.Label = new Label()
                    {
                        Text = labelData[1],
                        TokenType = GetTokenType(labelData[0])
                    };
                }
                return token;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        private bool IsLineNotLabeled(string[] lineData)
        {
            var lineEndToken = lineData.Last();
            return string.Equals(lineEndToken, NoLabelIdentifier, StringComparison.OrdinalIgnoreCase);

        }

        private TokenType GetTokenType(string discriminator)
        {
            if (string.Equals(discriminator, BeginLabelIdentifier, StringComparison.OrdinalIgnoreCase))
            {
                return TokenType.B;
            }
            if (string.Equals(discriminator, ContinueLabelIdentifier, StringComparison.OrdinalIgnoreCase))
            {
                return TokenType.I;
            }
            throw new Exception("Invalid label discriminator!");
        }
    }
}
