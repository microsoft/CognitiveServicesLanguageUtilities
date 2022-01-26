using Azure;
using Azure.AI.TextAnalytics;
using CustomSkillsetFunction.Models;
using CustomSkillsetFunction.Models.CustomTextPredictionService;
using CustomSkillsetFunction.ViewModels.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomSkillsetFunction.Services
{
    public class CustomTextPredictionService
    {
        private TextAnalyticsClient _textAnalyticsClient;
        public CustomTextPredictionService(ResourceCredentials credentials)
        {
            _textAnalyticsClient = new TextAnalyticsClient(new Uri(credentials.EndpointUrl), new AzureKeyCredential(credentials.Key));
        }
        public async Task<List<ResponseRecord>> AnalyzeDocuments(Dictionary<string, string> documents, List<ITextAnalyticsTask> targetTasks)
        {
            var requestTasks = new TextAnalyticsActions();
            targetTasks.ForEach(action => action.Inject(requestTasks));

            // map documents
            var mappedDocs = documents
                .ToList()
                .Select(entry => new TextDocumentInput(entry.Key, entry.Value))
                .ToList();

            // start analyzing
            var operation = await _textAnalyticsClient.StartAnalyzeActionsAsync(mappedDocs, requestTasks);

            // wait for operation to finish
            await operation.WaitForCompletionAsync();

            // extract results
            var operationResult = operation.GetValues().First();

            return ExtractResults(operationResult, targetTasks);
        }

        private List<ResponseRecord> ExtractResults(AnalyzeActionsResult operationResult, List<ITextAnalyticsTask> targetTasks)
        {
            // extract action results
            var allActionsResult = targetTasks.SelectMany(task => task.ExtractResult(operationResult));

            // group results by document id
            var documentGroups = allActionsResult.GroupBy(record => record.RecordId);

            // extract action results
            return documentGroups.ToList().Select(documentGroup =>
            {
                var res = new ResponseRecord()
                {
                    RecordId = documentGroup.Key
                };
                documentGroup.ToList().ForEach(action =>
                {
                    action.Data.ToList().ForEach(entry => res.Data.Add(entry.Key, entry.Value));
                    res.Errors.AddRange(action.Errors);
                });
                return res;
            }).ToList();
        }
    }
}
