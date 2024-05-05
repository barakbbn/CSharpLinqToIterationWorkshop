using System;
using System.Collections;
using System.Collections.Generic;

namespace Exercise_1B
{
    public class Program
    {
        static void Main(string[] args) { }
    }

    public class FizzBuzzSequence : IEnumerable<string>
    {
        private IEnumerable<int> _source;
        private bool _bonus;

        public FizzBuzzSequence(IEnumerable<int> source)
        {
            _source = source;
        }

        public FizzBuzzSequence(IEnumerable<int> source, bool bonus)
            : this(source)
        {
            _bonus = bonus;
        }

        public IEnumerator<string> GetEnumerator()
        {
            return _bonus
                ? (IEnumerator<string>)new FizzBuzzBonusEnumerator(_source)
                : new FizzBuzzEnumerator(_source);
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

    public class FizzBuzzEnumerator : IEnumerator<string>
    {
        private List<string> _results = new List<string>();
        private int _currentIndex = -1;

        public FizzBuzzEnumerator(IEnumerable<int> source)
        {
            foreach (var n in source)
            {
                string result = null;
                if (n != 0 && n % 3 == 0)
                {
                    result = "Fizz";
                }
                if (n != 0 && n % 5 == 0)
                {
                    result += "Buzz";
                }

                result = result ?? n.ToString();
                _results.Add(result);
            }
        }

        public string Current { get; private set; }

        object IEnumerator.Current => Current;

        public bool MoveNext()
        {
            if (_currentIndex + 1 == _results.Count)
            {
                return false;
            }
            _currentIndex++;
            Current = _results[_currentIndex];
            return true;
        }

        public void Dispose() { }

        public void Reset() => throw new NotImplementedException();
    }

    public class FizzBuzzBonusEnumerator : IEnumerator<string>
    {
        private List<string> _results = new List<string>();
        private int _currentIndex = -1;

        public FizzBuzzBonusEnumerator(IEnumerable<int> source)
        {
            foreach (var n in source)
            {
                bool isFizz = n != 0 && n % 3 == 0;
                bool isBuzz = n != 0 && n % 5 == 0;
                if (isFizz)
                {
                    _results.Add("Fizz");
                }
                if (isBuzz)
                {
                    _results.Add("Buzz");
                }
                if (!isFizz && !isBuzz)
                {
                    _results.Add(n.ToString());
                }
            }
        }

        public string Current { get; private set; }

        object IEnumerator.Current => Current;

        public bool MoveNext()
        {
            if (_currentIndex + 1 == _results.Count)
            {
                return false;
            }
            _currentIndex++;
            Current = _results[_currentIndex];
            return true;
        }

        public void Dispose() { }

        public void Reset() => throw new NotImplementedException();
    }
}
