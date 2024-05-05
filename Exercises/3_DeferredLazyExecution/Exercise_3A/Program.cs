using System;
using System.Collections;
using System.Collections.Generic;

namespace Exercise_3A
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
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }
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

        private class FizzBuzzEnumerator : IEnumerator<string>
        {
            private IEnumerator<int> _enumerator;

            public FizzBuzzEnumerator(IEnumerable<int> source)
            {
                _enumerator = source.GetEnumerator();
            }

            public string Current { get; private set; }

            object IEnumerator.Current => Current;

            public bool MoveNext()
            {
                try
                {
                    if (!(_enumerator?.MoveNext() ?? false))
                    {
                        Dispose();
                        return false;
                    }

                    var value = _enumerator.Current;

                    string current = null;
                    if (value != 0 && value % 3 == 0)
                    {
                        current = "Fizz";
                    }
                    if (value != 0 && value % 5 == 0)
                    {
                        current += "Buzz";
                    }

                    Current = current ?? value.ToString();
                    return true;
                }
                catch
                {
                    Dispose();
                    throw;
                }
            }

            public void Dispose()
            {
                _enumerator?.Dispose();
                _enumerator = null;
            }

            public void Reset() => throw new NotImplementedException();
        }

        private class FizzBuzzBonusEnumerator : IEnumerator<string>
        {
            private IEnumerator<int> _enumerator;
            private bool _isFizzBuzz;

            public FizzBuzzBonusEnumerator(IEnumerable<int> source)
            {
                _enumerator = source.GetEnumerator();
            }

            public string Current { get; private set; }

            object IEnumerator.Current => Current;

            public bool MoveNext()
            {
                try
                {
                    if (_isFizzBuzz)
                    {
                        _isFizzBuzz = false;
                        Current = "Buzz";
                        return true;
                    }
                    if (!(_enumerator?.MoveNext() ?? false))
                    {
                        Dispose();
                        return false;
                    }
                    var value = _enumerator.Current;

                    bool isFizz = value != 0 && value % 3 == 0;
                    bool isBuzz = value != 0 && value % 5 == 0;
                    _isFizzBuzz = isFizz && isBuzz;

                    Current = isFizz
                        ? "Fizz"
                        : isBuzz
                            ? "Buzz"
                            : value.ToString();
                    return true;
                }
                catch
                {
                    Dispose();
                    throw;
                }
            }

            public void Dispose()
            {
                _enumerator?.Dispose();
                _enumerator = null;
            }

            public void Reset() => throw new NotImplementedException();
        }
    }

    public class WavelengthsSampling : IEnumerable<double>
    {
        private long _version;
        private double _min;
        private double _max;
        private int _amount;

        public WavelengthsSampling(double min, double max, int amount)
        {
            if (min < 0.0)
                throw new ArgumentOutOfRangeException(nameof(min), "Value cannot be negative");
            if (max < 0.0)
                throw new ArgumentOutOfRangeException(nameof(max), "Value cannot be negative");
            if (max < min)
                throw new ArgumentOutOfRangeException(
                    nameof(max),
                    "Maximun cannot be less than Minimum"
                );
            if (amount < 1)
                throw new ArgumentOutOfRangeException(nameof(amount), "Amount must be positive");

            _min = min;
            _max = max;
            _amount = amount;
        }

        public bool IsWithinRange(double wavelength)
        {
            return wavelength >= _min && wavelength <= _max;
        }

        public void ChangeAmount(int amount)
        {
            _amount = amount;
            _version++;
        }

        public IEnumerator<double> GetEnumerator()
        {
            return new WavelengthsSamplingEnumerator(this);
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        private class WavelengthsSamplingEnumerator : IEnumerator<double>
        {
            private readonly WavelengthsSampling _owner;
            private readonly long _version;
            private int _nextIndex;
            private double _nextStep;

            public WavelengthsSamplingEnumerator(WavelengthsSampling owner)
            {
                _owner = owner;
                _version = owner._version;
                _nextStep = (owner._max - owner._min) / Math.Max(owner._amount - 1, 1);
            }

            public double Current { get; private set; }

            object IEnumerator.Current => Current;

            public bool MoveNext()
            {
                if (_version != _owner._version)
                    throw new InvalidOperationException(
                        "WavelengthsSampling was changed while iterating it"
                    );
                if (_nextIndex == _owner._amount)
                    return false;

                Current =
                    (_nextIndex == 0)
                        ? _owner._min
                        : (_nextIndex == _owner._amount - 1)
                            ? _owner._max
                            : (_owner._min + _nextIndex * _nextStep);

                _nextIndex++;

                return true;
            }

            public void Reset()
            {
                _nextIndex = 0;
            }

            public void Dispose()
            {
                _nextIndex = 0;
            }
        }
    }

    public class DistinctUntilChanged<T> : IEnumerable<T>
    {
        private readonly IEnumerable<T> _source;
        private readonly IEqualityComparer<T> _comparer;

        public DistinctUntilChanged(IEnumerable<T> source)
            : this(source, null) { }

        public DistinctUntilChanged(IEnumerable<T> source, IEqualityComparer<T> comparer)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }
            _source = source;
            _comparer = comparer ?? EqualityComparer<T>.Default;
        }

        public IEnumerator<T> GetEnumerator()
        {
            return new DistinctUntilChangedEnumerator(this);
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        private class DistinctUntilChangedEnumerator : IEnumerator<T>
        {
            private readonly DistinctUntilChanged<T> _owner;
            private IEnumerator<T> _sourceEnumerator;
            private bool _hasPreviousValue;

            public DistinctUntilChangedEnumerator(DistinctUntilChanged<T> owner)
            {
                _owner = owner;
                _sourceEnumerator = owner._source.GetEnumerator();
            }

            public T Current { get; private set; }

            object IEnumerator.Current => Current;

            public bool MoveNext()
            {
                if (_sourceEnumerator == null)
                {
                    return false;
                }

                try
                {
                    bool hasNext;
                    while (hasNext = _sourceEnumerator.MoveNext())
                    {
                        var newValue = _sourceEnumerator.Current;
                        if (!_hasPreviousValue)
                        {
                            // no previous value to compare to, so we take that value
                            Current = newValue;
                            _hasPreviousValue = true;
                            return true;
                        }

                        var equalsToPreviousValue = _owner._comparer.Equals(Current, newValue);

                        if (!equalsToPreviousValue)
                        {
                            Current = _sourceEnumerator.Current;
                            return true;
                        }
                    }
                }
                catch
                {
                    Dispose();
                    throw;
                }

                Dispose();
                return false;
            }

            public void Dispose()
            {
                _sourceEnumerator?.Dispose();
                _sourceEnumerator = null;
            }

            public void Reset() => throw new NotImplementedException();
        }
    }
}
