using CognitiveSearchIntegration.Runners.Configs;
using CognitiveSearchIntegration.Runners.Models.ConfigsModel;
using Microsoft.CognitiveSearchIntegration.Utilities.Nugets.Helpers;
using System;
using System.IO;

namespace CognitiveSearchIntegration.Runners.Services.Loaders
{
    public class ConfigsModelLoader
    {
        public ConfigModel Load(string configsFileDir)
        {
            var filePath = configsFileDir ?? Path.Combine(Constants.ConfigsFileDirectory, Constants.ConfigsFileName);
            if (File.Exists(filePath))
            {
                var configsFile = File.ReadAllText(filePath);
                return JsonHandler.DeserializeObject<ConfigModel>(configsFile, Constants.ConfigsFileName);
            }
            throw new Exception("Error Loading Configs File!");
        }
    }
}
