using System;
using System.Collections;
using System.Collections.Generic;

namespace Demo2
{
    public class DumbEnumerable<T> : IEnumerable<T>
    {
        private readonly Func<IEnumerator<T>> _getEnumerator;

        public DumbEnumerable(Func<IEnumerator<T>> getEnumerator)
        {
            _getEnumerator = getEnumerator;
        }

        public IEnumerator<T> GetEnumerator()
        {
            Console.WriteLine("DumbEnumerable.GetEnumerator");
            return _getEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}