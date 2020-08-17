using Autofac;
using CliTool.ServiceControllers.Controllers;
using CliTool.Services.Configuration;
using CliTool.Services.Configuration.Models;
using CliTool.Services.Logger;
using CliTool.Services.Parser;
using CliTool.Services.Storage;
using System.Threading.Tasks;

namespace CliTool.Configs
{
    class DependencyInjectionController
    {
        public static IContainer BuildDependencies() {
            IConfigurationService configurationService = new ConfigurationService();
            MSReadConfigModel msReadConfigs = configurationService.GetMSReadConfigModel();

            // Setup DI
            var builder = new ContainerBuilder();
            builder.RegisterInstance(new ConsoleLoggerService())
                   .As<ILoggerService>();
            builder.RegisterInstance(new MSReadParserService(msReadConfigs.CognitiveServiceEndPoint, msReadConfigs.CongnitiveServiceKey))
                   .As<IParserService>();
            builder.RegisterType<ParserServiceController>();
            builder.RegisterType<ConfigurationService>().As<IConfigurationService>();
            builder.RegisterType<StorageFactory>().As<IStorageFactory>();

            return builder.Build();
        }
    }
}
