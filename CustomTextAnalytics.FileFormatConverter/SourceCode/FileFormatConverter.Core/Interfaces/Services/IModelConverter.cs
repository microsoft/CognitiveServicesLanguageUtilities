namespace FileFormatConverter.Core.Interfaces.Services
{
    public interface IModelConverter<TSourceModel, TTargetModel>
    {
        TTargetModel ConvertModel(TSourceModel sourceModel);
    }
}
