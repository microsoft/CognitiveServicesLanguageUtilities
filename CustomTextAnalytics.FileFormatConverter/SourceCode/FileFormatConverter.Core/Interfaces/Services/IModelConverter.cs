namespace FileFormatConverter.Core.Interfaces.Services
{
    public interface IModelConverter<TModel, TIntermediateModel>
    {
        TIntermediateModel ConvertToIntermediate(TModel sourceModel);
        TModel ConvertFromIntermediate(TIntermediateModel intermediateModel);
    }
}
