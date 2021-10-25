# CogSLanguagueUtils

The Cognitive Services Language Utilities is a CLI tool that provides some core functionalities to make the experience of using some of the Cognitive Language Services simpler (mainly Custom Text).


## Installation

You can download the executable [here](https://github.com/microsoft/CogSLanguageUtilities/releases)
If you would like to use the tool system wide, you can add it to your PATH environment variable.
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

## Supported files
The following file formats are currently supported
- txt
- PDF
- Docx
- Scanned documents and Images (jpeg, bmp, png)

## Chunking Methods
The tool supports different types of chunking. All chunking methods respect paragraph endings so that chunks do not start or end mid-paragraph. If paragraph endings are not available for the document being parsed, sentences marked by '.' are used as the building block of a chunk.
- **Chunk by character limit**
- **Chunk by page**
- **Chunk by section** 

# Commands Overview

- config
    - load from file
        - clutils config load --path <ABSOLUTE_PATH>

- parsing
    - clutils parse --source <BLOB/LOCAL> --destination <BLOB/LOCAL> [ --chunk-type <PAGE/CHAR> ]

- chunking
    - clutils chunk --source <BLOB/LOCAL> --destination <BLOB/LOCAL>

- prediction
    - clutils predict --cognitive-service <customtext/textanalytics/both> --source <BLOB/LOCAL> --destination <BLOB/LOCAL> [ --chunk-type <PAGE/CHAR> ]

- evaluation
    - clutils evaluate --cognitive-service <customtext/textanalytics/both> --source <BLOB/LOCAL> --destination <BLOB/LOCAL>



[1]: ./CogSLanguageUtilities.ViewLayer.CliCommands/Commands/ParseCommand/README.md
[2]: ./CogSLanguageUtilities.ViewLayer.CliCommands/Commands/PredictCommand/README.md
[3]: ./CogSLanguageUtilities.ViewLayer.CliCommands/Commands/EvaluateCommand/README.md
[4]: ./CogSLanguageUtilities.ViewLayer.CliCommands/Commands/ChunkCommand/README.md
[5]: ./CogSLanguageUtilities.ViewLayer.CliCommands/Commands/ConfigCommand/README.md
