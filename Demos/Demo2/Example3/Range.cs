using System;
using System.Collections;
using System.Collections.Generic;

namespace Demo2
{
    class Range : IEnumerable<int>
    {
        public Range(int start, int count)
        {
            Start = start;
            Count = count;
        }

        private int Start { get; }
        private int Count { get; }

        public IEnumerator<int> GetEnumerator() => new RangeEnumerator(this);

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();


        class RangeEnumerator : IEnumerator<int>
        {
            private readonly Range _owner;

            public RangeEnumerator(Range owner)
            {
                _owner = owner;
                Current = _owner.Start - 1;
            }

            public bool MoveNext()
            {
                Current++;
                bool moveNextOk = (Current - _owner.Start) < _owner.Count;
                return moveNextOk;
            }

            public int Current { get; private set; }

            object IEnumerator.Current => Current;

            public void Dispose()
            {
            }

            public void Reset() => throw new NotImplementedException();
        }
    }
}