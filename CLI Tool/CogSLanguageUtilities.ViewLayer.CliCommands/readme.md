# Overview

This cli tool provides a set of utils for Custom Text users.
Like extracting text from documents, chunking text files, and integrating with prediction endpoint for their Custom Text app
<br/><br/>



# Commands
<!-- commands -->
- help
    - clutils --help

- config
    - load from file
        - clutils config load --path <ABSOLUTE_PATH>
    - msread
        - clutils config show msread
        - clutils config set msread --azure-resource-key <AZURE_RESOURCE_KEY> --cognitive-services-endpoint <ENDPOINT_URL>
    - tika (Not Supported Yet)
        - clutils config show tika
        - clutils config set tika --enable-ocr <BOOLEAN> --detect-titlted-text <BOOLEAN> --sort-by-position <BOOLEAN>
    - storage
        - clutils config show storage
        - clutils config set storage local --source-dir <ABSOLUTE_PATH> --destination-dir <ABSOLUTE_PATH> 
        - clutils config set storage blob --connection-string <CONNECTION_STRING> --source-container <CONTAINER_NAME> --destination-container <CONTAINER_NAME> 
    - chunker
        - clutils config show chunker
        - clutils config set chunker --char-limit <NUMBER>
    - prediction
        - clutils config show prediction
        - clutils config set prediction --azure-resource-endpoint <ENDPOINT_URL> --azure-resource-key <AZURE_RESOURCE_KEY> --app-id <APP_ID>
    - textanalytics
        - clutils config show textanalytics
        - clutils config set textanalytics --azure-resource-key <AZURE_RESOURCE_KEY> --azure-resource-endpoint <ENDPOINT_URL> --default-language <LANGUAGE> --sentiment <BOOLEAN> --ner <BOOLEAN> --keyphrase <BOOLEAN>

- parsing
    - clutils parse --parser <MSREAD/TIKA> --source <BLOB/LOCAL> --destination <BLOB/LOCAL> [ --chunk-type <PAGE/CHAR> ]

- chunker
    - clutils chunk --source <BLOB/LOCAL> --destination <BLOB/LOCAL>

- prediction
    - clutils predict --cognitive-service <customtext/textanalytics/both> --parser <MSREAD/TIKA> --source <BLOB/LOCAL> --destination <BLOB/LOCAL> [ --chunk-type <PAGE/CHAR> ]
