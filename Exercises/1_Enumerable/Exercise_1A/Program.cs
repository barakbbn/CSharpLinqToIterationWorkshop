using System;
using System.Collections;
using System.Collections.Generic;

namespace Exercise_1A
{
    public class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Exercise 1A");
        }
    }

    public class CountdownSequence : IEnumerable<int>
    {
        private int _count;

        public CountdownSequence(int count)
        {
            _count = count;
        }

        public IEnumerator<int> GetEnumerator()
        {
            var values = new List<int>();
            for (int i = _count; i >= 0; i--)
            {
                values.Add(i);
            }
            return values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
