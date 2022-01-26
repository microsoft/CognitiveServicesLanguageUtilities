using Autofac;
using CognitiveSearchIntegration.Common.Logging;
using CognitiveSearchIntegration.Runners.Models.ConfigsModel;
using CognitiveSearchIntegration.Runners.Services;
using CognitiveSearchIntegration.Runners.Services.Loaders;
using CognitiveSearchIntegration.Runners.Services.Validation;
using CognitiveSearchIntegration.Runners.Services.Validation.ConfigsValidation;
using Microsoft.CognitiveSearchIntegration.Core;
using Microsoft.CognitiveSearchIntegration.Definitions.Models;
using System;
using System.Threading.Tasks;

namespace CognitiveSearchIntegration.Runners
{
    public class IndexingOperationRunner
    {
        private static ConfigsModelLoader _configsLoader = new ConfigsModelLoader();
        private static ConfigsValidationService _configsValidationService = new ConfigsValidationService();
        private static CustomTextSchemaLoader _customtextSchemaLoader = new CustomTextSchemaLoader();
        private static CustomTextSchemaValidationService _schemaValidationService = new CustomTextSchemaValidationService();
        private static ILoggerService _logger = new ConsoleLoggerService();
        public static async Task RunOperation(string indexName, string configsFileDir)
        {
            try
            {
                await RunOperationInternal(indexName, configsFileDir);
            }
            catch (Exception e)
            {
                _logger.LogUnhandledError(e);
            }
        }
        private static async Task RunOperationInternal(string indexName, string configsFileDir)
        {
            // load configs and schema
            var appConfigs = await LoadAndValidateConfigs(configsFileDir);
            var selectedProjects = SelectedProjectsLoader.LoadSelectedProjects(appConfigs.CustomText.Projects);
            var customTextSchema = await LoadAndValidateAppSchema(appConfigs, selectedProjects);

            // build dependencies
            var container = DependencyInjector.BuildDependencies(appConfigs);

            // run
            using (var scope = container.BeginLifetimeScope())
            {
                var orchestrator = scope.Resolve<IndexingOrchestrator>();
                await orchestrator.IndexCustomText(indexName, customTextSchema, selectedProjects);
            }
        }

        private static async Task<ConfigModel> LoadAndValidateConfigs(string configsFileDir)
        {
            // log operation
            _logger.LogOperation(OperationType.ReadAndValidateConfigs, $"configs.json");

            // load configs
            var appConfigs = _configsLoader.Load(configsFileDir);

            // validate configs
            var selectedProjects = SelectedProjectsLoader.LoadSelectedProjects(appConfigs.CustomText.Projects);
            await _configsValidationService.ValidateAppConfigs(appConfigs, selectedProjects);

            return appConfigs;
        }

        private static async Task<CustomTextSchema> LoadAndValidateAppSchema(ConfigModel appConfigs, SelectedProjects selectedProjects)
        {
            // log operation
            _logger.LogOperation(OperationType.ReadAndValidateAppSchema, $"CustomText Resource");

            // load schema
            var customTextSchema = await _customtextSchemaLoader.LoadCustomTextAppSchema(
                appConfigs.CustomText.Resource,
                appConfigs.CustomText.Projects,
                selectedProjects);

            // validate schema
            _schemaValidationService.ValidateSchema(customTextSchema, appConfigs.CustomText.Projects, selectedProjects);

            return customTextSchema;
        }
    }
}
