using CustomTextAnalytics.MiniSDK.RestClient.Models.GetJobResultApi;
using Newtonsoft.Json;
using System;
using System.Linq;

namespace CustomTextAnalytics.MiniSDK.Client.Models
{
    public class AnalyzeJobInfo
    {
        [JsonProperty("jobId")]
        public Guid JobId { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("errors")]
        public object[] Errors { get; set; }

        [JsonProperty("customEntityRecognitionTask")]
        public AnalyzeJobResultCustomEntityRecognitionTask CustomEntityRecognitionTask { get; set; }

        internal static AnalyzeJobInfo FormGenerated(GetJobResultApiResponse getJobResultApiResponse)
        {
            var taskResult = getJobResultApiResponse?.Tasks?.CustomEntityRecognitionTasks?.First()?.Results;

            var CustomEntityRecognitionTaskErrors = taskResult?.Errors;
            var documentId = taskResult?.Documents.First().Id;
            var entities = taskResult?.Documents?.First()?.Entities;

            var resultEntities = entities == null ? null : entities.Select(entity =>
                {
                    return new CustomEntity()
                    {
                        Category = entity.Category,
                        ConfidenceScore = entity.ConfidenceScore,
                        Length = entity.Length,
                        Offset = entity.Offset,
                        Text = entity.Text
                    };
                }).ToArray();

            return new AnalyzeJobInfo()
            {
                JobId = getJobResultApiResponse.JobId,
                Status = getJobResultApiResponse.Status,
                Errors = getJobResultApiResponse.Errors,
                CustomEntityRecognitionTask = new AnalyzeJobResultCustomEntityRecognitionTask
                {
                    Errors = CustomEntityRecognitionTaskErrors,
                    Document = new AnalyzeJobResultDocument
                    {
                        Id = documentId,
                        Entities = resultEntities
                    }
                }
            };
        }

        public CustomEntity[] GetResultEntities()
        {
            return CustomEntityRecognitionTask.Document.Entities;
        }
    }

    public class AnalyzeJobResultCustomEntityRecognitionTask
    {
        [JsonProperty("document")]
        public AnalyzeJobResultDocument Document { get; set; }

        [JsonProperty("errors")]
        public object[] Errors { get; set; }
    }

    public class AnalyzeJobResultDocument
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("entities")]
        public CustomEntity[] Entities { get; set; }
    }

    public class CustomEntity
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
