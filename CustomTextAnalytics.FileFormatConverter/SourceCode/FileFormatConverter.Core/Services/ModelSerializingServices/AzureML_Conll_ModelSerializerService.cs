using FileFormatConverter.Core.DataStructures.FileModels.AzureML.Conll;
using FileFormatConverter.Core.Interfaces.Services;
using System;
using System.Linq;

namespace FileFormatConverter.Core.Services.ModelSerializingServices
{
    internal class AzureML_Conll_ModelSerializerService : IModelSerializer<AzureML_Conll_FileModel>
    {
        private readonly string NoLabelIdentifier = "O";
        private readonly string BeginLabelIdentifier = "B";
        private readonly string ContinueLabelIdentifier = "I";

        private readonly string EmptyTokenText = " ";
        private readonly char SpaceChar = ' ';
        private readonly char DashChar = '-';
        public AzureML_Conll_FileModel Deserialize(string content)
        {
            try
            {
                return ParseFile(content);
            }
            catch (Exception)
            {
                throw new Exception("Invalid Json file format");
            }
        }

        public string Serialize(AzureML_Conll_FileModel model)
        {
            try
            {
                return null;
            }
            catch (Exception)
            {
                throw new Exception("Something went wrong went writing to target file!");
            }
        }

        private AzureML_Conll_FileModel ParseFile(string content)
        {
            var tokens = content
                .Split(Environment.NewLine)
                .Select(line => ParseLine(line));

            return new AzureML_Conll_FileModel()
            {
                Tokens = tokens.ToArray(),
            };
        }

        private Token ParseLine(string line)
        {
            var lineData = line.Split(new char[] { SpaceChar, DashChar });
            var token = new Token()
            {
                RawLine = line
            };

            // case 1: space token
            if (lineData.Length == 0)
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
                token.Text = lineData[0]; /* ATTENTION : handle special case that text has spaces or special characters (split not working properly) */
                var labelIdentifierIndex = lineData.Length - 2;
                token.Label = new Label()
                {
                    Text = lineData[1],
                    TokenType = GetTokenType(lineData[labelIdentifierIndex])
                };
            }
            return token;
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
