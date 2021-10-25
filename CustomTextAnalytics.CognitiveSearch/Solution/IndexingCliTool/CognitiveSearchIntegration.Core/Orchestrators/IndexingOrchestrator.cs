using Microsoft.CognitiveSearchIntegration.Core.Helpers;
using Microsoft.CognitiveSearchIntegration.Core.Services.SchemaValidation;
using Microsoft.CognitiveSearchIntegration.Definitions.APIs.Services;
using Microsoft.CognitiveSearchIntegration.Definitions.Consts;
using Microsoft.CognitiveSearchIntegration.Definitions.Enums.Logger;
using Microsoft.CognitiveSearchIntegration.Definitions.Models.CognitiveSearch.Indexer;
using Microsoft.CognitiveSearchIntegration.Definitions.Models.CustomText.Schema;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.CognitiveSearchIntegration.Core.Orchestrators
{
    public class IndexingOrchestrator
    {
        private IStorageService _storageService;
        private ICustomTextIndexingService _customTextIndexingService;
        private ICognitiveSearchClient _cognitiveSearchService;
        private ILoggerService _loggerService;
        private IndexerConfigs _indexerConfigs;

        public IndexingOrchestrator(
            IStorageService storageService,
            ICustomTextIndexingService customTextIndexingService,
            ICognitiveSearchClient cognitiveSearchService,
            ILoggerService loggerService,
            IndexerConfigs indexerConfigs)
        {
            _storageService = storageService;
            _customTextIndexingService = customTextIndexingService;
            _cognitiveSearchService = cognitiveSearchService;
            _loggerService = loggerService;
            _indexerConfigs = indexerConfigs;
        }
        public async Task IndexCustomText(string schemaPath, string indexName)
        {
            // initialize resource names
            var dataSourceName = indexName.ToLower() + Constants.DataSourceSuffix;
            var indexerName = indexName.ToLower() + Constants.IndexerSuffix;
            var skillSetName = indexName.ToLower() + Constants.SkillsetSuffix;
            var customTextSkillName = indexName.ToLower() + Constants.CustomSkillSuffix;
            var schemaFileName = Path.GetFileName(schemaPath);

            // read schema
            _loggerService.LogOperation(OperationType.ReadingAppSchema, schemaFileName);
            var schemaString = await _storageService.ReadAsStringFromAbsolutePathAsync(schemaPath);
            var schema = JsonHandler.DeserializeObject<CustomTextSchema>(schemaString, schemaFileName);

            // tmp: validate entity names can't have spaces
            CustomTextSchemaValidationService.ValidateAppSchema(schema);

            // create models (index & skillset)
            var searchIndex = _customTextIndexingService.CreateIndex(schema, indexName);
            var customSkill = _customTextIndexingService.CreateSkillSetSchema(schema, skillSetName, customTextSkillName, _indexerConfigs.AzureFunctionUrl);
            var searchIndexer = _customTextIndexingService.CreateIndexer(schema, indexerName, dataSourceName, skillSetName, indexName);

            // indexing pipeline
            _loggerService.LogOperation(OperationType.CreateDataSource, $"{dataSourceName}");
            await _cognitiveSearchService.CreateDataSourceConnectionAsync(dataSourceName, _indexerConfigs.DataSourceContainerName, _indexerConfigs.DataSourceConnectionString);

            _loggerService.LogOperation(OperationType.CreatingSearchIndex, $"{indexName}");
            await _cognitiveSearchService.CreateIndexAsync(searchIndex);

            _loggerService.LogOperation(OperationType.CreatingSkillSet, $"{skillSetName}");
            await _cognitiveSearchService.CreateSkillSetAsync(customSkill);

            _loggerService.LogOperation(OperationType.CreatingIndexer, $"{indexerName}");
            await _cognitiveSearchService.CreateIndexerAsync(searchIndexer);

            // log success message
            _loggerService.LogSuccessMessage("Indexing Application Was Successfull!");
        }
    }
}
