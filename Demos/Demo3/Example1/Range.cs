using System;
using System.Collections;
using System.Collections.Generic;

namespace Demo3
{
    public class Range : IEnumerable<int>
    {
        private int _start;
        private int _count;

        public Range(int start, int count)
        {
            _start = start;
            _count = count;
        }

        public IEnumerator<int> GetEnumerator()
        {
            // No need to implement IEnumerator !!!
            for (int i = 0; i < _count; i++)
            {
                yield return _start + i;
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public IEnumerable<int> Reversed
        {
            get
            {
                for (int i = _count - 1; i >= 0; i--)
                {
                    yield return _start + i;
                }
            }
        }
    }
}
