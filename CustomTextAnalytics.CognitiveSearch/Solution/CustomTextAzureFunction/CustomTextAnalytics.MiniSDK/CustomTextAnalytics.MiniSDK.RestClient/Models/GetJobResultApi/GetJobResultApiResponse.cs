using Newtonsoft.Json;
using System;

namespace CustomTextAnalytics.MiniSDK.RestClient.Models.GetJobResultApi
{
    public class GetJobResultApiResponse
    {
        [JsonProperty("displayName")]
        public string DisplayName { get; set; }

        [JsonProperty("jobId")]
        public Guid JobId { get; set; }

        [JsonProperty("lastUpdateDateTime")]
        public DateTimeOffset LastUpdateDateTime { get; set; }

        [JsonProperty("createdDateTime")]
        public DateTimeOffset CreatedDateTime { get; set; }

        [JsonProperty("expirationDateTime")]
        public DateTimeOffset ExpirationDateTime { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("errors")]
        public object[] Errors { get; set; }

        [JsonProperty("tasks")]
        public GetJobResultTasks Tasks { get; set; }
    }

    public class GetJobResultTasks
    {
        [JsonProperty("details")]
        public GetJobResultDetails Details { get; set; }

        [JsonProperty("completed")]
        public long Completed { get; set; }

        [JsonProperty("failed")]
        public long Failed { get; set; }

        [JsonProperty("inProgress")]
        public long InProgress { get; set; }

        [JsonProperty("total")]
        public long Total { get; set; }

        [JsonProperty("customEntityRecognitionTasks")]
        public GetJobResultCustomEntityRecognitionTask[] CustomEntityRecognitionTasks { get; set; }
    }
    public class GetJobResultDetails
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("lastUpdateDateTime")]
        public DateTimeOffset LastUpdateDateTime { get; set; }
    }

    public class GetJobResultCustomEntityRecognitionTask
    {
        [JsonProperty("lastUpdateDateTime")]
        public DateTimeOffset LastUpdateDateTime { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("state")]
        public string State { get; set; }

        [JsonProperty("results")]
        public GetJobResultResults Results { get; set; }
    }

    public class GetJobResultResults
    {
        [JsonProperty("documents")]
        public GetJobResultDocument[] Documents { get; set; }

        [JsonProperty("errors")]
        public object[] Errors { get; set; }

        [JsonProperty("projectName")]
        public string ProjectName { get; set; }

        [JsonProperty("deploymentName")]
        public string DeploymentName { get; set; }
    }

    public class GetJobResultDocument
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("entities")]
        public GetJobResultEntity[] Entities { get; set; }

        [JsonProperty("warnings")]
        public object[] Warnings { get; set; }
    }

    public class GetJobResultEntity
    {
        [JsonProperty("text")]
        public string Text { get; set; }

        [JsonProperty("category")]
        public string Category { get; set; }

        [JsonProperty("offset")]
        public long Offset { get; set; }

        [JsonProperty("length")]
        public long Length { get; set; }

        [JsonProperty("confidenceScore")]
        public double ConfidenceScore { get; set; }
    }
}
