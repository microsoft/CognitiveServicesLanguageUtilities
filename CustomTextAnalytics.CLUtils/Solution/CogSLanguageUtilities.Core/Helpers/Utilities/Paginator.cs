// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
using Microsoft.IAPUtilities.Definitions.APIs.Helpers.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.IAPUtilities.Core.Helpers.Utilities
{
    class Paginator<T> : IPaginator<T>
    {
        private IEnumerable<T> _items;
        private int _currentPage;
        private int _pageSize;
        public Paginator(IEnumerable<T> items, int pageSize)
        {
            _items = items;
            _currentPage = 0;
            _pageSize = pageSize;
        }
        public bool HasNext()
        {
            var skip = _currentPage * _pageSize;
            return skip < _items.Count();
        }
        public IEnumerable<T> GetNextPage()
        {
            var skip = _currentPage * _pageSize;
            _currentPage++;
            return _items.Skip(skip).Take(_pageSize);
        }
    }
}
