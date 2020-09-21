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
        public EvaluationService(IEnumerable<Model> entities, IEnumerable<Model> classes)
        {
            InitClassificationAndEntityStats(entities, classes);
        }

        public Dictionary<string, ConfusionMatrix> ClassificationStats { get; } = new Dictionary<string, ConfusionMatrix>();

        public Dictionary<string, MucEntityConfusionMatrix> EntityStats { get; } = new Dictionary<string, MucEntityConfusionMatrix>();

        public List<QueryStats> QueryStats { get; } = new List<QueryStats>();

        /// <summary>
        /// Incrementally aggregates the confusion results for classes
        /// </summary>
        public void AggregateClassificationStats(
            HashSet<string> actualClassNamesSet,
            HashSet<string> predictedClassNamesSet)
        {
            // initialize actualClassName in ClassificationStats
            actualClassNamesSet.Select(actualClassName =>
            {
                if (!ClassificationStats.TryGetValue(actualClassName, out ConfusionMatrix labeledConfusionCount))
                {
                    // Initialize if not in dictionary to avoid null errors
                    labeledConfusionCount = ClassificationStats[actualClassName] = new ConfusionMatrix
                    {
                        ModelName = actualClassName,
                        ModelType = GetModelTypeString(actualClassName)
                    };
                }
                return 0;
            });
            // calculate false positives
            predictedClassNamesSet.Select(predictedClassName =>
            {
                if (actualClassNamesSet.Contains(predictedClassName))
                {
                    ClassificationStats[predictedClassName].TruePositives++;
                }
                else
                {
                    if (!ClassificationStats.TryGetValue(predictedClassName, out ConfusionMatrix predictedConfusionCount))
                    {
                        predictedConfusionCount = ClassificationStats[predictedClassName] = new ConfusionMatrix
                        {
                            ModelName = predictedClassName,
                            ModelType = GetModelTypeString(predictedClassName)
                        };
                    }
                    predictedConfusionCount.FalsePositives++;
                }
                return 0;
            });
            // calculate false negatives
            actualClassNamesSet.Select(actualClassName =>
            {
                if (!predictedClassNamesSet.Contains(actualClassName))
                {
                    ClassificationStats[actualClassName].FalseNegatives++;
                }
                return 0;
            });
        }

        /// <summary>
        /// Incrementally aggregates the confusion results for entities
        /// and populates false positive and negatives entities for each query
        /// </summary>
        public void PopulateQueryAndEntityStats(
            IReadOnlyList<Entity> labeledEntities,
            IEnumerable<Entity> predictedEntities,
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
        private static bool LocationsOverlap(int start1, int end1, int start2, int end2)
        {
            return start1 <= end2 && start2 <= end1;
        }

        /// <summary>
        /// Returns a bool telling if the provided locations match exactly
        /// </summary>
        private static bool LocationsMatchExactly(int start1, int end1, int start2, int end2)
        {
            return start1 == start2 && end1 == end2;
        }

        /// <summary>
        /// Appends the parent name with entity name to get a full qualified name
        /// </summary>
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
        private static string GetUniqueEntityKey(string labeledEntityFullName, int startPosition, int endPosition)
        {
            return $"{labeledEntityFullName}{startPosition}{endPosition}";
        }

        /// <summary>
        /// Add up labeled entities and their children counts
        /// and populate all labeled entities as false negatives initially
        /// </summary>
        private void ProcessLabeledEntitiesRecursively(
            Entity labeledEntity,
            Dictionary<string, Entity> falseNegativeEntities,
            string labeledEntityPrefix = "")
        {
            // get or create entity stats
            string labeledEntityFullName;
            MucEntityConfusionMatrix labeledEntityEvalObj;
            GetOrAddEntityStatObject(labeledEntity, labeledEntityPrefix, out labeledEntityFullName, out labeledEntityEvalObj);

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
            string predictedEntityFullName;
            MucEntityConfusionMatrix predictedEntityEvalObj;
            GetOrAddEntityStatObject(predictedEntity, predictedEntityPrefix, out predictedEntityFullName, out predictedEntityEvalObj);

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
            string predictedEntityFullName;
            MucEntityConfusionMatrix predictedEntityEvalObj;
            GetOrAddEntityStatObject(predictedEntity, predictedEntityPrefix, out predictedEntityFullName, out predictedEntityEvalObj);

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
        /// Add classes and entities to the stats data structues
        /// This will help models that never appeared in the labeled or the predicted to also be indicated in the results
        /// </summary>
        private void InitClassificationAndEntityStats(IEnumerable<Model> entities, IEnumerable<Model> classes)
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
        /// Given a class or entity display name returns the corresponding model type string
        /// </summary>
        private string GetModelTypeString(string entityDisplayName)
        {
            // TODO: Check with Amr that this is correct
            return "Unknown Model Type";
        }

        static string ModelHierarchySeparator = "::";

        public static string GetFormattedHierarchicalChildName(string parentName, string childName)
        {
            return $"{parentName}{ModelHierarchySeparator}{childName}";
        }

        private void GetOrAddEntityStatObject(Entity entity, string entityPrefix, out string entityFullName, out MucEntityConfusionMatrix entityEvalObj)
        {
            entityFullName = GetEntityFullName(entityPrefix, entity.Name);

            // Add up MUC possible/guessed count
            if (!EntityStats.TryGetValue(entityFullName, out entityEvalObj))
            {
                // Create if not exists in dictionary
                entityEvalObj = EntityStats[entityFullName] = new MucEntityConfusionMatrix
                {
                    ModelName = entityFullName,
                    ModelType = GetModelTypeString(entityFullName)
                };
            }
        }
    }
}
