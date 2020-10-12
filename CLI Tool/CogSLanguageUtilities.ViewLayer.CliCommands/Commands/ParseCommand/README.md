# Parse Command

The parse command allows you to extract text from different document formats, chunk documents and upload them to your blob storage to be consumed by Custom Text.


    Usage: 
        clutils parse [options]

    Options:
        --source <local/blob>       [required] source storage type (which contains the documents to be parsed)
        --destination <local/blob>  [required] destination storage type (which will hold the parsing result documents)
        --chunk-type <page/char>    [optional] chunking type. if not set, no chunking will be used
        -?|-h|--help                Show help information

## How To Use

To run the parse command you need to follow these steps

1. Prepare your storage
    1. Move all the documents you need to parse to a single directory or blob container
    1. Create a directory or container to store the converted documents
1. Specify the required configs as shown below
1. Run the parse command

## Configure Services
Define configurations using the config command before using the parse command
1. Storage
    - The source storage which contains the documents you want to parse
    - The destination storage which will store the converted files
    - It can be a Local Disk directory **OR** Blob Storage container
    - Configuration
        - Local Disk: Define absolute path of local disk directory to read from
        - Blob Storage: define ConnectionString, EndpointUrl, and Container to read from
2. Parsing
    - Currently, we only support parsing the following document formats
        - Pdf, Docx, Images (Jpeg, Bmp, Png)
    - For Pdf and image documents, you need to configure a [Microsoft Read](https://docs.microsoft.com/en-us/azure/cognitive-services/computer-vision/concept-recognizing-text) azure resource
    - But as for Docx, you don't need any configuration
    - Configuration
        - Microsoft Read: EndpointUrl, and ResourceKey
3. Chunking
    - You can define the character limit (default is 5000)

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
  }
}
```
## Parsing Pipeline
Running the command goes through the following steps:
1. Read documents
    - Reads the documents from the selected source storage
2. Parse the documents
    - Extracting text from documents
    - Using [MsRead](https://docs.microsoft.com/en-us/azure/cognitive-services/computer-vision/concept-recognizing-text) (for PDF and images) and [OpenXml SDK](https://www.nuget.org/packages/Open-XML-SDK/) (for Docx).
3. Chunking
    - Chunk the document into smaller parts according to the specified chunking type.
4. Store Result
    - Store result text files in selected destination storage.
    - Each of the converted chunks is stored in a separate text file with the original file name and the chunk number separated by '\_' as follows {originalFileName}_{chunkNumber}.txt
