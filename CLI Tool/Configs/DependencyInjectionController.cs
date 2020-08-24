using Autofac;
using CustomTextCliUtils.ApplicationLayer.Factories.Storage;
using CustomTextCliUtils.Configs.Consts;
using CustomTextCliUtils.ApplicationLayer.Controllers;
using CustomTextCliUtils.ApplicationLayer.Services.Logger;
using CustomTextCliUtils.ApplicationLayer.Services.Parser;
using CustomTextCliUtils.ApplicationLayer.Services.Storage;
using System;
using CustomTextCliUtils.ApplicationLayer.Services.Chunker;
using CustomTextCliUtils.ApplicationLayer.Services.Prediction;
using CustomTextCliUtils.ApplicationLayer.Modeling.Enums.Misc;

namespace CustomTextCliUtils.Configs
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

        public static IContainer BuildParseCommandDependencies(ParserType parserType)
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
                var chunkerService = CreateChunkerService(parserType);
                return new ParserServiceController(configService, new StorageFactoryFactory(), parserservice, 
                    loggerService, chunkerService);
            }).As<ParserServiceController>();
            return builder.Build();
        }

        public static IContainer BuildPredictCommandDependencies(ParserType parserType)
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
                var predictionConfigs = configService.GetPredictionConfigModel();
                return new CustomTextPredictionService(predictionConfigs.CustomTextKey, predictionConfigs.EndpointUrl,
                    predictionConfigs.AppId);
            }).As<IPredictionService>();
            builder.Register(c =>
            {
                var configService = c.Resolve<IConfigsLoader>();
                var loggerService = c.Resolve<ILoggerService>();
                var parserservice = c.Resolve<IParserService>();
                var chunkerService = CreateChunkerService(parserType);
                var predictionService = c.Resolve<IPredictionService>();
                return new PredictionServiceController(configService, new StorageFactoryFactory(), parserservice,
                    loggerService, chunkerService, predictionService);
            }).As<PredictionServiceController>();
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

        private static IChunkerService CreateChunkerService(ParserType parserType)
        {
            if (parserType.Equals(ParserType.MSRead))
            {
                return new MsReadChunkerService();
            }
            else
            {
                throw new NotImplementedException();
            }
        }
    }
}
