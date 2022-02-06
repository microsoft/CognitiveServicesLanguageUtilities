using FileFormatConverter.Core.DataStructures.FileModels.AzureML.Conll;
using FileFormatConverter.Core.DataStructures.FileModels.IntermediateEntitiesModel;
using FileFormatConverter.Core.Interfaces.Services;
using System;

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
            throw new NotImplementedException();
        }
    }
}
