using Autofac;
using CliTool.Services.Configuration;
using CliTool.Services.Configuration.Models;
using CliTool.Services.Logger;
using CliTool.Services.Parser;
using CliTool.Services.Storage;
using System;

namespace CliTool
{
    class Program
    {
        private static IContainer Container { get; set; }

        static void Main(string[] args)
        {
            // Reading Configuration from file
            IConfigurationService configurationService = new ConfigurationService();
            MSReadConfigModel msReadConfigs = configurationService.GetMSReadConfigModel();

            // Setup DI
            var builder = new ContainerBuilder();    
            builder.RegisterInstance(new ConsoleLoggerService())
                   .As<ILoggerService>();
            builder.RegisterInstance(new MSReadParserService(msReadConfigs.CognitiveServiceEndPoint, msReadConfigs.CongnitiveServiceKey))
                   .As<IParserService>();
            builder.RegisterType<Orchestrator>();
            builder.RegisterType<ConfigurationService>().As<IConfigurationService>();
            builder.RegisterType<StorageFactory>().As<IStorageFactory>();
            Container = builder.Build();

            RunOrchestrator();
        }

        public static void RunOrchestrator()
        {
            using (var scope = Container.BeginLifetimeScope())
            {
                var orchestrator = scope.Resolve<Orchestrator>();
                orchestrator.Run();
            }
        }
    }
}
