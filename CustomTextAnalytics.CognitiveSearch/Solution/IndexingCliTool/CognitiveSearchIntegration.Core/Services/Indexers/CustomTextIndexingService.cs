using Azure.Search.Documents.Indexes.Models;
using Microsoft.CognitiveSearchIntegration.Definitions.APIs.Services;
using Microsoft.CognitiveSearchIntegration.Definitions.Models.CognitiveSearch.Api;
using Microsoft.CognitiveSearchIntegration.Definitions.Models.CognitiveSearch.Api.Indexer;
using Microsoft.CognitiveSearchIntegration.Definitions.Models.CustomText.Schema;
using System.Collections.Generic;

namespace Microsoft.CognitiveSearchIntegration.Core.Services.Indexers
{
    public class CustomTextIndexingService : ICustomTextIndexingService
    {
        private string _customTextEndpointUrl, _customTextApiKey, _customTextProjectName, _customTextDeploymentName;
        public CustomTextIndexingService(string customTextEndpointUrl, string customTextApiKey, string customTextProjectName, string customTextDeploymentName)
        {
            _customTextEndpointUrl = customTextEndpointUrl;
            _customTextApiKey = customTextApiKey;
            _customTextProjectName = customTextProjectName;
            _customTextDeploymentName = customTextDeploymentName;
        }
        public SkillSet CreateSkillSetSchema(CustomTextSchema schema, string skillSetName, string customTextSkillName, string azureFunctionUrl)
        {
            CustomSkillSchema customSkillSchema = new CustomSkillSchema()
            {
                name = customTextSkillName,
                uri = azureFunctionUrl
            };
            List<Output> outputs = new List<Output>();


            foreach (string entityName in schema.EntityNames)
            {
                Output output = new Output()
                {
                    name = entityName,
                    targetName = entityName
                };
                outputs.Add(output);
            }
            customSkillSchema.outputs = outputs;
            customSkillSchema.HttpHeaders = new HttpHeaders()
            {
                EndpointUrl = _customTextEndpointUrl,
                ApiKey = _customTextApiKey,
                ProjectName = _customTextProjectName,
                DeploymentName = _customTextDeploymentName
            };
            return new SkillSet
            {
                Name = skillSetName,
                Description = "Custom Text Skillset",
                Skills = new List<CustomSkillSchema> { customSkillSchema }
            };
        }

        public SearchIndex CreateIndex(CustomTextSchema schema, string indexName)
        {
            List<SearchField> indexFields = new List<SearchField>();

            // id
            indexFields.Add(new SearchField("id", SearchFieldDataType.String)
            {
                IsKey = true
            });

            // extractors
            foreach (string entityName in schema.EntityNames)
            {
                SearchField indexField = new SearchField(
                    entityName,
                    SearchFieldDataType.String);

                indexFields.Add(indexField);
            }

            return new SearchIndex(indexName)
            {
                Fields = indexFields
            };
        }

        public Indexer CreateIndexer(CustomTextSchema schema, string indexerName, string dataSourceName, string skillSetName, string indexName)
        {
            var outputFieldMappings = new List<IndexerFieldMapping>();

            // entities
            foreach (string entityName in schema.EntityNames)
            {
                outputFieldMappings.Add(new IndexerFieldMapping
                {
                    SourceFieldName = $"/document/content/{entityName}",
                    TargetFieldName = entityName
                });
            }

            List<IndexerFieldMapping> fieldMappings = new List<IndexerFieldMapping>
            {
                new IndexerFieldMapping
                {
                    SourceFieldName = "metadata_storage_name",
                    TargetFieldName = "id",
                    MappingFunction = new MappingFunction
                    {
                        Name = "base64Encode"
                    }
                }
            };

            var indexerParameters = new IndexerParameters
            {
                Configuration = new IndexerConfiguration
                {
                    IndexedFileNameExtensions = ".txt"
                }
            };

            var indexer = new Indexer
            {
                Name = indexerName,
                DataSourceName = dataSourceName,
                TargetIndexName = indexName,
                SkillsetName = skillSetName,
                FieldMappings = fieldMappings,
                OutputFieldMappings = outputFieldMappings,
                Parameters = indexerParameters
            };
            return indexer;
        }
    }
}
