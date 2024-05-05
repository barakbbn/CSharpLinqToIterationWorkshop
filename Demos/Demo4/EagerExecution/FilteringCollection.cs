using System;
using System.Collections;
using System.Collections.Generic;

namespace Demo4.EagerExecution
{
    class FilteringCollection<T> : IEnumerable<T>
    {
        private readonly IEnumerable<T> _source;
        private readonly Func<T, bool> _filter;

        public FilteringCollection(IEnumerable<T> source, Func<T, bool> filter)
        {
            _source = source;
            _filter = filter;
        }

        public IEnumerator<T> GetEnumerator()
        {
            var filteredItems = new List<T>();
            foreach (var item in _source)
            {
                if (_filter(item)) { filteredItems.Add(item); }
            }
            return filteredItems.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}