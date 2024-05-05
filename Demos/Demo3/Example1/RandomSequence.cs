using System;
using System.Collections;
using System.Collections.Generic;

namespace Demo3
{
    class RandomSequence : IEnumerable<int>
    {
        public IEnumerator<int> GetEnumerator()
        {
            return new RandomEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        class RandomEnumerator : IEnumerator<int>
        {
            private Random _random = new Random();

            public bool MoveNext()
            {
                Current = _random.Next();
                return true;
            }

            public void Reset()
            {
                _random = new Random();
            }

            public int Current { get; private set; }

            object IEnumerator.Current => Current;

            public void Dispose()
            {
            }
        }
    }
}