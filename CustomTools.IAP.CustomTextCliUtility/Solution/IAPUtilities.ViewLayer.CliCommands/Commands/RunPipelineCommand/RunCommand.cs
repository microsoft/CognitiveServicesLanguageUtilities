// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
using Autofac;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.IAPUtilities.Definitions.APIs.Controllers;
using Microsoft.IAPUtilities.ViewLayer.CliCommands.Configs;
using System.Threading.Tasks;

namespace Microsoft.IAPUtilities.ViewLayer.CliCommands.Commands.RunPipelineCommand
{
    [Command("run", Description = "Run IAP pipeline")]
    public class RunCommand
    {
        private async Task OnExecute(CommandLineApplication app)
        {
            // build dependencies
            var container = DependencyInjectionController.BuildIAPControllerDependencies();

            // run program
            using (var scope = container.BeginLifetimeScope())
            {
                var controller = scope.Resolve<IIAPProccessController>();
                await controller.Run();
            }
        }
    }
}
