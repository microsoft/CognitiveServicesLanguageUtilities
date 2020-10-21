# Config Command
The config command allows you to set the configurations required for running the tool by loading them from a file or through the command line. You can also view the current configuration in your terminal.

    Usage: 
        clutils config [command] [options]

    Commands:
        load          loads app configs from file
        set           sets app configs
        show          shows app configs

    Options:
        -?|-h|--help  Show help information

## Config Load

The config load command allows to you to load your configs from a file. This helps you maintain several config files and load the one you need before using the tool.

    Usage: 
        clutils config load [options]

    Options:
    --path <absolute_path>  absolute path to configs file
    -?|-h|--help            Show help information

The config file is a json file with the following structure
```
{
  "storage": {
    "blob": {
      "connection-string": "DefaultEndpointsProtocol=https;AccountName=***;AccountKey=***;EndpointSuffix=core.windows.net",
      "source-container": "***",
      "destination-container": "***"
    },
    "local": {
      "source-dir": "***",
      "destination-dir": "***"
    }
  },
  "parser": {
    "msread": {
      "azure-resource-endpoint": "https://eastus.api.cognitive.microsoft.com/",
      "azure-resource-key": "***"
    }
  },
  "chunker": {
    "char-limit": 5000
  },
  "customText": {
    "authoring": {
      "azure-resource-key": "***",
      "azure-resource-endpoint": "https://***.cognitiveservices.azure.com/",
      "app-id": "***"
    },
    "prediction": {
      "azure-resource-key": "***",
      "azure-resource-endpoint": "https://***.cognitiveservices.azure.com",
      "app-id": "***"
    }
  },
  "textanalytics": {
    "azure-resource-endpoint": "https://***.cognitiveservices.azure.com",
    "azure-resource-key": "***",
    "default-language": "en",
    "default_operations": {
      "sentiment": true,
      "ner": false,
      "keyphrase": true
    }
  }
}
```
## Config Show

The config show command allows you to view the current configurations in the terminal. You can view all configurations or a certain section by using the subcommands.

    Usage: 
        clutils config show [command] [options]

    Commands:
        chunker        shows configs for chunker
        customtext     shows configs for Custom Text
        parser         shows configs for all parsers
        storage        shows configs for all storage services
        textanalytics  shows configs for text analytics
    
    Options:
        -?|-h|--help   Show help information

## Config Set

The config set command allows you to set specific configs through the command line without reloading all configs from a file.
The command has the following syntax.

    Usage: 
        clutils config set [command] [options]

    Commands:
        chunker        sets configs for chunker
        customtext     sets configs for Custom Text
        parser         sets configs for all parsers
        storage        sets configs for all storage services
        textanalytics  sets configs for textanalytics

    Options:
        -?|-h|--help   Show help information

The configurations for each section are set using the options for their respective subcommand.

For example setting the configurations for local storage is done as follows:

    $ clutils config set storage local --source-dir /source --destination-dir /destination

The subcommands structure and the option names are the same as the config file. You can run the help command for any of the subcommands to see the configs you can set as shown below.

    $ clutils config set storage local --help
    sets configs for local storage

    Usage: 
        clutils config set storage local [options]

    Options:
        --source-dir <ABSOLUTE_PATH>       absolute path for source directory
        --destination-dir <ABSOLUTE_PATH>  absolute path for destination directory
        -?|-h|--help                       Show help information
