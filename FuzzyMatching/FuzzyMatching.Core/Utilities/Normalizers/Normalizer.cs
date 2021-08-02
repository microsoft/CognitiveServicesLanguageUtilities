using System;
using System.Collections.Generic;
using System.Text;

namespace FuzzyMatching.Core.Utilities.Normalizers
{
    public static class Normalizer
    {
        public static float NormalizeSimilarity(float similarity)
        {
            //Happens when sentenceToMatch is an empty string
            if(float.IsNaN(similarity) || float.IsPositiveInfinity(similarity))
            {
                return 0;
            }

            //perfect match threshold
            if (Math.Abs(similarity - 1f) < 0.0001f)
                return 1;


            var exp = Math.Pow(Math.E, 2f*similarity-4.5);
            var score = -exp / (1 + exp) + 1;
            
            
            return (float)score;
        }
    }
}
