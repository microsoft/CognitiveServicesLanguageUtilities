# File Format Converter

This cli tool is meant to convert file formats from Azure ML labeling formats - such as Jsonl, Conll, and TSV - to Custom Text labeling file formats - such as custom_entities.json and custom_classifiers.json - and vice versa.

##  Supported File Conversions
* AML `Jsonl` entities -> CT `Json` entities
* AML `CoNLL` entities -> CT `Json` entities

##  How to use

- First download [this package](FileConverter.rar) and unzip to extract the cli tool.
- Second, open your terminal/cmd/powershell in same dir as the exe file.
<br>(Or add that dir to your PATH env variable to run from anywhere)
- Then, run the following command:

```console
    convert -sp <source_file_path> -st <source_file_type> -tp [optional]<target_file_path> -tt <target_file_type> -l [optional] <language>
```

Example

```console
    convert -sp "./entities.conll" -st "jsonl" -tp "./target.json" -tt "ct_entities"
```

### File types as arguments
* AML Jsonl entities &ensp; &ensp; -> jsonl
* AML CONLL entities &ensp; -> conll
* CT Json entities &ensp; &ensp; &ensp; &ensp;-> ct_entities

### Notes
1. Location Parameter
    * when converting from AML-JSONL format to CT-Entities Format, our tool trims each document location
    * example
        * AML-JSONL document
        ```json
        "image_url": "AmlDatastore://textcontainer/conll_2003_ner/file1513.txt"
        ``` 
        * CT-Entities document
        ```json
        "location": "file1513.txt"
        ```
    * rationale: AML supports multiple storage options, what we did previously was to take the location as is from jsonl and use it. Most of these locations will not be accessible if used as is (those not in storage account), suggestion is to only take the file name and use it in the `location` field. We wanted to note this out for users to upload the files to their container.