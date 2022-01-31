using FileFormatConverter.Orchestrators;
using McMaster.Extensions.CommandLineUtils;
using System.ComponentModel.DataAnnotations;

namespace Microsoft.CognitiveSearchIntegration.Cli.Commands
{
    [Command("convert", Description = "")]
    public class ConvertFileCommand
    {
        [Option("--source <absolute_path>", Description = "source file path")]
        [Required]
        public string SourcePath { get; }
        [Option("--target <absolute_path>", Description = "target file path")]
        public string TargetPath { get; }

        public void OnExecute(CommandLineApplication app)
        {
            var inputFilePath = @"C:\Users\mshaban\Desktop\cli tool\file samples\source-labeled_datapoints.jsonl";
            var targetFilePath = @"C:\Users\mshaban\Desktop\cli tool\file samples\test.json";
            FileConversionOrchestrator.ConvertModelFile(inputFilePath, targetFilePath);
        }
    }
}
