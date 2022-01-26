using CognitiveSearchIntegration.Common.Logging;
using CognitiveSearchIntegration.Runners.Models.ConfigsModel;
using Microsoft.CognitiveSearchIntegration.Core.Services;
using Microsoft.CognitiveSearchIntegration.Definitions.Models;
using System.Threading.Tasks;

namespace Microsoft.CognitiveSearchIntegration.Core
{
    public class IndexingOrchestrator
    {
        private CognitiveSearchSchemaCreatorService _cognitiveSearchSchemaCreatorService;
        private CognitiveSearchService _cognitiveSearchService;
        private ILoggerService _loggerService;
        private ConfigModel _appConfigs;

        public IndexingOrchestrator(
            CognitiveSearchSchemaCreatorService cognitiveSearchSchemaCreatorService,
            CognitiveSearchService cognitiveSearchService,
            ILoggerService loggerService,
            ConfigModel appConfigs)
        {
            _cognitiveSearchSchemaCreatorService = cognitiveSearchSchemaCreatorService;
            _cognitiveSearchService = cognitiveSearchService;
            _loggerService = loggerService;
            _appConfigs = appConfigs;
        }
        public async Task IndexCustomText(
            string indexName,
            CustomTextSchema customtexSchema,
            SelectedProjects selectedProjects)
        {
            // initialize resource names
            var dataSourceName = indexName.ToLower() + "-data";
            var indexerName = indexName.ToLower() + "-indexer";
            var skillSetName = indexName.ToLower() + "-skillset";
            var customTextSkillName = indexName.ToLower() + "-customtext-skill";

            // create models (index & skillset)
            var indexSchema = _cognitiveSearchSchemaCreatorService.CreateSearchIndexSchema(
                customtexSchema,
                indexName,
                selectedProjects);
            var skillsetSchema = _cognitiveSearchSchemaCreatorService.CreateSkillSetSchema(
                skillSetName,
                customTextSkillName,
                _appConfigs.AzureFunction.FunctionUrl,
                _appConfigs.CustomText,
                selectedProjects);
            var indexerSchema = _cognitiveSearchSchemaCreatorService.CreateIndexerSchema(
                customtexSchema,
                indexerName,
                dataSourceName,
                skillSetName,
                indexName,
                selectedProjects);

            // indexing pipeline
            _loggerService.LogOperation(OperationType.CreateDataSource, $"{dataSourceName}");
            await _cognitiveSearchService.CreateDataSourceConnectionAsync(dataSourceName, _appConfigs.BlobStorage.ContainerName, _appConfigs.BlobStorage.ConnectionString);

            _loggerService.LogOperation(OperationType.CreatingSearchIndex, $"{indexName}");
            await _cognitiveSearchService.CreateIndexAsync(indexSchema);

            _loggerService.LogOperation(OperationType.CreatingSkillSet, $"{skillSetName}");
            await _cognitiveSearchService.CreateSkillSetAsync(skillsetSchema);

            _loggerService.LogOperation(OperationType.CreatingIndexer, $"{indexerName}");
            await _cognitiveSearchService.CreateIndexerAsync(indexerSchema);

            // log success message
            _loggerService.LogSuccessMessage("Indexing Application Was Successfull!");
        }
    }
}
