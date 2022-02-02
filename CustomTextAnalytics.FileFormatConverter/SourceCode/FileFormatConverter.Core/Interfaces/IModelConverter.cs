namespace FileFormatConverter.Core.Interfaces
{
    public interface IModelConverter<TSourceModel, TTargetModel>
    {
        TTargetModel ConvertModel(TSourceModel sourceModel);
    }
}
