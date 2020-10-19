using System;

namespace Microsoft.LuisModelEvaluation.Exceptions
{
    public class InvalidInputException : Exception
    {
        public InvalidInputException(string message)
            : base(ConstructMessage(message))
        { }

        public static string ConstructMessage(string fieldName)
        {
            return $"The field {fieldName} can't be null";
        }
    }
}
