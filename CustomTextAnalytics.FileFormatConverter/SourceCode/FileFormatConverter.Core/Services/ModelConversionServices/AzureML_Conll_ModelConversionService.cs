using FileFormatConverter.Core.DataStructures.FileModels.AzureML.Conll;
using FileFormatConverter.Core.DataStructures.FileModels.IntermediateEntitiesModel;
using FileFormatConverter.Core.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FileFormatConverter.Core.Services.ModelConversionServices
{
    public class AzureML_Conll_ModelConversionService : IModelConverter<AzureML_Conll_FileModel, IntermediateEntitiesModel>
    {
        public AzureML_Conll_FileModel ConvertFromIntermediate(IntermediateEntitiesModel intermediateModel)
        {
            throw new NotImplementedException();
        }

        public IntermediateEntitiesModel ConvertToIntermediate(AzureML_Conll_FileModel sourceModel)
        {
            // extract entity names
            var allEntityNames = ExtractEntityNames(sourceModel);

            // create entity names map
            var allEntitiesMap = CreateEntitiesMap(allEntityNames);

            // extract documents
            var labels = ExtractLabels(sourceModel, allEntitiesMap);

            // conver overall model
            return new IntermediateEntitiesModel()
            {
                Extractors = allEntityNames.ToArray(),
                Documents = new CustomDocument[]
                {
                    new CustomDocument()
                    {
                        Extractors = new CustomExtractor[]
                        {
                            new CustomExtractor()
                            {
                                Labels = labels.ToArray()
                            }
                        }
                    }
                }
            };
        }

        private List<CustomLabel> ExtractLabels(AzureML_Conll_FileModel sourceModel, Dictionary<string, int> allEntitiesMap)
        {
            var i = 0;
            var charIndex = 0;
            var tokensArray = sourceModel.Tokens.ToArray();
            var labels = new List<CustomLabel>();
            while (i < tokensArray.Length)
            {
                if (tokensArray[i].Label?.TokenType == TokenType.B)
                {
                    // get label basic info
                    var conllLabel = tokensArray[i].Label;
                    var entityIndex = allEntitiesMap[conllLabel.Text];
                    var start = charIndex;

                    // get length
                    var length = GetLabelLength(ref i, ref charIndex, tokensArray);

                    // create label
                    var label = new CustomLabel()
                    {
                        ExtractorName = entityIndex,
                        Offset = start,
                        Length = length,
                    };
                    labels.Add(label);
                }
                else
                {
                    charIndex += tokensArray[i].Text.Length;
                    i++;
                }
            }

            return labels;
        }

        private int GetLabelLength(ref int i, ref int charIndex, Token[] tokensArray)
        {
            var j = i + 1;
            var length = tokensArray[i].Text.Length;
            while (true)
            {
                if (j >= tokensArray.Length || tokensArray[j].Label == null || tokensArray[j].Label?.TokenType == TokenType.B)
                {
                    break;
                }
                length += tokensArray[j].Text.Length;
                j++;
            }

            i = j;
            charIndex += length;

            return length;
        }

        private HashSet<string> ExtractEntityNames(AzureML_Conll_FileModel sourceModel)
        {
            var entities = new HashSet<string>();
            sourceModel.Tokens.ToList().ForEach(token =>
            {
                if (token.Label != null)
                {
                    entities.Add(token.Label.Text);
                }
            });
            return entities;
        }

        private Dictionary<string, int> CreateEntitiesMap(IEnumerable<string> allEntityNames)
        {
            var allEntitiesMap = new Dictionary<string, int>();
            var tmp = allEntityNames.ToArray();
            for (var i = 0; i < tmp.Length; i++)
            {
                allEntitiesMap[tmp[i]] = i;
            }
            return allEntitiesMap;
        }
    }
}
