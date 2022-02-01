using FileFormatConverter.Core.Models;

namespace FileFormatConverter.Core.Interfaces
{
    public interface IFileParser<TModel>
        where TModel : BaseFileModel
    {
        TModel Parse(string content);
    }
}
