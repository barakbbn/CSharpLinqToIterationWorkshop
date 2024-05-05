using System;
using System.Collections;
using System.Collections.Generic;

namespace Exercise_1D
{
    public class Program
    {
        static void Main(string[] args) { }
    }

    // TODO: Implement IEnumerable<T>
    public class DistinctUntilChanged<T>
    {
        private IEqualityComparer<T> _comparer = EqualityComparer<T>.Default;

        public DistinctUntilChanged(IEnumerable<T> source)
        {
            // TODO: You'll need the 'source' parameter to work on
            throw new NotImplementedException();
        }

        public DistinctUntilChanged(IEnumerable<T> source, IEqualityComparer<T> comparer)
        {
            // BONUS: support custom equality comparer
            throw new NotImplementedException();
        }
    }
    // TODO: Create class DistinctUntilChangedEnumerator that implements IEnumerator<T>
}
