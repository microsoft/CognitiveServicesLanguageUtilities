// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
﻿namespace Microsoft.CogSLanguageUtilities.Definitions.Enums.Parser
{
    public enum ElementType
    {
        Root = 0,
        Title = 1,
        Heading1 = 2,
        Heading2 = 3,
        Heading3 = 4,
        Paragraph = 5,
        BulletedList = 6,
        Table = 7,
        Other
    }

    public static class ElementTypeExtension
    {
        /// <summary>
        ///  by simple types we mean elements that can't have children
        ///  paragraphs, tables, bulleted lists
        /// </summary>
        /// <returns>returns true if element type is a simple type</returns>
        public static bool IsSimpleTypeElement(this ElementType elementType)
        {
            if (elementType == ElementType.Paragraph || elementType == ElementType.BulletedList || elementType == ElementType.Table)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// returns true if precedence (newElement  < baseElement)
        /// </summary>
        public static bool IsLowerPrecedence(this ElementType newElement, ElementType baseElement)
        {
            return newElement > baseElement;
        }

        /// <summary>
        /// returns true if precedence (newElement  >= baseElement)
        /// </summary>
        public static bool IsHigherOrEqualPrecedence(this ElementType newElement, ElementType baseElement)
        {
            return newElement <= baseElement;
        }
    }
}
