using FileFormatConverter.Core.Models;
using McMaster.Extensions.CommandLineUtils;
using System.ComponentModel.DataAnnotations;

namespace Microsoft.CognitiveSearchIntegration.Cli.Commands
{
    [Command("convert", Description = "")]
    public class ConvertFileCommand
    {
        [Option("-sp", Description = "source file path")]
        [Required]
        public string SourcePath { get; }

        [Option("-st", Description = "source file type")]
        [Required]
        public FileType SourceType { get; }

        [Option("-tp", Description = "target file path")]
        public string TargetPath { get; }

        [Option("-tt", Description = "target file type")]
        [Required]
        public FileType TargetType { get; }

        public void OnExecute(CommandLineApplication app)
        {
            var inputFilePath = @"C:\Users\mshaban\Desktop\cli tool\file samples\source-labeled_datapoints.jsonl";
            var targetFilePath = @"C:\Users\mshaban\Desktop\cli tool\file samples\test.json";
            //FileConversionOrchestrator.ConvertModelFile(inputFilePath, targetFilePath);
        }
    }
}
