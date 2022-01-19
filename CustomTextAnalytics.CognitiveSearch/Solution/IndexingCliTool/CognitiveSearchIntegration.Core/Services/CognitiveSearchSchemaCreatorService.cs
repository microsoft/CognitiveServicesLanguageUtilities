using Azure.Search.Documents.Indexes.Models;
using CognitiveSearchIntegration.Runners.Models.ConfigsModel;
using Microsoft.CognitiveSearchIntegration.Definitions.Configs;
using Microsoft.CognitiveSearchIntegration.Definitions.Models;
using Microsoft.CognitiveSearchIntegration.Utilities.Nugets.CognitiveSearchClient.RestClient.ApiModels;
using Microsoft.CognitiveSearchIntegration.Utilities.Nugets.CognitiveSearchClient.RestClient.ApiModels.Indexer;
using System.Collections.Generic;

namespace Microsoft.CognitiveSearchIntegration.Core.Services
{
    public class CognitiveSearchSchemaCreatorService
    {
        public CognitiveSearchSchemaCreatorService() { }

        public SearchIndex CreateSearchIndexSchema(CustomTextSchema schema, string indexName, SelectedProjects selectedProjects)
        {
            // core fields
            var indexFields = new List<SearchField>
            {
                new SearchField("id", SearchFieldDataType.String) { IsKey = true },
                new SearchField("document_name", SearchFieldDataType.String),
                new SearchField("document_uri", SearchFieldDataType.String)
            };

            // classifers
            if (selectedProjects.IsSelected_SingleClassificationProject)
            {
                var singleClassField = new SearchField(
                    Constants.SearchIndexSingleClassColumnName,
                    SearchFieldDataType.String);
                indexFields.Add(singleClassField);
            }
            if (selectedProjects.IsSelected_MultiClassificationProject)
            {
                var multiClassField = new SearchField(
                    Constants.SearchIndexMultiClassColumnName,
                    SearchFieldDataType.Collection(SearchFieldDataType.String));
                indexFields.Add(multiClassField);
            }

            // extractors
            if (selectedProjects.IsSelected_EntityRecognitionProject)
            {
                foreach (var entityName in schema.EntityNames)
                {
                    var entityField = new SearchField(
                        entityName,
                        SearchFieldDataType.Collection(SearchFieldDataType.String));
                    indexFields.Add(entityField);
                }
            }

            // return
            return new SearchIndex(indexName)
            {
                Fields = indexFields
            };
        }

        public SkillSet CreateSkillSetSchema(
            string skillSetName,
            string customTextSkillName,
            string azureFunctionUrl,
            CustomTextConfigModel customTextConfigs,
            SelectedProjects selectedProjects)
        {
            var customSkillSchema = CreateCustomSkillSchema(
                customTextSkillName,
                azureFunctionUrl,
                customTextConfigs,
                selectedProjects);
            return new SkillSet
            {
                Name = skillSetName,
                Description = "Custom Text Skillset",
                Skills = new List<CustomSkillSchema> { customSkillSchema }
            };
        }

        private static CustomSkillSchema CreateCustomSkillSchema(
            string customTextSkillName,
            string azureFunctionUrl,
            CustomTextConfigModel customTextConfigs,
            SelectedProjects selectedProjects)
        {
            // basic info
            var customSkillSchema = new CustomSkillSchema()
            {
                name = customTextSkillName,
                uri = azureFunctionUrl
            };

            // add headers / outputs
            var outputs = new List<Output>();
            var headers = new HttpHeaders()
            {
                CustomTextResourceEndpointHeader = customTextConfigs.Resource.Endpoint,
                CustomTextResourceKeyHeader = customTextConfigs.Resource.Key
            };

            if (selectedProjects.IsSelected_EntityRecognitionProject)
            {
                headers.EntityRecognitionProjectNameHeader = customTextConfigs.Projects.EntityRecognition.ProjectName;
                headers.EntityRecognitionDeploymentNameHeader = customTextConfigs.Projects.EntityRecognition.DeploymentName;
                var output = new Output()
                {
                    name = Constants.SkillsetResponseEntitiesKey,
                    targetName = Constants.SkillsetResponseEntitiesKey
                };
                outputs.Add(output);
            }
            if (selectedProjects.IsSelected_SingleClassificationProject)
            {
                headers.SingleClassificationProjectNameHeader = customTextConfigs.Projects.SingleClassification.ProjectName;
                headers.SingleClassificationDeploymentNameHeader = customTextConfigs.Projects.SingleClassification.DeploymentName;
                var output = new Output()
                {
                    name = Constants.SkillsetResponseSingleClassKey,
                    targetName = Constants.SkillsetResponseSingleClassKey
                };
                outputs.Add(output);
            }
            if (selectedProjects.IsSelected_MultiClassificationProject)
            {
                headers.MultiClassificationProjectNameHeader = customTextConfigs.Projects.MultiClassification.ProjectName;
                headers.MultiClassificationDeploymentNameHeader = customTextConfigs.Projects.MultiClassification.DeploymentName;
                var output = new Output()
                {
                    name = Constants.SkillsetResponseMultiClassKey,
                    targetName = Constants.SkillsetResponseMultiClassKey
                };
                outputs.Add(output);
            }

            customSkillSchema.HttpHeaders = headers;
            customSkillSchema.outputs = outputs;


            // return 
            return customSkillSchema;
        }

        public Indexer CreateIndexerSchema(
            CustomTextSchema schema,
            string indexerName,
            string dataSourceName,
            string skillSetName,
            string indexName,
            SelectedProjects selectedProjects)
        {
            // field mappings
            var fieldMappings = new List<IndexerFieldMapping>
            {
                new IndexerFieldMapping
                {
                    SourceFieldName = "metadata_storage_name",
                    TargetFieldName = "id",
                    MappingFunction = new MappingFunction
                    {
                        Name = "base64Encode"
                    }
                },
                new IndexerFieldMapping
                {
                    SourceFieldName = "metadata_storage_name",
                    TargetFieldName = "document_name",
                },
                new IndexerFieldMapping
                {
                    SourceFieldName = "metadata_storage_path",
                    TargetFieldName = "document_uri",
                }
            };

            // output fields mapping
            var outputFieldMappings = new List<IndexerFieldMapping>();
            if (selectedProjects.IsSelected_EntityRecognitionProject)
            {
                foreach (string entityName in schema.EntityNames)
                {
                    outputFieldMappings.Add(new IndexerFieldMapping
                    {
                        SourceFieldName = $"/document/content/{Constants.SkillsetResponseEntitiesKey}/{entityName}",
                        TargetFieldName = entityName
                    });
                }
            }
            if (selectedProjects.IsSelected_SingleClassificationProject)
            {
                outputFieldMappings.Add(new IndexerFieldMapping
                {
                    SourceFieldName = $"/document/content/{Constants.SkillsetResponseSingleClassKey}",
                    TargetFieldName = Constants.SearchIndexSingleClassColumnName
                });
            }
            if (selectedProjects.IsSelected_MultiClassificationProject)
            {
                outputFieldMappings.Add(new IndexerFieldMapping
                {
                    SourceFieldName = $"/document/content/{Constants.SkillsetResponseMultiClassKey}",
                    TargetFieldName = Constants.SearchIndexMultiClassColumnName
                });
            }

            // configs
            var indexerParameters = new IndexerParameters
            {
                Configuration = new IndexerConfiguration
                {
                    IndexedFileNameExtensions = ".txt"
                }
            };

            return new Indexer
            {
                Name = indexerName,
                DataSourceName = dataSourceName,
                TargetIndexName = indexName,
                SkillsetName = skillSetName,
                FieldMappings = fieldMappings,
                OutputFieldMappings = outputFieldMappings,
                Parameters = indexerParameters
            };
        }
    }
}
