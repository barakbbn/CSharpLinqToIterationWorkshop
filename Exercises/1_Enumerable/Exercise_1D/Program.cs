using System;
using System.Collections;
using System.Collections.Generic;

namespace Exercise_1D
{
    public class Program
    {
        static void Main(string[] args) { }
    }

    public class DistinctUntilChanged<T> : IEnumerable<T>
    {
        private readonly IEnumerable<T> _source;
        private readonly IEqualityComparer<T> _comparer;

        public DistinctUntilChanged(IEnumerable<T> source)
            : this(source, null) { }

        public DistinctUntilChanged(IEnumerable<T> source, IEqualityComparer<T> comparer)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }
            _source = source;
            _comparer = comparer ?? EqualityComparer<T>.Default;
        }

        public IEnumerator<T> GetEnumerator()
        {
            return new DistinctUntilChangedEnumerator(this);
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        private class DistinctUntilChangedEnumerator : IEnumerator<T>
        {
            private List<T> _results = new List<T>();
            private int _currentIndex = -1;

            public DistinctUntilChangedEnumerator(DistinctUntilChanged<T> owner)
            {
                var comparer = owner._comparer;
                foreach (var item in owner._source)
                {
                    if (_results.Count == 0)
                    {
                        _results.Add(item);
                        continue;
                    }
                    var lastResult = _results[_results.Count - 1];
                    var sameValue = comparer.Equals(lastResult, item);
                    if (!sameValue)
                    {
                        _results.Add(item);
                    }
                }
            }

            public T Current { get; private set; }

            object IEnumerator.Current => Current;

            public bool MoveNext()
            {
                if (_currentIndex + 1 == _results.Count)
                {
                    return false;
                }

                _currentIndex++;
                Current = _results[_currentIndex];
                return true;
            }

            public void Dispose() { }

            public void Reset() => throw new NotImplementedException();
        }
    }
}
