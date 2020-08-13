using Autofac;
using CliTool.Services.Configuration;
using CliTool.Services.Configuration.Models;
using CliTool.Services.Logger;
using CliTool.Services.Parser;
using CliTool.Services.Storage;
using System.Threading.Tasks;

namespace CliTool
{
    class Program
    {
        private static IContainer Container { get; set; }

        static async Task Main(string[] args)
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

            await RunOrchestratorAsync();
        }

        public static async Task RunOrchestratorAsync()
        {
            using (var scope = Container.BeginLifetimeScope())
            {
                var orchestrator = scope.Resolve<Orchestrator>();
                await orchestrator.RunAsync();
            }
        }
    }
}
