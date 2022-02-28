namespace FileFormatConverter.Core.Interfaces.Services
{
    public interface IModelConverter<TModel, TIntermediateModel>
    {
        TIntermediateModel ConvertToIntermediate(TModel sourceModel, string language);
        TModel ConvertFromIntermediate(TIntermediateModel intermediateModel);
    }
}
