namespace FileFormatConverter.Core.Interfaces
{
    public interface IModelSerializer<TModel>
    {
        TModel Deserialize(string content);
        string Serialize(TModel model);
    }
}
