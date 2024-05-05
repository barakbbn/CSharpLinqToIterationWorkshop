using System;
using System.Collections;
using System.Collections.Generic;

namespace Demo5
{
    class RandomSequence : IEnumerable<int>
    {
        public IEnumerator<int> GetEnumerator() => new RandomEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        class RandomEnumerator : IEnumerator<int>
        {
            private Random _random = new Random();

            public bool MoveNext()
            {
                Current = _random.Next();
                return true;
            }

            public void Reset() => throw new NotImplementedException();

            public int Current { get; private set; }

            object IEnumerator.Current => Current;

            public void Dispose()
            {
            }
        }
    }
}