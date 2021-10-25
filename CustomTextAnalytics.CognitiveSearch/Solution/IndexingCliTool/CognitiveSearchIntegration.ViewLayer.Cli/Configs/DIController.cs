using Autofac;
using Microsoft.CognitiveSearchIntegration.Core.Helpers;
using Microsoft.CognitiveSearchIntegration.Core.Orchestrators;
using Microsoft.CognitiveSearchIntegration.Core.Services.CognitiveSearch;
using Microsoft.CognitiveSearchIntegration.Core.Services.Indexers;
using Microsoft.CognitiveSearchIntegration.Core.Services.Logger;
using Microsoft.CognitiveSearchIntegration.Core.Services.Storage;
using Microsoft.CognitiveSearchIntegration.Definitions.APIs.Services;
using Microsoft.CognitiveSearchIntegration.Definitions.Models.CognitiveSearch.Indexer;
using Microsoft.CognitiveSearchIntegration.ViewLayer.Cli.Configs.ConfigModels;
using Microsoft.CogSLanguageUtilities.Core.Helpers.HttpHandler;
using System.IO;

namespace Microsoft.CognitiveSearchIntegration.ViewLayer.Cli.Configs
{
    public class DIController
    {
        public static IContainer BuildIndexCommandDependencies()
        {
            // load configs
            var appConfigs = LoadApplicationConfigs();

            // register services
            var builder = new ContainerBuilder();
            builder.RegisterType<LocalStorageService>().As<IStorageService>();
            builder.RegisterType<ConsoleLoggerService>().As<ILoggerService>();
            builder.Register(c =>
            {
                return new CustomTextIndexingService(
                    appConfigs.CustomText.EndpointUrl,
                    appConfigs.CustomText.ApiKey,
                    appConfigs.CustomText.ProjectName,
                    appConfigs.CustomText.DeploymentName);
            }).As<ICustomTextIndexingService>();
            builder.Register(c =>
            {
                return new CognitiveSearchClient(new HttpHandler(), appConfigs.CognitiveSearch.EndpointUrl, appConfigs.CognitiveSearch.ApiKey);
            }).As<ICognitiveSearchClient>();
            builder.Register(c =>
            {
                return new IndexingOrchestrator(
                    c.Resolve<IStorageService>(),
                    c.Resolve<ICustomTextIndexingService>(),
                    c.Resolve<ICognitiveSearchClient>(),
                    c.Resolve<ILoggerService>(),
                    new IndexerConfigs
                    {
                        DataSourceConnectionString = appConfigs.BlobStorage.ConnectionString,
                        DataSourceContainerName = appConfigs.BlobStorage.ContainerName,
                        AzureFunctionUrl = appConfigs.AzureFunction.FunctionUrl
                    });
            });
            return builder.Build();
        }

        private static ConfigModel LoadApplicationConfigs()
        {
            var filePath = Path.Combine(Constants.ConfigsFileDirectory, Constants.ConfigsFileName);
            if (File.Exists(filePath))
            {
                var configsFile = File.ReadAllText(filePath);
                return JsonHandler.DeserializeObject<ConfigModel>(configsFile, Constants.ConfigsFileName);
            }
            // throw exception
            throw new Definitions.Exceptions.Storage.FileNotFoundException(filePath);
        }
    }
}
