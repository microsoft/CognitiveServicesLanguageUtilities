# CogSLanguagueUtils

The Cognitive Services Language Utilities is a CLI tool that provides some core functionalities to make the experience of Cognitive Language Services simpler (mainly Custom Text).


## Installation

CLUtils is based on dotnet core and an open source fork of Microsoft's [CommandLineUtils](https://github.com/natemcmaster/CommandLineUtils) for command line parsing.
You need to download the executable and add it your PATH environment variable.
Run the tool to check your installation.

    $ clutils

## Available Commands

The following commands are currently available:
* [Parse][1]
* [Predict][2]
* [Evaluate][3]
* [Chunk][4]
* [Config][5]

## Overview
The tool currently supports three main use cases
### Document Parsing
- In order to make the app training experience easier, we added the "parse" command. The reason being is that Text Analytics and Custom Text only accept plain text input.
- The "parse" command allows you to extract plain text from your documents of any supported extension. 
- It also allows you to chunk your document into smaller segments using different methods. This is to be compliant with the services' character limit as well as allow you to be in control of how your document is broken down to smaller parts.
- The tool also uploads the converted text files to your blob storage which is a pre-requisite for training your Custom Text Application.

### Prediction
- In order to make the prediction pipline easier, we added the 'predict' command. The reason being is that the user must submit the document as plain text, as well as abide by the char-limit set by the service (for example: Custom Text only accepts text documents of up to 25k tokens).
- So the "predict" command integrates this pipeline (of parsing, chunking, calling prediction apis, aggregating different chunks of the same document) into a single command.

### Model Evaluation
- In order for users to test app performance, we added support for the "evaluate" command. The reason being is that Text Analytics and Custom Text currently do not provide any means for testing application performance.
- So we integrated the testing pipeline (reading labeled examples, calling prediction apis, evaluating model performance, ..) into this simple command.


# Commands
<!-- commands -->
- help
    - clutils --help

- config
    - load from file
        - clutils config load --path <ABSOLUTE_PATH>
    - msread
        - clutils config show parser msread
        - clutils config set parser msread --azure-resource-key <AZURE_RESOURCE_KEY> --cognitive-services-endpoint <ENDPOINT_URL>
    - storage
        - clutils config show storage
        - clutils config set storage local --source-dir <ABSOLUTE_PATH> --destination-dir <ABSOLUTE_PATH> 
        - clutils config set storage blob --connection-string <CONNECTION_STRING> --source-container <CONTAINER_NAME> --destination-container <CONTAINER_NAME> 
    - chunker
        - clutils config show chunker
        - clutils config set chunker --char-limit <NUMBER>
    - customtext
        - clutils config show customtext prediction
        - clutils config set customtext prediction --azure-resource-endpoint <ENDPOINT_URL> --azure-resource-key <AZURE_RESOURCE_KEY> --app-id <APP_ID>
        - clutils config set customtext authoring --azure-resource-endpoint <ENDPOINT_URL> --azure-resource-key <AZURE_RESOURCE_KEY> --app-id <APP_ID>
    - textanalytics
        - clutils config show textanalytics
        - clutils config set textanalytics --azure-resource-key <AZURE_RESOURCE_KEY> --azure-resource-endpoint <ENDPOINT_URL> --default-language <LANGUAGE> --sentiment <BOOLEAN> --ner <BOOLEAN> --keyphrase <BOOLEAN>

- parsing
    - clutils parse --source <BLOB/LOCAL> --destination <BLOB/LOCAL> [ --chunk-type <PAGE/CHAR> ]

- chunker
    - clutils chunk --source <BLOB/LOCAL> --destination <BLOB/LOCAL>

- prediction
    - clutils predict --cognitive-service <customtext/textanalytics/both> --source <BLOB/LOCAL> --destination <BLOB/LOCAL> [ --chunk-type <PAGE/CHAR> ]

- evaluation
    - clutils evaluate --cognitive-service <customtext/textanalytics/both> --source <BLOB/LOCAL> --destination <BLOB/LOCAL>


[1]: ./CLI%20Tool/CogSLanguageUtilities.ViewLayer.CliCommands/Commands/ParseCommand/README.md
[2]: ./CLI%20Tool/CogSLanguageUtilities.ViewLayer.CliCommands/Commands/PredictCommand/README.md
[3]: ./CLI%20Tool/CogSLanguageUtilities.ViewLayer.CliCommands/Commands/EvaluateCommand/README.md
[4]: ./CLI%20Tool/CogSLanguageUtilities.ViewLayer.CliCommands/Commands/ChunkCommand/README.md
[5]: ./CLI%20Tool/CogSLanguageUtilities.ViewLayer.CliCommands/Commands/ConfigCommand/README.md
