using Autofac;
using CliTool.AppController.Factories.Storage;
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
            builder.RegisterType<ConfigsLoader>().As<IConfigsLoader>();
            builder.Register(c =>
            {
                var configService = c.Resolve<IConfigsLoader>();
                return CreateParserService(parserType, configService);
            }).As<IParserService>();
            builder.Register(c =>
            {
                var configService = c.Resolve<IConfigsLoader>();
                var loggerService = c.Resolve<ILoggerService>();
                var parserservice = c.Resolve<IParserService>();
                return new ParserServiceController(configService, new StorageFactoryFactory(), parserservice, 
                    loggerService, source, destination);
            }).As<ParserServiceController>();
            return builder.Build();
        }

        private static IParserService CreateParserService(ParserType parserType, IConfigsLoader configService)
        {
            if (parserType.Equals(ParserType.MSRead))
            {
                var msReadConfig = configService.GetMSReadConfigModel();
                return new MSReadParserService(msReadConfig.CognitiveServiceEndPoint, msReadConfig.CongnitiveServiceKey);
            }
            else
            {
                throw new NotImplementedException();
            }
        }
    }
}
