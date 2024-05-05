using System;
using System.Collections;
using System.Collections.Generic;

namespace Demo2
{
    public class FilteringCollection<T> : IEnumerable<T>
    {
        private readonly IEnumerable<T> _source;
        private readonly Func<T, bool> _filter;

        public FilteringCollection(IEnumerable<T> source, Func<T, bool> filter)
        {
            _source = source;
            _filter = filter;
        }

        public IEnumerator<T> GetEnumerator() => new FilteringEnumerator(this);

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        private class FilteringEnumerator : IEnumerator<T>
        {
            private readonly FilteringCollection<T> _owner;
            private IEnumerator<T> _sourceEnumerator;

            public FilteringEnumerator(FilteringCollection<T> owner)
            {
                _owner = owner;
                _sourceEnumerator = _owner._source.GetEnumerator();
            }

            public bool MoveNext()
            {
                while (_sourceEnumerator.MoveNext())
                {
                    var current = _sourceEnumerator.Current;
                    if (_owner._filter(current))
                    {
                        Current = current;
                        return true;
                    }
                }
                return false;
            }

            public void Reset() => throw new NotImplementedException();

            public T Current { get; private set; }

            object IEnumerator.Current => Current;

            public void Dispose() => _sourceEnumerator.Dispose();
        }
    }
}