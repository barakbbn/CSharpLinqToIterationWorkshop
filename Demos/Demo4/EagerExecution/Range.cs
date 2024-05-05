using System.Collections;
using System.Collections.Generic;

namespace Demo4.EagerExecution
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

        public IEnumerator<int> GetEnumerator()
        {
            var range = new List<int>();
            for (int i = Start; i < Start + Count; i++)
            {
                range.Add(i);
            }
            return range.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public IEnumerable<int> ReverseOrder
        { 
            get
            {
                var reverseRange = new List<int>();
                for (int i = Start + Count - 1; i > Start; i--)
                {
                    reverseRange.Add(i);
                }

                return reverseRange;
            } 
        }
    }
}