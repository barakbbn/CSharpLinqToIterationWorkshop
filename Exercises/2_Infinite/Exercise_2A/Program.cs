using System;
using System.Collections;
using System.Collections.Generic;

namespace Exercise_2A
{
    public class Program
    {
        static void Main(string[] args) { }
    }

    public class EndlessCycle<T> : IEnumerable<T>
    {
        private IEnumerable<T> _source;

        public EndlessCycle(IEnumerable<T> source)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }
            _source = source;
        }

        public IEnumerator<T> GetEnumerator()
        {
            return new EndlessCycleEnumerator(this);
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        private class EndlessCycleEnumerator : IEnumerator<T>
        {
            private IEnumerator<T> _sourceEnumerator;
            private EndlessCycle<T> _owner;
            private int _state = 0; // State machine: -1 - completed/not more iteration , 0 - initial , 1 - move to next item

            public EndlessCycleEnumerator(EndlessCycle<T> owner)
            {
                _owner = owner;
            }

            public T Current => _sourceEnumerator != null ? _sourceEnumerator.Current : default(T);

            object IEnumerator.Current => Current;

            public bool MoveNext()
            {
                try
                {
                    switch (_state)
                    {
                        case 0: // Initial
                            _sourceEnumerator = _owner._source.GetEnumerator();
                            _state = 1;
                            var hasNext1 = _sourceEnumerator.MoveNext();
                            if (!hasNext1) // Is Empty sequence? then can't loop it endlessly
                            {
                                Dispose();
                                return false;
                            }

                            return true;

                        case 1: // Move to next item
                            var hasNext2 = _sourceEnumerator.MoveNext();
                            if (hasNext2)
                            {
                                return true;
                            }

                            _state = 0; // End of sequence, back to start
                            _sourceEnumerator.Dispose();
                            _sourceEnumerator = null;
                            goto case 0;

                        default:
                            return false;
                    }
                }
                catch
                {
                    Dispose();
                    throw;
                }
            }

            public void Dispose()
            {
                _state = -1; // No more iteration
                _sourceEnumerator?.Dispose();
                _sourceEnumerator = null;
            }

            public void Reset() => throw new NotImplementedException();
        }
    }
}
