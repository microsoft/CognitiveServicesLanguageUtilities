# Language Model Evaluation

The Language Model Evaluation nuget allows you to evaluate the performance of your language models by calculating metrics like precision, recall and fscore for both the classes and entities as well as provide some document specific metrics.

## Credits
- This project is a nugetized version of the model evaluation code created in LUIS.ai
- All credits go to [Amr Saleh](https://github.com/AmrSaleh)

## Installation

- This module is published as nuget on nuget.org
- You can add this nuget to your project and use it directly

## How to use
Add the [nuget](https://) to your project
Then call this method with corresponding parameters to evaluate your model
```
BatchTestingController.EvaluateModel(
            IReadOnlyList<TestingExample> testData,
            bool verbose = false,
            IReadOnlyList<Model> entities = null,
            IReadOnlyList<Model> classes = null)
```
* testData: is a list of the test dataset (ground truth and prediction of each example)
* entites: list of application entity models
* classes: list of application classe models
* verbose: set to true if you would like the output to include the 'Text F-Score and Type F-Score'

### Input Models

```
public class TestingExample
    {
        public string Text { get; set; }
        public PredictionObject LabeledData { get; set; }
        public PredictionObject PredictedData { get; set; }
    }
```
This class represents each testing example in the test set
* Text: Is the document text
* LabeledData: Are the labels provided by the user for this test example
* PredictedData: Are the labels as predicted by the application
```
public class PredictionObject
    {
        public List<string> Classification { get; set; }
        public List<Entity> Entities { get; set; }
    }
```
This class represents the labels for each document
* Classification: list of classes for this document
* Entities: list of entities in this document
```
public class Entity
    {
        public int StartPosition { get; set; }

        public int EndPosition { get; set; }
        
        public string Name { get; set; }

        public List<Entity> Children { get; set; }
    }
```
This class represents each labeled entity in the document
* StartPosition: the index at which the entity starts in the document
* EndPosition: the index at which the entity ends in the document
* Name: name of the entity
* Children: list of child entities of this parent entity (only for hierarchical entities)

### Output Models
```
public class BatchTestResponse
    {
        public IReadOnlyList<ModelStats> ClassificationModelsStats { get; set; }

        public IReadOnlyList<ModelStats> EntityModelsStats { get; set; }

        public IReadOnlyList<QueryStats> QueryStats { get; set; }
    }
```
This is the class for the return value of the evaluate function
- ClassificationModelsStats: statistics about each classification 
- EntityModelsStats: statistics about each entity model
- QueryStats: statistics about each query
```
public class ModelStats
    {
        public string ModelName { get; set; }

        public string ModelType { get; set; }

        public double Precision { get; set; }

        public double Recall { get; set; }

        public double FScore { get; set; }

        public double? EntityTextFScore { get; set; }

        public double? EntityTypeFScore { get; set; }
    }
```
This class represents the result info/stats from evaluation process for each model (class or entity)
* ModelName: unique name used to identify the model. For nested entities the separator '::' is used to construct the hierarchical name (ex. Person::Address::Street)
* ModelType: model type (Entity Extractor, Classifier, ...)
* Precision: precision score
* Recall: recall score
* FScore: fscore for this model
* EntityTextFScore: (for entities only)
* EntityTypeFScore: (for entities only)
```
    public class QueryStats
    {
        public string QueryText { get; set; }

        public List<string> PredictedClassNames { get; set; }

        public List<string> LabeledClassNames { get; set; }

        public List<EntityNameAndLocation> FalsePositiveEntities { get; set; }

        public List<EntityNameAndLocation> FalseNegativeEntities { get; set; }
    }
```
This class represents info/stats for each input test example as returned by evaluation 
* QueryText: text for this test example
* PredictedClassNames: class names as predicted by application
* LabeledClassNames: class names as was labeled/provided in the test set
* FalsePositiveEntities: List of false positive entities predicted in this example
* FalseNegativeEntities: List of false negative entities predicted in this example

```
public class EntityNameAndLocation
    {
        public string EntityName { get; set; }

        public int StartPosition { get; set; }

        public int EndPosition { get; set; }
    }
```

## Metrics Calculation
### Classification
1. Calculate confusion matrix
    - For each testing example a classification is categorized as  TP, FN or RP as shown below 

    |                      | Expected Classification | Predicted Classification |
    | -------------------- | ----------------------- | ------------------------ |
    | Expected = Predicted | TP                      | -                        |
    | Expected != Predicted| FN                      | FP                       |

1. Calculate Precision


    ![\Large Precision=\frac{TP}{TP+FP}](https://latex.codecogs.com/svg.latex?\Large&space;Precision=\frac{TP}{TP+FP}) 

1. Calculate Recall


    ![\Large Recall=\frac{TP}{TP+FN}](https://latex.codecogs.com/svg.latex?\Large&space;Recall=\frac{TP}{TP+FN}) 

1. Calculate Fscore


    ![\Large F_1=\frac{2*Precision*Recall}{Precision+Recall}](https://latex.codecogs.com/svg.latex?\Large&space;F_1=\frac{2*Precision*Recall}{Precision+Recall}) 


### Entities
- Using MUC we calculate four metrics
    1. **Correct Type**: count of entities that intersect in location and have same type
    1. **Correct Text**: count of entities with exact location match
    1. **Actual Count**: count of the actual entities defined in the input
    1. **Possible Count**: count of predicted entities by the system

- We use Micro Averaging F-score
    - Aggregate the prementioned counts for all utterances and calculate F-score

- Calculate Precision


    ![\Large Precision=\frac{CorrectType+CorrectText}{Actual*2}](https://latex.codecogs.com/svg.latex?\Large&space;Precision=\frac{CorrectType+CorrectText}{Actual*2})

- Calculate Recall


    ![\Large Recall=\frac{CorrectType+CorrectText}{Actual*2}](https://latex.codecogs.com/svg.latex?\Large&space;Recall=\frac{CorrectType+CorrectText}{Possible*2})

- Calculate Fscore


    ![\Large F_1=\frac{2*Precision*Recall}{Precision+Recall}](https://latex.codecogs.com/svg.latex?\Large&space;F_1=\frac{2*Precision*Recall}{Precision+Recall}) 

- Calculate Entity Type Fscore


    ![\Large Precision=\frac{CorrectType+CorrectText}{Actual*2}](https://latex.codecogs.com/svg.latex?\Large&space;Precision=\frac{CorrectType}{Actual})

    ![\Large Precision=\frac{CorrectType+CorrectText}{Actual*2}](https://latex.codecogs.com/svg.latex?\Large&space;Precision=\frac{CorrectType}{Possible})

- Calculate Entity Text Fscore


    ![\Large Precision=\frac{CorrectType+CorrectText}{Actual*2}](https://latex.codecogs.com/svg.latex?\Large&space;Precision=\frac{CorrectText}{Actual})
    
    ![\Large Precision=\frac{CorrectType+CorrectText}{Actual*2}](https://latex.codecogs.com/svg.latex?\Large&space;Precision=\frac{CorrectText}{Possible})
