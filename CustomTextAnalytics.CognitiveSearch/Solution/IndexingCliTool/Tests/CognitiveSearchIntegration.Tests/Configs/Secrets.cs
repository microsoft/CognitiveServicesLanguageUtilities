using System;

namespace CognitiveSearchIntegration.Tests.Configs
{
    public sealed class Secrets
    {
        #region singleton_definition
        private Secrets()
        {
            LoadTestSecrets();
        }
        private static Secrets instance = null;
        public static Secrets Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Secrets();
                }
                return instance;
            }
        }
        #endregion singleton_definition

        // blob secrets
        public string BlobConnectionString;
        public string BlobContainerName;

        // custom skillset secrets
        public string CustomSkillsetAzureFunctionUrl;
        public string CustomSkillsetCustomtextServiceEndpoint;
        public string CustomSkillsetCustomtextServiceKey;
        public string CustomSkillsetCustomtextProjectName;
        public string CustomSkillsetCustomtextDeploymentName;

        // cognitive search
        public string CognitiveSearchServiceEndpoint;
        public string CognitiveSearchServiceKey;

        public void LoadTestSecrets()
        {
            // blob secrets
            BlobConnectionString = Environment.GetEnvironmentVariable("BLOB_CONNECTION_STRING");
            BlobContainerName = Environment.GetEnvironmentVariable("BLOB_CONTAINER_NAME");

            // custom skillset secrets
            CustomSkillsetAzureFunctionUrl = Environment.GetEnvironmentVariable("CUSTOMSKILLSET_AZUREFUNCTION_URL");
            CustomSkillsetCustomtextServiceEndpoint = Environment.GetEnvironmentVariable("CUSTOMSKILLSET_CUSTOMTEXT_SERVICEURL");
            CustomSkillsetCustomtextServiceKey = Environment.GetEnvironmentVariable("CUSTOMSKILLSET_CUSTOMTEXT_SERVICEKEY");
            CustomSkillsetCustomtextProjectName = Environment.GetEnvironmentVariable("CUSTOMSKILLSET_CUSTOMTEXT_PROJECTNAME");
            CustomSkillsetCustomtextDeploymentName = Environment.GetEnvironmentVariable("CUSTOMSKILLSET_CUSTOMTEXT_DEPLOYMENTNAME");

            // cognitive search
            CognitiveSearchServiceEndpoint = Environment.GetEnvironmentVariable("CUSTOMSKILLSET_COGNITIVESEARCH_ENDPOINT");
            CognitiveSearchServiceKey = Environment.GetEnvironmentVariable("CUSTOMSKILLSET_COGNITIVESEARCH_KEY");
        }
    }
}
