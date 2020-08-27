# Overview

This cli tool provides a set of utils for Custom Text users.
Like extracting text from documents, chunking text files, and integrating with prediction endpoint for their Custom Text app
<br/><br/>



# Commands
<!-- commands -->
- help
    - ctcu --help

- config
    - msread
        - ctcu config show msread
        - ctcu config set msread --cognitive-services-key <AZURE_RESOURCE_KEY> --cognitive-services-endpoint <ENDPOINT_URL>
    - tika (Not Supported Yet)
        - ctcu config show tika
        - ctcu config set tika --enable-ocr <BOOLEAN> --detect-titlted-text <BOOLEAN> --sort-by-position <BOOLEAN>
    - storage
        - ctcu config show storage
        - ctcu config set storage local --source-dir <ABSOLUTE_PATH> --destination-dir <ABSOLUTE_PATH> 
        - ctcu config set storage blob --connection-string <CONNECTION_STRING> --source-container <CONTAINER_NAME> --destination-container <CONTAINER_NAME> 
    - chunker
        - ctcu config show chunker
        - ctcu config set chunker --char-limit <NUMBER>
    - prediction
        - ctcu config show prediction
        - ctcu config set prediction --customtext-endpoint <ENDPOINT_URL> --customtext-key <APIM_SUBSCRIPTION_ID> --app-id <APP_ID>

- parsing
    - ctcu parse --parser <MSREAD/TIKA> --source <BLOB/LOCAL> --destination <BLOB/LOCAL> [ --chunking-type <PAGE/CHAR> ]

- prediction
    - ctcu predict --parser <MSREAD/TIKA> --source <BLOB/LOCAL> --file-name <FILENAME> [ --chunking-type <PAGE/CHAR> ]

- chunker
    - ctcu chunk --source <BLOB/LOCAL> --destination <BLOB/LOCAL>
