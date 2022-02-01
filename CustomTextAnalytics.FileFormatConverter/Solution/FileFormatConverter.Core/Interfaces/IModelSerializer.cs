namespace FileFormatConverter.Core.Interfaces
{
    public interface IModelSerializer
    {
        T Deserialize<T>(string content);
        string Serialize<T>(T model);
    }
}
