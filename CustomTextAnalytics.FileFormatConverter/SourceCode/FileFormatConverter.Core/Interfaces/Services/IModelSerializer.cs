namespace FileFormatConverter.Core.Interfaces.Services
{
    public interface IModelSerializer<TModel>
    {
        TModel Deserialize(string content);
        string Serialize(TModel model);
    }
}
