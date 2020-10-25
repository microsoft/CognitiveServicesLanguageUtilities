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
            if(x == null && y == null)
            {
                return true;
            }
            else if (x == null || y == null)
            {
                return false;
            }
            var list = x.RootSegment.Children.Zip(y.RootSegment.Children, (e1, e2) => new { e1, e2 });
            foreach (var entry in list)
            {
                if (!EqualsInternal(entry.e1, entry.e2))
                {
                    return false;
                }
            }
            return true;
        }

        private bool EqualsInternal(DocumentSegment segment1, DocumentSegment segment2)
        {
            if (segment1.RootElement.PageNumber != segment2.RootElement.PageNumber || segment1.RootElement.Text != segment2.RootElement.Text)
            {
                return false;
            }
            if (segment1.Children != null)
            {
                if (segment2.Children == null || segment1.Children.Count != segment2.Children.Count)
                {
                    return false;
                }
                var list = segment1.Children.Zip(segment2.Children, (e1, e2) => new { e1, e2 });
                foreach (var entry in list)
                {
                    if (!EqualsInternal(entry.e1, entry.e2))
                    {
                        return false;
                    }
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
