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

    // ==========
    // Exercise 1
    // ==========

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

    // ==========
    // Exercise 2
    // ==========

    public class DebugEnumerable<T> : IEnumerable<T>
    {
        private readonly IEnumerable<T> _source;

        public DebugEnumerable(IEnumerable<T> source)
        {
            Console.WriteLine("DebugEnumerable C'tor");
            _source = source;
        }

        public IEnumerator<T> GetEnumerator()
        {
            Console.WriteLine("DebugEnumerable.GetEnumerator()");
            return new DebugEnumerator(this);
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        private class DebugEnumerator : IEnumerator<T>
        {
            private IEnumerator<T> _sourceEnumerator;
            private T _current;

            public DebugEnumerator(DebugEnumerable<T> owner)
            {
                Console.WriteLine("DebugEnumerator C'tor");
                _sourceEnumerator = owner._source.GetEnumerator();
            }

            public T Current
            {
                get
                {
                    Console.WriteLine("DebugEnumerable.Current");
                    return _sourceEnumerator.Current;
                }
            }

            object IEnumerator.Current => Current;

            public void Dispose()
            {
                _sourceEnumerator.Dispose();
            }

            public bool MoveNext()
            {
                var hasNextItem = _sourceEnumerator.MoveNext();
                Console.WriteLine($"DebugEnumerable.MoveNext() -> {hasNextItem}");
                return hasNextItem;
            }

            public void Reset() => throw new NotImplementedException();
        }
    }
}
