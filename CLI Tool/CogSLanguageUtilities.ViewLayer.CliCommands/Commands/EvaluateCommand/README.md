# Evaluate Command

The evaluate command allows you to run batch tests on your Custom Text application and get performance metrics for the predictions.

The calculations for the evaluation metrics are carried out by the Nuget found [here](https://)

    Usage: 
        clutils evaluate [options]

    Options:
        --source <local/blob>       [required] indicates source storage type
        --destination <local/blob>  [optional] indicates destination storage type, defaults to blob
        -?|-h|--help                Show help information
        
## How To Use

The evaluate command allows you to test the performance of your application by running the prediction on unseen data and comparing the results to the labels you provide.
To add labeled data and run the evaluation command you need to follow these steps:
1. **Create Labeled Examples**
    1. Create a new Blob Container.
    1. Populate the container with unseen text documents (test data).
        - You can use the parse command and set the source storage to the original unseen documents and the destination storage to the newly created container. 
        - **NOTE:** You can't use the same container used for training because the labeled data needs to be unseen by the original trained application
    1. Create a new Custom Text App linked to the new container
        - This application will be used by the tool only for retrieving the labels so the app doesn't need to be trained
    1. Label your unseen data through the Custom Text web portal in your new application.
1. **Set Tool Configs**
    - In the tool configuration you need to specify both the authoring and prediction applications for custom text.
        1. Authoring application is the newly created application containing the unseed labeled data. this will only be used for retrieving labels.
        1. Prediction application is the trained application that will be used for prediction.
1. **Model Evaluation**
    - Run the "Evaluate" command



## Configure Services
Define configurations using the config command before using the evaluate command
1. **Labeled Data Source**
    - Currently we use Custom Text web portal to label our documents
    - Custom Text app stores labeling info for each document in the app itself with a reference to the actual document in the blob store
    - In order for the tool to read the labeled examples, you need to provide the following 
    - Configurations
        - Custom Text App (Authoring Resource):
            - This is authoring resource of your Custom Text app which contains the labeled examples
            - Endpoint Url
            - Resource Key
            - App Id
        - Source Documents (Blob Store):
            - This is blob storage container name which contains the referenced documents in the labels
            - Endpoint Url
            - Connection String
            - Container Name

1. **Prediction Service**
    - This is **Prediction Resource** of your trained Custom Text app which we will use to run predictions on the labeled examples
    - Configurations
        - Custom Text App Prediction Resource:
            - Endpoint Url
            - Resource Key
            - App Id

1. **Destination Storage**
    - This is the storage location we will use to store the output of the evaluation process
    - Configuration
        - Can either be Local Directory or Blob Container

```
{
  "storage": {
    "blob": {
      "connection-string": "DefaultEndpointsProtocol=https;AccountName=***;AccountKey=***;EndpointSuffix=core.windows.net",
      "source-container": "***",
      "destination-container": "***"
    }
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
  }
}
```

## Evaluation Pipeline
Running the command goes through the following steps:
1. Read labeled data
    - Reads labeled data from the test Custom Text App
1. Read the test data
    - Read the text files from the created test data container
1. Prediction
    - Call the prediction API using the original trained application for each of the test files
1. Evaluation
    - Compare the results received from the trained application predictions with the labeled data
1. Store Result
    - Store the prediction result for each of the test files in a folder named prediction_ouptut inside the destination directory/container
    - Write the evaluation result to a file named CustomTextBatchTesting.json in the destination directory/container


## Output Format

The output json file contains the metrics calculated to evaluate the performance of the application. These include precision, recall and fscore for each model in the application as well as some stats for each of the test files.

Nested entities are marked using the separator '::'. For example, if the entity 'Name' is a child of the entity 'Author' its name will be 'Author::Name'

The output json has the following structure

```
{
  "classificationModelsStats": [
    {
      "modelName": "Introduction",
      "modelType": "Intent Classifier",
      "precision": 1.0,
      "recall": 1.0,
      "fScore": 1.0
    }
  ],
  "entityModelsStats": [
    {
      "modelName": "Authors",
      "modelType": "Entity Extractor",
      "precision": 0.5,
      "recall": 0.22727272727272727,
      "fScore": 0.3125,
      "entityTextFScore": 0.0,
      "entityTypeFScore": 0.625
    },
    {
      "modelName": "Authors::Name",
      "modelType": "Child Entity Extractor",
      "precision": 0.5,
      "recall": 0.2857142857142857,
      "fScore": 0.36363636363636365,
      "entityTextFScore": 0.0,
      "entityTypeFScore": 0.7272727272727273
    },
    {
      "modelName": "Authors::Contact",
      "modelType": "Child Entity Extractor",
      "precision": 0.5,
      "recall": 0.28125,
      "fScore": 0.36,
      "entityTextFScore": 0.0,
      "entityTypeFScore": 0.72
    }
  ],
  "documentsStats": [
    {
      "text": "***",
      "predictedClassNames": [],
      "labeledClasstNames": [],
      "falsePositiveEntities": [
        {
          "entityName": "Authors",
          "startCharIndex": 2666,
          "endCharIndex": 2739
        },
        {
          "entityName": "Authors::Contact",
          "startCharIndex": 2666,
          "endCharIndex": 2739
        }
      ],
      "falseNegativeEntities": []
    },
    {
      "text": "***",
      "predictedClassNames": ["Introduction"],
      "labeledClasstNames": ["Introduction"],
      "falsePositiveEntities": [
        {
          "entityName": "Authors::Name",
          "startCharIndex": 60,
          "endCharIndex": 72
        },
        {
          "entityName": "Authors",
          "startCharIndex": 60,
          "endCharIndex": 72
        }
      ],
      "falseNegativeEntities": [
        {
          "entityName": "Authors",
          "startCharIndex": 60,
          "endCharIndex": 71
        },
        {
          "entityName": "Authors::Name",
          "startCharIndex": 60,
          "endCharIndex": 71
        }
      ]
    }
  ]
}
```