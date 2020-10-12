# Predict Command

The predict command allows you to run the prediction API on your documents of different extensions using the same parsing and chunking pipeline used by the parse command.

    Usage: 
        clutils predict [options]

    Options:
        --cognitive-service <customtext/textanalytics/both>  [required] indicates which cognitive service to use for prediction
        --source <local/blob>                                [required] indicates source storage type
        --destination <local/blob>                           [required] indicates destination storage type
        --chunk-type <page/char>                             [optional] indicates chunking type. if not set, no chunking will be used
        -?|-h|--help                                         Show help information

## Configure Services
Define configurations using the config command before using the predict command
1. Source Storage
    - The storage service which contains the documents you want to parse
    - It can be a Local Disk directory **OR** Blob Storage container
    - Configuration
        - Local Disk: Define absolute path of local disk directory to read from
        - Blob Storage: define ConnectionString, EndpointUrl, and Container to read from
2. Parsing
    - Currently, we only support parsing the following document formats
        - PDF, Docx, Images (jpeg, bmp, png)
    - For PDF and image documents, you need to configure a [Microsoft Read](https://docs.microsoft.com/en-us/azure/cognitive-services/computer-vision/concept-recognizing-text) azure resource
    - But as for Docx, you don't need any configuration
    - Configuration
        - Microsoft Read: EndpointUrl and ResourceKey
3. Chunking
    - You can define the character limit
4. Prediction
    - You can choose either CustomText or TextAnalytics or both to run predictions on your document
    - For Text Analytics Configuration:
        - Endpoint Url
        - Resource Key
        - Default Operations to be performed (NER, Keyphrase, Sentiment)
    - Custom Text Configuration:
        - Prediction Endpoint Url
        - Prediction Resource Key

## Prediction Pipeline
Running the command goes through the following steps:
1. Read documents
    - Reads the documents from the selected source storage
1. Parse the documents
    - Extracting text from documents
    - Using [MsRead](https://docs.microsoft.com/en-us/azure/cognitive-services/computer-vision/concept-recognizing-text) (for PDF and images) and [OpenXml SDK](https://www.nuget.org/packages/Open-XML-SDK/) (for Docx).
1. Chunking
    - Chunk the document into smaller parts according to the specified chunking type.
1. Prediction
    - Call the prediction API for the specified service for each of the chunks in every file
1. Concatenation 
    - Concatenate the prediction results of the chunks of a certain file in one object
1. Store Result
    - Store the prediction result to the specified storage
    - For each input document a json file is stored contatining the concatenated result of the individual chunks


## Output Format

The output json file contains the prediction result for each of the chunks as an array. The json file also contains metadata for each chunk.

```
[
  {
    "ChunkInfo": {
      "ChunkNumber": 1,
      "CharCount": 4536,
      "StartPage": 1,
      "EndPage": 1,
      "Summary": "FirstWord ... LastWord"
    },
    "CustomTextResponse": {
      "Prediction": {
        "positiveClassifiers": [],
        "classifiers": {...},
        "extractors": {}
      }
    },
    "SentimentResponse": {
      "DocumentSentiment": {
        "Sentences": [...],
        "Warnings": []
      },
      "Id": "0",
      "Statistics": {...},
      "Error": {...},
      "HasError": false
    },
    "NerResponse": {
      "Entities": [...],
      "Id": "0",
      "Statistics": {...},
      "Error": {...},
      "HasError": false
    },
    "KeyphraseResponse": {
      "KeyPhrases": [...],
      "Id": "0",
      "Statistics": {...},
      "Error": {...},
      "HasError": false
    }
  }
]
```

You can refer to the individual services documentation for more info on their responses.
- [Text Analytics Documentation](https://docs.microsoft.com/en-us/azure/cognitive-services/text-analytics/)