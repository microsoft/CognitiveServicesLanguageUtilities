using Autofac;
using CliTool.Configs.Consts;
using CliTool.Configs.Models.Enums;
using CliTool.ServiceControllers.Controllers;
using CliTool.Services.Configuration;
using CliTool.Services.Logger;
using CliTool.Services.Parser;
using CliTool.Services.Storage;
using CliTool.Services.Storage.StorageServices;
using System;

namespace CliTool.Configs
{
    class DependencyInjectionController
    {
        private static ContainerBuilder BuildCommonDependencies()
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<ConfigurationService>().As<IConfigurationService>();
            builder.RegisterInstance(new ConsoleLoggerService()).As<ILoggerService>();
            return builder;
        }

        public static IContainer BuildConfigCommandDependencies()
        {
            var builder = BuildCommonDependencies();
            builder.RegisterInstance(new LocalStorageService(Constants.ConfigsFileLocalDirectory)).As<IStorageService>();
            builder.RegisterType<ConfigServiceController>();
            return builder.Build();
        }

        public static IContainer BuildParseCommandDependencies(ParserType parserType, StorageType source, StorageType destination)
        {
            var builder = BuildCommonDependencies();
            builder.Register(c =>
            {
                var configService = c.Resolve<IConfigurationService>();
                var loggerService = c.Resolve<ILoggerService>();
                return CreateParserService(parserType, configService, loggerService);
            }).As<IParserService>();
            builder.RegisterType<ParserServiceController>();
            builder.RegisterType<StorageFactory>().As<IStorageFactory>();
            return builder.Build();
        }

        private static IParserService CreateParserService(ParserType parserType, IConfigurationService configService, ILoggerService loggerService)
        {
            if (parserType.Equals(ParserType.MSRead))
            {
                var msReadConfig = configService.GetMSReadConfigModel();
                return new MSReadParserService(loggerService, msReadConfig.CognitiveServiceEndPoint, msReadConfig.CongnitiveServiceKey);
            }
            else
            {
                throw new NotImplementedException();
            }
        }
    }
}
