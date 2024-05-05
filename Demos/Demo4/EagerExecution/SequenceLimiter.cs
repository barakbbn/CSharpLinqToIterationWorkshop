using System.Collections;
using System.Collections.Generic;

namespace Demo4.EagerExecution
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
            var limitedItems = new List<int>();
            int count = 0;
            foreach (int item in _sourceSequence)
            {
                if (count++ >= _limit)
                {
                    break;
                }
                limitedItems.Add(item);
            }
            return limitedItems.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}