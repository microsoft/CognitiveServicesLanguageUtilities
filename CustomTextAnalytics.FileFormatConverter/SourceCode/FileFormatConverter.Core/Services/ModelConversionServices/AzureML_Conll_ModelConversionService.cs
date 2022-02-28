﻿using FileFormatConverter.Core.DataStructures.FileModels.AzureML.Conll;
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

        public IntermediateEntitiesModel ConvertToIntermediate(AzureML_Conll_FileModel sourceModel, string language)
        {
            // extract entity names
            var allEntityNames = GetExtractors(sourceModel);

            // extract documents
            var labels = ExtractLabels(sourceModel);

            // conver overall model
            var regionLength = GetRegionOffset(labels);
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
                                RegionOffset = 0,
                                RegionLength = regionLength,
                                Labels = labels.ToArray()
                            }
                        }
                    }
                }
            };
        }

        private long GetRegionOffset(List<CustomLabel> labels)
        {
            var endLabelOffset = labels.Last()?.Offset ?? 0;
            var endLabellength = labels.Last()?.Length ?? 0;
            return endLabelOffset + endLabellength;
        }

        private List<CustomLabel> ExtractLabels(AzureML_Conll_FileModel sourceModel)
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
                    var entityIndex = conllLabel.Text;
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

        private IEnumerable<CustomExtractorInfo> GetExtractors(AzureML_Conll_FileModel sourceModel)
        {
            return sourceModel.Tokens
                .Where(token => token.Label != null)
                .Select(token => token.Label.Text)
                .ToHashSet()
                .Select(text =>
                {
                    return new CustomExtractorInfo() { Name = text };
                });
        }
    }
}
