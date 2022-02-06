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
            var entities = ExtractEntityNames(sourceModel);

            // extract documents
            var document = new EntityDocument()
            {
                Culture = null,
                Location = null,
                Entities = null
            };

            return new IntermediateEntitiesModel()
            {
                EntityNames = entities.ToArray(),
                Documents = new EntityDocument[]
                {
                    document
                }
            };
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
    }
}
