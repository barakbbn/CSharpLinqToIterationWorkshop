using System;
using System.Collections;
using System.Collections.Generic;

namespace Demo5.Example1
{
    public class Range : IEnumerable<int>
    {
        public Range(int start, int count)
        {
            Start = start;
            Count = count;
        }

        private int Start { get; }
        private int Count { get; }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public IEnumerator<int> GetEnumerator()
        {
            for (int value = Start; value <(Start + Count); value++)
            {
                yield return value;
            }
        }

        public IEnumerable<int> ReverseOrder 
        { 
            get
            {
                for (int value = Start + Count - 1; value >= Start; value--)
                {
                    yield return value;
                }
            } 
        }
    }
}