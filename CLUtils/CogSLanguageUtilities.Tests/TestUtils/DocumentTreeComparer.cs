using Microsoft.CogSLanguageUtilities.Definitions.Models.Document;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Microsoft.CogSLanguageUtilities.Tests.TestUtils
{
    public class DocumentTreeComparer : IEqualityComparer<DocumentTree>
    {
        public bool Equals(DocumentTree x, DocumentTree y)
        {
            return EqualsInternal(x.RootSegment, y.RootSegment);
        }

        private bool EqualsInternal(DocumentSegment segment1, DocumentSegment segment2)
        {
            // basic
            if ( (segment1 == null && segment2 == null) || (segment1.Children == null && segment2.Children == null) )
            {
                return true;
            }
            if ( (segment1 == null || segment2 == null) || (segment1.Children == null || segment2.Children == null) )
            {
                return false;
            }
            if (segment1.Children.Count != segment2.Children.Count)
            {
                return false;
            }
            if (segment1.RootElement.PageNumber != segment2.RootElement.PageNumber || segment1.RootElement.Text != segment2.RootElement.Text)
            {
                return false;
            }
            // recursive step
            for (var i = 0; i < segment1.Children.Count; i++)
            {
                // access time is O(1). List is implemented internally as dyanmic array
                if (!EqualsInternal(segment1.Children[i], segment2.Children[i]))
                {
                    return false;
                }
            }
            return true;
        }

        public int GetHashCode(DocumentTree obj)
        {
            return obj.GetHashCode();
        }
    }
}
