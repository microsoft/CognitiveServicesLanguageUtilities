using FileFormatConverter.Core.Interfaces;
using FileFormatConverter.Core.Interfaces.Services;

namespace FileFormatConverter.Core
{
    public class FileConversionOrchestrator<TSourceModel, TIntermediateModel, TTargetModel> : IFileConversionOrchestrator
    {
        private IFileHandler _fileHandlerService;
        private IModelSerializer<TSourceModel> _sourceModelSerializerService;
        private IModelConverter<TSourceModel, TIntermediateModel> _sourceModelConverterService;
        private IModelConverter<TTargetModel, TIntermediateModel> _targetModelConverterService;
        private IModelSerializer<TTargetModel> _targetModelSerializerService;

        public FileConversionOrchestrator(
            IFileHandler fileHandlerService,
            IModelSerializer<TSourceModel> sourceModelSerializerService,
            IModelConverter<TSourceModel, TIntermediateModel> sourceModelConverterService,
            IModelConverter<TTargetModel, TIntermediateModel> targetModelConverterService,
            IModelSerializer<TTargetModel> targetModelSerializerService)
        {
            _fileHandlerService = fileHandlerService;
            _sourceModelSerializerService = sourceModelSerializerService;
            _sourceModelConverterService = sourceModelConverterService;
            _targetModelConverterService = targetModelConverterService;
            _targetModelSerializerService = targetModelSerializerService;
        }

        public void ConvertFile(string inputFilePath, string targetFilePath)
        {
            // read input file
            var fileContent = _fileHandlerService.ReadFileAsString(inputFilePath);

            // parse file
            var sourceModel = _sourceModelSerializerService.Deserialize(fileContent);

            // convert source to intermediate model
            var intermediateModel = _sourceModelConverterService.ConvertToIntermediate(sourceModel);

            // convert intermediate to target model
            var targetModel = _targetModelConverterService.ConvertFromIntermediate(intermediateModel);

            // serialize model
            var serializedTargetModel = _targetModelSerializerService.Serialize(targetModel);

            // save output
            _fileHandlerService.WriteFileAsString(targetFilePath, serializedTargetModel);
        }
    }
}
