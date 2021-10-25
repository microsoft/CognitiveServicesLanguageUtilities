// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
using Microsoft.IAPUtilities.Definitions.Models.Luis;
using System.Threading.Tasks;

namespace Microsoft.IAPUtilities.Definitions.APIs.Services
{
    public interface ILuisPredictionService
    {
        public Task<CustomLuisResponse> Predict(string query);
    }
}