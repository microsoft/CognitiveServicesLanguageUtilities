# Chunk Command

The chunk command allows you to chunk your text files to smaller parts by specifying a character limit.

The chunker respects sentences separated by '.' so that a chunk does not end mid sentence.

    Usage: clutils chunk [options]

    Options:
      --source <local/blob>       [required] indicates source storage type
      --destination <local/blob>  [required] indicates destination storage type
      -?|-h|--help                Show help information

## How To Use

To run the chunk command you need to follow these steps

1. Prepare your documents
    - upload the documents you want to chunk to some blob container or in a directory on your local machine
1. update configs file
    - specify source storage
    - specify default chunking limit
    - specify output storage
1. Run the chunk command


## Configure Services

Define configurations using the config command before using the chunk command

1. Storage
    - The source storage which contains the text files you want to chunk
    - The destination storage which will store the chunked files
    - It can be a Local Disk directory **OR** Blob Storage container
    - Configuration
        - Local Disk: Define absolute path of local disk directory to read from
        - Blob Storage: define ConnectionString, EndpointUrl, and Container to read from
1. Chunking
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
  "chunker": {
    "char-limit": 5000
  }
}
```
## Chunking Pipeline
Running the command goes through the following steps:
1. Read documents
    - Reads the text files from the selected source storage
1. Chunking
    - Chunk the document into smaller parts according to the specified character limits without splitting sentences.
1. Store Result
    - Each of the chunks is stored in a separate text file with the original file name and the chunk number separated by '\_' as follows {originalFileName}_{chunkNumber}.txt
    