﻿using Autofac;
using CustomTextCliUtils.Configs;
using CustomTextCliUtils.ApplicationLayer.Controllers;
using McMaster.Extensions.CommandLineUtils;

namespace CustomTextCliUtils.CommandsLayer.ConfigCommand.Show
{
    [Command("storage", Description = "shows configs for all storage services")]
    [Subcommand(
        typeof(ConfigShowLocalCommand),
        typeof(ConfigShowBlobCommand))]
    class ConfigShowStorageCommand
    {
        private int OnExecute(CommandLineApplication app)
        {
            // build dependencies
            var container = DependencyInjectionController.BuildConfigCommandDependencies();

            // run program
            using (var scope = container.BeginLifetimeScope())
            {
                var controller = scope.Resolve<ConfigServiceController>();
                controller.ShowStorageConfigs();
            }
            return 1;
        }
    }
}