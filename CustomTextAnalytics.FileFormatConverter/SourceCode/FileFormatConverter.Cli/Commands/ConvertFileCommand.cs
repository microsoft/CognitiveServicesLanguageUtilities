using FileFormatConverter.Runner;
using FileFormatConverter.Runner.DataStructures;
using McMaster.Extensions.CommandLineUtils;
using System.ComponentModel.DataAnnotations;

namespace Microsoft.FileFormatConverter.Cli.Commands
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

        [Option("-l", Description = "language")]
        public string Language { get; } = "en-US";

        public void OnExecute(CommandLineApplication app)
        {
            FileConversionOperationRunner.RunOperation(SourcePath, SourceType, TargetPath, TargetType, Language);
        }
    }
}
