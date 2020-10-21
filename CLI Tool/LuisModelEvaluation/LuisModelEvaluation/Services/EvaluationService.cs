// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
﻿using Microsoft.LuisModelEvaluation.Configs;
using Microsoft.LuisModelEvaluation.Models.Evaluation;
using Microsoft.LuisModelEvaluation.Models.Input;
using Microsoft.LuisModelEvaluation.Models.Result;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.LuisModelEvaluation.Services
{
    /// <summary>
    /// This class is responsible for calculating and aggregating all the confusion values for a batch test.
    /// It allows incremental aggregation of confusions results of classes and entities as well as keeping track
    /// of queries false positive and false negative entities.
    /// This class follows the "Message Understanding Conference (MUC)" terminology
    /// https://en.wikipedia.org/wiki/Message_Understanding_Conference
    /// Entity evaluation specifics can be found in the following paper
    /// https://nlp.cs.nyu.edu/sekine/papers/li07.pdf
    /// Some useful MUC terms
    /// ActualCount: is the count of labeled entities.
    /// PossibleCount: is the count of predicted entities.
    /// CorrectTypeCount: is the count of predicted entities that match in type and overlap in place with labeled ones.
    /// CorrectTextCount: is the count of predicted entities that exactly match in place with labeled ones.
    /// </summary>
    public class EvaluationService
    {
        public EvaluationService(IReadOnlyList<Model> entities, IReadOnlyList<Model> classes)
        {
            InitClassificationAndEntityStats(entities, classes);
        }

        public Dictionary<string, ConfusionMatrix> ClassificationStats { get; } = new Dictionary<string, ConfusionMatrix>();

        public Dictionary<string, MucEntityConfusionMatrix> EntityStats { get; } = new Dictionary<string, MucEntityConfusionMatrix>();

        public List<QueryStats> QueryStats { get; } = new List<QueryStats>();

        /// <summary>
        /// Incrementally aggregates the confusion results for classes
        /// </summary>
        /// <param name="labeledClassNamesSet">HashSet containing the labeled class names for the test example</param>
        /// <param name="predictedClassNamesSet">HashSet containing the predicted class names for the test example</param>
        public void AggregateClassificationStats(
            HashSet<string> labeledClassNamesSet,
            HashSet<string> predictedClassNamesSet)
        {
            // calculate false negatives
            foreach (var predictedClassName in labeledClassNamesSet)
            {
                // Initialize if not in dictionary to avoid null errors
                if (!ClassificationStats.ContainsKey(predictedClassName))
                {
                    ClassificationStats[predictedClassName] = new ConfusionMatrix
                    {
                        ModelName = predictedClassName,
                        ModelType = Constants.ModelNotFoundMessage
                    };
                }
                // increment false negatives
                if (!predictedClassNamesSet.Contains(predictedClassName))
                {
                    ClassificationStats[predictedClassName].FalseNegatives++;
                }
            }
            // calculate false positives
            foreach (var predictedClassName in predictedClassNamesSet)
            {
                if (labeledClassNamesSet.Contains(predictedClassName))
                {
                    ClassificationStats[predictedClassName].TruePositives++;
                }
                else
                {
                    // Initialize if not in dictionary to avoid null errors
                    if (!ClassificationStats.TryGetValue(predictedClassName, out ConfusionMatrix predictedConfusionCount))
                    {
                        predictedConfusionCount = ClassificationStats[predictedClassName] = new ConfusionMatrix
                        {
                            ModelName = predictedClassName,
                            ModelType = Constants.ModelNotFoundMessage
                        };
                    }
                    predictedConfusionCount.FalsePositives++;
                }
            }
        }

        /// <summary>
        /// Incrementally aggregates the confusion results for entities
        /// and populates false positive and negatives entities for each query
        /// </summary>
        /// <param name="labeledEntities">List of labeled entities for the test example</param>
        /// <param name="predictedEntities">List of predicted entities for the test example</param>
        /// <param name="queryStats">Stats calculated for the query</param>
        public void PopulateQueryAndEntityStats(
            IReadOnlyList<Entity> labeledEntities,
            IReadOnlyList<Entity> predictedEntities,
            QueryStats queryStats)
        {
            /*
             *  Will keep track of false negative entities in a dictionary with key in this format
             *  key = ["<entity name><entity start index><entity end index>"]
             *  to make sure they do not collide if multiple entities of the same type exist in same query but in different places
             */
            var falseNegativeEntities = new Dictionary<string, Entity>();
            foreach (var labeledEntity in labeledEntities)
            {
                ProcessLabeledEntitiesRecursively(
                    labeledEntity,
                    falseNegativeEntities);
            }

            foreach (var predictedEntity in predictedEntities)
            {
                EvaluatePredictedEntity(
                    labeledEntities,
                    predictedEntity,
                    queryStats,
                    falseNegativeEntities);
            }

            // After the false negatives are now purified (while evaluating entities), convert and add them to the queryStats
            foreach (var falseNegative in falseNegativeEntities.Values)
            {
                queryStats.FalseNegativeEntities.Add(new EntityNameAndLocation
                {
                    EntityName = falseNegative.Name,
                    StartPosition = falseNegative.StartPosition,
                    EndPosition = falseNegative.EndPosition
                });
            }

            // Add the processed query to the list of processed queries
            QueryStats.Add(queryStats);
        }

        /// <summary>
        /// Returns a bool telling if the provided locations overlap
        /// </summary>
        /// <param name="start1">Start character index of the first entity</param>
        /// <param name="end1">End character index of the first entity</param>
        /// <param name="start2">Start character index of the second entity</param>
        /// <param name="end2">End character index of the second entity</param>
        private static bool LocationsOverlap(int start1, int end1, int start2, int end2)
        {
            return start1 <= end2 && start2 <= end1;
        }

        /// <summary>
        /// Returns a bool telling if the provided locations match exactly
        /// </summary>
        /// <param name="start1">Start character index of the first entity</param>
        /// <param name="end1">End character index of the first entity</param>
        /// <param name="start2">Start character index of the second entity</param>
        /// <param name="end2">End character index of the second entity</param>
        private static bool LocationsMatchExactly(int start1, int end1, int start2, int end2)
        {
            return start1 == start2 && end1 == end2;
        }

        /// <summary>
        /// Appends the parent name with entity name to get a full qualified name
        /// </summary>
        /// <param name="parentName">Name of the parent entity</param>
        /// <param name="entityName">Name of the child entity</param>
        private static string GetEntityFullName(string parentName, string entityName)
        {
            if (string.IsNullOrEmpty(parentName))
            {
                return entityName;
            }
            return GetFormattedHierarchicalChildName(parentName, entityName);
        }

        /// <summary>
        /// Appends the full entity name with start and end positions to form a unique key
        /// key = ["{entity_name}{entity_start_index}{entity_end_index}"]
        /// to make sure they do not collide if multiple entities of the same type exist in same query but in different places
        /// </summary>
        /// <param name="entityFullName">Entity full hierarchical name</param>
        /// <param name="startPosition">Entity start character index</param>
        /// <param name="endPosition">Entity end character index</param>
        /// <returns>A unique string representation of the entity in the example</returns>
        private static string GetUniqueEntityKey(string entityFullName, int startPosition, int endPosition)
        {
            return $"{entityFullName}{startPosition}{endPosition}";
        }

        /// <summary>
        /// Add up labeled entities and their children counts
        /// and populate all labeled entities as false negatives initially
        /// </summary>
        /// <param name="labeledEntity">The entity to be processed</param>
        /// <param name="falseNegativeEntities">Dictionay containing false negative entities indexed by their unique key</param>
        /// <param name="labeledEntityPrefix">Parent entity full hierarchical name</param>
        private void ProcessLabeledEntitiesRecursively(
            Entity labeledEntity,
            Dictionary<string, Entity> falseNegativeEntities,
            string labeledEntityPrefix = "")
        {
            // get or create entity stats
            string labeledEntityFullName = GetEntityFullName(labeledEntityPrefix, labeledEntity.Name);
            var labeledEntityEvalObj = GetOrAddEntityStatObject(labeledEntityFullName);

            // init possible count
            labeledEntityEvalObj.ActualCount++;

            // Add all labeled as false negatives now and remove entities that match while evaluating entities
            var entityKey = GetUniqueEntityKey(labeledEntityFullName, labeledEntity.StartPosition, labeledEntity.EndPosition);
            falseNegativeEntities[entityKey] = new Entity()
            {
                Name = labeledEntityFullName,
                StartPosition = labeledEntity.StartPosition,
                EndPosition = labeledEntity.EndPosition
            };

            // Process children recursively
            if (labeledEntity.Children != null)
            {
                foreach (var childLabeledEntity in labeledEntity.Children)
                {
                    ProcessLabeledEntitiesRecursively(
                        childLabeledEntity,
                        falseNegativeEntities,
                        labeledEntityFullName);
                }
            }
        }

        /// <summary>
        /// Performs all the needed calculations on a predicted entity
        /// </summary>
        private void EvaluatePredictedEntity(
            IReadOnlyList<Entity> labeledEntities,
            Entity predictedEntity,
            QueryStats queryStats,
            Dictionary<string, Entity> falseNegativeEntities)
        {
            AddUpPredictedEntitiesCountRecursively(predictedEntity);

            AddUpCorrectTypeAndTextEntitiesCountsRecursively(
                                labeledEntities,
                                predictedEntity,
                                queryStats,
                                falseNegativeEntities);
        }

        /// <summary>
        /// Add up predicted entities count for the predicted entity and all its children
        /// </summary>
        private void AddUpPredictedEntitiesCountRecursively(
            Entity predictedEntity,
            string predictedEntityPrefix = "")
        {
            // get or create entity stats
            string predictedEntityFullName = GetEntityFullName(predictedEntityPrefix, predictedEntity.Name);
            var predictedEntityEvalObj = GetOrAddEntityStatObject(predictedEntityFullName);

            // init possible count
            predictedEntityEvalObj.PossibleCount++;

            if (predictedEntity.Children != null)
            {
                foreach (var childPredictedEntity in predictedEntity.Children)
                {
                    AddUpPredictedEntitiesCountRecursively(childPredictedEntity, predictedEntityFullName);
                }
            }
        }

        /// <summary>
        /// Add up the correct matchings (type/text) counts for predicted entities and their children
        /// and remove any matching labeled entity from false negatives
        /// and populate false positive predicted entities
        /// </summary>
        private void AddUpCorrectTypeAndTextEntitiesCountsRecursively(
            IReadOnlyList<Entity> labeledEntities,
            Entity predictedEntity,
            QueryStats queryStats,
            Dictionary<string, Entity> falseNegativeEntities,
            string labeledEntityPrefix = "",
            string predictedEntityPrefix = "")
        {
            // get or create entity stats
            string predictedEntityFullName = GetEntityFullName(predictedEntityPrefix, predictedEntity.Name);
            var predictedEntityEvalObj = GetOrAddEntityStatObject(predictedEntityFullName);

            // A boolean to keep track if the guessed entity matches with any labeled entity
            var isFalsePositive = true;
            var setChildrenAsFalse = true;
            foreach (var labeledEntity in labeledEntities ?? Enumerable.Empty<Entity>())
            {
                var labeledEntityFullName = GetEntityFullName(labeledEntityPrefix, labeledEntity.Name);

                // Filtering on entity type/name
                if (predictedEntityFullName == labeledEntityFullName)
                {
                    // If predicted parent type matched a labeled then some children might be true positives
                    // So we don't need to set children as false positives since they're going to be handled in the recursive call
                    setChildrenAsFalse = false;

                    // Check MUC Type correctness by validating intersection
                    var locationsOverlap = LocationsOverlap(
                        labeledEntity.StartPosition,
                        labeledEntity.EndPosition,
                        predictedEntity.StartPosition,
                        predictedEntity.EndPosition);
                    if (locationsOverlap)
                    {
                        predictedEntityEvalObj.CorrectTypeCount++;
                    }

                    // Check MUC Text correctness by validating exact location match
                    var locationsMatchExactly = LocationsMatchExactly(
                        labeledEntity.StartPosition,
                        labeledEntity.EndPosition,
                        predictedEntity.StartPosition,
                        predictedEntity.EndPosition);
                    if (locationsMatchExactly)
                    {
                        predictedEntityEvalObj.CorrectTextCount++;

                        // Guessed entity matches a labeled one then it is a true positive and not a false positive
                        isFalsePositive = false;

                        // If labeled entity matches exactly with a guessed entity then remove the labeled one from false negatives
                        falseNegativeEntities.Remove(GetUniqueEntityKey(labeledEntityFullName, labeledEntity.StartPosition, labeledEntity.EndPosition));
                    }

                    // Since this level matches, we need to also check if lower levels match and account for them recursively
                    if (predictedEntity.Children != null)
                    {
                        foreach (var childPredictedEntity in predictedEntity.Children)
                        {
                            AddUpCorrectTypeAndTextEntitiesCountsRecursively(
                                labeledEntity.Children,
                                childPredictedEntity,
                                queryStats,
                                falseNegativeEntities,
                                labeledEntityFullName,
                                predictedEntityFullName);
                        }
                    }
                }
            }

            // If guessed entity didn't match exactly with any labeled entity then it is a false positive
            // and if it didn't match type with any labeled one then also all of its children are false positives
            if (isFalsePositive)
            {
                AddFalsePositivesRecursively(
                    predictedEntity,
                    queryStats,
                    setChildrenAsFalse,
                    predictedEntityPrefix);
            }
        }

        /// <summary>
        /// Add the predicted entity and its children* as false positives
        /// You can control processing the children or not by providing the bool `alsoAddChildren`
        /// </summary>
        private void AddFalsePositivesRecursively(
            Entity predictedEntity,
            QueryStats queryStats,
            bool alsoAddChildren,
            string predictedEntityPrefix = "")
        {
            var predictedEntityFullName = GetEntityFullName(predictedEntityPrefix, predictedEntity.Name);

            queryStats.FalsePositiveEntities.Add(new EntityNameAndLocation
            {
                EntityName = predictedEntityFullName,
                StartPosition = predictedEntity.StartPosition,
                EndPosition = predictedEntity.EndPosition
            });

            if (alsoAddChildren && predictedEntity.Children != null)
            {
                foreach (var childPredictedEntity in predictedEntity.Children)
                {
                    AddFalsePositivesRecursively(
                        childPredictedEntity,
                        queryStats,
                        alsoAddChildren,
                        predictedEntityFullName);
                }
            }
        }

        /// <summary>
        /// <summary>
        /// Add classes and entities to the stats data structues
        /// This will help models that never appeared in the labeled or the predicted to also be indicated in the results
        /// </summary>
        /// </summary>
        /// <param name="entities">List of all entity models in the application</param>
        /// <param name="classes">List of all classification models in the application</param>
        private void InitClassificationAndEntityStats(IReadOnlyList<Model> entities, IReadOnlyList<Model> classes)
        {
            if (classes != null)
            {
                foreach (var e in classes)
                {
                    ClassificationStats[e.Name] = new ConfusionMatrix
                    {
                        ModelName = e.Name,
                        ModelType = e.Type
                    };
                }
            }
            if (entities != null)
            {
                foreach (var c in entities)
                {
                    EntityStats[c.Name] = new MucEntityConfusionMatrix
                    {
                        ModelName = c.Name,
                        ModelType = c.Type
                    };
                }
            }
        }

        /// <summary>
        /// Returns the full hierarchical name of an entity from its name and its parent's name using the hierarchical separator "::"
        /// </summary>
        /// <param name="parentName">Parent entity full hierarchical name</param>
        /// <param name="childName">Child entity name</param>
        public static string GetFormattedHierarchicalChildName(string parentName, string childName)
        {
            return $"{parentName}{Constants.ModelHierarchySeparator}{childName}";
        }

        /// <summary>
        /// Returns the MucEntityConfusionMatrix for the given entity from the EntityStats dictionary and creates one if it doesn't exist
        /// </summary>
        /// <param name="entityFullName">Entity full hierarchical name</param>
        private MucEntityConfusionMatrix GetOrAddEntityStatObject(string entityFullName)
        {
            // Create if not exists in dictionary
            if (!EntityStats.ContainsKey(entityFullName))
            {
                EntityStats[entityFullName] = new MucEntityConfusionMatrix
                {
                    ModelName = entityFullName,
                    ModelType = Constants.ModelNotFoundMessage
                };
            }
            return EntityStats[entityFullName];
        }
    }
}
