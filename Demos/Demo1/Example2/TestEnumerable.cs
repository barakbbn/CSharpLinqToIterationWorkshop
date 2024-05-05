using System;
using System.Collections;
using System.Collections.Generic;

namespace Demo1
{
    /// <summary>
    /// This is not the correct way to implement IEnumerator, this is just a demo how foreach works
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class TestEnumerable : IEnumerable<string>, IEnumerator<string>
    {
        private int _currentIndex = -1;
        private string[] _values = new string[] { "1", "1", "Testing" };

        public IEnumerator<string> GetEnumerator()
        {
            Console.WriteLine("IEnumerable.GetEnumerator()");
            return new TestEnumerable();
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public bool MoveNext()
        {
            bool canMoveNext = _currentIndex < _values.Length - 1;
            
            Console.WriteLine("IEnumerator.MoveNext() - {0}", canMoveNext);
            if (canMoveNext) _currentIndex++;

            return canMoveNext;
        }

        public string Current
        {
            get
            {
                Console.WriteLine("IEnumerator.Current");
                return _values[_currentIndex];
            }
        }

        object IEnumerator.Current => Current;

        public void Dispose() => Console.WriteLine("IEnumerator.Dispose()");
        public void Reset() => Console.WriteLine("IEnumerator.Reset()");
    }
}