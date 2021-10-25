using CustomTextAnalytics.MiniSDK.RestClient.Utilities;
using Newtonsoft.Json;

namespace CustomTextAnalytics.MiniSDK.RestClient.Models.AnalyzeApi
{
    internal class AnalyzeApiRequestBody
    {
        [JsonProperty("displayName")]
        internal string DisplayName { get; set; }

        [JsonProperty("analysisInput")]
        internal AnalyzeInput AnalysisInput { get; set; }

        [JsonProperty("tasks")]
        internal AnalyzeTasks Tasks { get; set; }

        internal AnalyzeApiRequestBody(string documentText, string projectName, string deploymentName)
        {
            DisplayName = RandomGenerator.GenerateRandomString();
            AnalysisInput = new AnalyzeInput()
            {
                Documents = new AnalyzeDocument[] {
                    new AnalyzeDocument() {
                        Id = RandomGenerator.GenerateRandomId(),
                        Text = documentText
                    }
                }
            };
            Tasks = new AnalyzeTasks()
            {
                CustomEntityRecognitionTasks = new AnalyzeCustomEntityRecognitionTask[] {
                    new AnalyzeCustomEntityRecognitionTask(){
                        Parameters = new AnalyzeCustomEntityParameters(){
                            ProjectName = projectName,
                            DeploymentName = deploymentName,
                            StringIndexType = "TextElements_v8"
                        }
                    }
                }
            };
        }
    }

    internal class AnalyzeInput
    {
        [JsonProperty("documents")]
        internal AnalyzeDocument[] Documents { get; set; }
    }

    internal class AnalyzeDocument
    {
        [JsonProperty("id")]
        internal string Id { get; set; }

        [JsonProperty("text")]
        internal string Text { get; set; }
    }

    internal class AnalyzeTasks
    {
        [JsonProperty("customEntityRecognitionTasks")]
        internal AnalyzeCustomEntityRecognitionTask[] CustomEntityRecognitionTasks { get; set; }
    }

    internal class AnalyzeCustomEntityRecognitionTask
    {
        [JsonProperty("parameters")]
        internal AnalyzeCustomEntityParameters Parameters { get; set; }
    }

    internal class AnalyzeCustomEntityParameters
    {
        [JsonProperty("project-name")]
        internal string ProjectName { get; set; }

        [JsonProperty("deployment-name")]
        internal string DeploymentName { get; set; }

        [JsonProperty("stringIndexType")]
        internal string StringIndexType { get; set; }
    }
}
