// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System;

namespace Microsoft.Azure.CognitiveServices.Language.LUIS.Utilities.FuzzyMatching.Tests.Helpers
{
    internal static class RandomGenerator
    {
        private static Random _random = new Random();
        internal static int GetRandomIndex(int modulo)
        {
            return _random.Next() % modulo;
        }
    }
}
