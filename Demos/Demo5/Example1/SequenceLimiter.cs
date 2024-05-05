using System;
using System.Collections;
using System.Collections.Generic;

namespace Demo5
{
    class SequenceLimiter : IEnumerable<int>
    {
        private readonly IEnumerable<int> _sourceSequence;
        private readonly int _limit;

        public SequenceLimiter(IEnumerable<int> sourceSequence, int limit)
        {
            _sourceSequence = sourceSequence;
            _limit = limit;
        }

        public IEnumerator<int> GetEnumerator()
        {
            return new SequenceLimiterEnumerator(_sourceSequence.GetEnumerator(), _limit);
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        private class SequenceLimiterEnumerator : IEnumerator<int>
        {
            private readonly IEnumerator<int> _sourceSequence;
            private readonly int _limit;
            private int _count;

            public SequenceLimiterEnumerator(IEnumerator<int> sourceSequence, int limit)
            {
                _sourceSequence = sourceSequence;
                _limit = limit;
                _count = 0;
            }

            public void Dispose() => _sourceSequence.Dispose();

            public bool MoveNext()
            {
                return _count++ < _limit && _sourceSequence.MoveNext();
            }

            public int Current => _sourceSequence.Current;

            object IEnumerator.Current => Current;

            public void Reset() => throw new NotImplementedException();
        }
    }
}