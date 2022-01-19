using Autofac;
using CognitiveSearchIntegration.Common.Logging;
using CognitiveSearchIntegration.Runners.Models.ConfigsModel;
using Microsoft.CognitiveSearchIntegration.Core;
using Microsoft.CognitiveSearchIntegration.Core.Services;

namespace CognitiveSearchIntegration.Runners.Services
{
    public class DependencyInjector
    {
        public static IContainer BuildDependencies(ConfigModel appConfigs)
        {
            var builder = new ContainerBuilder();

            // register services
            builder.RegisterType<ConsoleLoggerService>().As<ILoggerService>();
            builder.Register(c =>
            {
                return new CognitiveSearchSchemaCreatorService();
            }).As<CognitiveSearchSchemaCreatorService>();

            builder.Register(c =>
            {
                return new CognitiveSearchService(appConfigs.CognitiveSearch.EndpointUrl, appConfigs.CognitiveSearch.ApiKey);
            }).As<CognitiveSearchService>();

            builder.Register(c =>
            {
                return new IndexingOrchestrator(
                    c.Resolve<CognitiveSearchSchemaCreatorService>(),
                    c.Resolve<CognitiveSearchService>(),
                    c.Resolve<ILoggerService>(),
                    appConfigs);
            });
            return builder.Build();
        }
    }
}
