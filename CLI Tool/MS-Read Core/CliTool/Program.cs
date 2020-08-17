using CliTool.CommandControllers;
using CliTool.Configs.Constants;
using CliTool.Exceptions.Commands;
namespace CliTool
{
    class Program
    {
        static void Main(string[] args)
        {
            // obtain command
            var commandName = args[0].Split(":")[0];

            // select proper controller
            ICommandController commandController;
            if (commandName.Equals(Constants.CommandNames.ConfigCommand))
            {
                commandController = new ParseCommandController();
            }
            else if (commandName.Equals(Constants.CommandNames.ParseCommand))
            {
                commandController = new ParseCommandController();
            }
            else {
                throw new CommandNotFoundException(commandName);
            }

            // execute command
            commandController.Execute(args);
        }
    }
}
