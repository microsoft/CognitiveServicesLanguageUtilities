// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
﻿using Autofac;
using Microsoft.CogSLanguageUtilities.Core.Controllers;
using Microsoft.CogSLanguageUtilities.Core.Factories.Parser;
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
using Microsoft.CogSLanguageUtilities.Definitions.APIs.Factories.Parser;
using Microsoft.CogSLanguageUtilities.Definitions.APIs.Factories.Storage;
using Microsoft.CogSLanguageUtilities.Definitions.APIs.Services;
using Microsoft.CogSLanguageUtilities.Definitions.Configs.Consts;

namespace Microsoft.CustomTextCliUtils.Configs
{
    public class DependencyInjectionController
    {
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
            builder.RegisterType<StorageFactoryFactory>().As<IStorageFactoryFactory>();
            builder.RegisterType<ChunkerService>().As<IChunkerService>();
            builder.RegisterType<PlainTextParserService>().As<IParserService>();
            builder.RegisterType<ChunkerController>();
            return builder.Build();
        }

        public static IContainer BuildParseCommandDependencies()
        {
            var builder = BuildCommonDependencies();
            builder.Register(c =>
            {
                var configService = c.Resolve<IConfigsLoader>();
                return new ParserPoolManager(configService.GetParserConfigModel());
            }).As<IParserPoolManager>();
            builder.RegisterType<StorageFactoryFactory>().As<IStorageFactoryFactory>();
            builder.RegisterType<ChunkerService>().As<IChunkerService>();
            builder.RegisterType<ParserController>();
            return builder.Build();
        }

        public static IContainer BuildPredictCommandDependencies()
        {
            var builder = BuildCommonDependencies();
            builder.RegisterType<StorageFactoryFactory>().As<IStorageFactoryFactory>();
            builder.RegisterType<ChunkerService>().As<IChunkerService>();
            builder.RegisterType<ConcatenationService>().As<IConcatenationService>();
            builder.Register(c =>
            {
                var configService = c.Resolve<IConfigsLoader>();
                return new ParserPoolManager(configService.GetParserConfigModel());
            }).As<IParserPoolManager>();
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

        private static ContainerBuilder BuildCommonDependencies()
        {
            var builder = new ContainerBuilder();
            builder.RegisterInstance(new ConsoleLoggerService()).As<ILoggerService>();
            builder.RegisterType<ConfigsLoader>().As<IConfigsLoader>();
            return builder;
        }
    }
}
