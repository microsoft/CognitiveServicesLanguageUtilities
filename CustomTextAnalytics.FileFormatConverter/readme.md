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
    convert -sp <source_file_path> -st <source_file_type> -tp [optional]<target_file_path> -tt <target_file_type>
```

Example

```console
    convert -sp "./entities.conll" -st "jsonl" -tp "./target.json" -tt "ct_entities"
```

### File types as arguments
* AML Jsonl entities &ensp; &ensp; -> jsonl
* AML CONLL entities &ensp; -> conll
* CT Json entities &ensp; &ensp; &ensp; &ensp;-> ct_entities