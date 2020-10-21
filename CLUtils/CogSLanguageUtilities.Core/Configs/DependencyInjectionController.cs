// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
﻿using Autofac;
using Microsoft.CogSLanguageUtilities.Core.Controllers;
using Microsoft.CogSLanguageUtilities.Core.Factories.Storage;
using Microsoft.CogSLanguageUtilities.Core.Helpers.HttpHandler;
using Microsoft.CogSLanguageUtilities.Core.Services.Chunker;
using Microsoft.CogSLanguageUtilities.Core.Services.Concatenation;
using Microsoft.CogSLanguageUtilities.Core.Services.CustomText;
using Microsoft.CogSLanguageUtilities.Core.Services.Evaluation;
using Microsoft.CogSLanguageUtilities.Core.Services.Logger;
using Microsoft.CogSLanguageUtilities.Core.Services.Parser;
using Microsoft.CogSLanguageUtilities.Core.Services.Storage;
using Microsoft.CogSLanguageUtilities.Core.Services.TextAnalytics;
using Microsoft.CogSLanguageUtilities.Definitions.APIs.Configs;
using Microsoft.CogSLanguageUtilities.Definitions.APIs.Factories.Storage;
using Microsoft.CogSLanguageUtilities.Definitions.APIs.Services;
using Microsoft.CogSLanguageUtilities.Definitions.Configs.Consts;
using Microsoft.CogSLanguageUtilities.Definitions.Models.Enums.Parser;
using System;

namespace Microsoft.CustomTextCliUtils.Configs
{
    public class DependencyInjectionController
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
            builder.RegisterType<ConfigsController>();
            return builder.Build();
        }

        public static IContainer BuildChunkerCommandDependencies()
        {
            var builder = BuildCommonDependencies();
            builder.RegisterType<ConfigsLoader>().As<IConfigsLoader>();
            builder.RegisterType<StorageFactoryFactory>().As<IStorageFactoryFactory>();
            builder.RegisterType<ChunkerService>().As<IChunkerService>();
            builder.RegisterType<ChunkerController>();
            builder.RegisterType<PlainTextParserService>().As<IParserService>();
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
                return new ParserController(configService, new StorageFactoryFactory(), parserservice,
                    loggerService, chunkerService);
            }).As<ParserController>();
            return builder.Build();
        }

        public static IContainer BuildPredictCommandDependencies(ParserType parserType)
        {
            var builder = BuildCommonDependencies();
            builder.RegisterType<ConfigsLoader>().As<IConfigsLoader>();
            builder.RegisterType<StorageFactoryFactory>().As<IStorageFactoryFactory>();
            builder.RegisterInstance<IChunkerService>(CreateChunkerService(parserType));
            builder.RegisterType<ConcatenationService>().As<IConcatenationService>();
            builder.Register(c =>
            {
                var configService = c.Resolve<IConfigsLoader>();
                return CreateParserService(parserType, configService);
            }).As<IParserService>();
            builder.Register(c =>
            {
                var configService = c.Resolve<IConfigsLoader>();
                var predictionConfigs = configService.GetCustomTextPredictionConfigModel();
                return new CustomTextPredictionService(new HttpHandler(), predictionConfigs.AzureResourceKey, predictionConfigs.AzureResourceEndpoint,
                    predictionConfigs.AppId);
            }).As<ICustomTextPredictionService>();
            builder.Register(c =>
            {
                var configService = c.Resolve<IConfigsLoader>();
                return new TextAnalyticsService(
                    configService.GetTextAnalyticsConfigModel().AzureResourceKey,
                    configService.GetTextAnalyticsConfigModel().AzureResourceEndpoint,
                    configService.GetTextAnalyticsConfigModel().DefaultLanguage);
            }).As<ITextAnalyticsService>();
            builder.RegisterType<PredictionController>();
            return builder.Build();
        }


        public static IContainer BuildEvaluateCommandDependencies()
        {
            var builder = BuildCommonDependencies();
            builder.RegisterType<ConfigsLoader>().As<IConfigsLoader>();
            builder.RegisterType<StorageFactoryFactory>().As<IStorageFactoryFactory>();
            builder.RegisterType<BatchTestingService>().As<IBatchTestingService>();
            builder.Register(c =>
            {
                var configService = c.Resolve<IConfigsLoader>();
                var predictionConfigs = configService.GetCustomTextPredictionConfigModel();
                return new CustomTextPredictionService(new HttpHandler(), predictionConfigs.AzureResourceKey, predictionConfigs.AzureResourceEndpoint,
                    predictionConfigs.AppId);
            }).As<ICustomTextPredictionService>();
            builder.Register(c =>
            {
                var configService = c.Resolve<IConfigsLoader>();
                var labeledExamplesConfigs = configService.GetCustomTextAuthoringConfigModel();
                return new CustomTextAuthoringService(new HttpHandler(), labeledExamplesConfigs.AzureResourceKey, labeledExamplesConfigs.AzureResourceEndpoint,
                    labeledExamplesConfigs.AppId);
            }).As<ICustomTextAuthoringService>();
            builder.RegisterType<BatchTestingController>();
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
                return new ChunkerService();
            }
            else
            {
                throw new NotImplementedException();
            }
        }
    }
}
