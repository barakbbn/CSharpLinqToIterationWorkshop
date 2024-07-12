using System;
using System.Collections;
using System.Collections.Generic;

namespace Demo2
{
    // Iterate strings and return only those which are numeric
    public class OnlyNumbers : IEnumerable<int>
    {
        private readonly IEnumerable<string> _source;

        public OnlyNumbers(IEnumerable<string> source)
        {
            _source = source;
        }

        public IEnumerator<int> GetEnumerator() => new OnlyNumbersEnumerator(this);

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        private class OnlyNumbersEnumerator : IEnumerator<int>
        {
            private OnlyNumbers _owner;
            private IEnumerator<string> _sourceEnumerator;

            public OnlyNumbersEnumerator(OnlyNumbers owner)
            {
                _sourceEnumerator = owner._source.GetEnumerator();
            }

            public int Current { get; private set; }

            object IEnumerator.Current => Current;

            public void Dispose()
            {
                _sourceEnumerator.Dispose();
            }

            public bool MoveNext()
            {
                try
                {
                    while (_sourceEnumerator.MoveNext())
                    {
                        var current = _sourceEnumerator.Current;
                        int number;
                        if (int.TryParse(current, out number))
                        {
                            Current = number;
                            return true;
                        }
                    }

                    Dispose();
                    return false;

                }
                catch
                {
                    Dispose();
                    throw;
                }
            }

            public void Reset() => throw new NotImplementedException();
        }
    }
}
