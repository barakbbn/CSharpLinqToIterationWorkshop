using System;
using System.Collections;
using System.Collections.Generic;

namespace Exercise_1C
{
    public class Program
    {
        static void Main(string[] args) { }
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
            {
                throw new ArgumentOutOfRangeException(nameof(min), "Value cannot be negative");
            }
            if (max < 0.0)
            {
                throw new ArgumentOutOfRangeException(nameof(max), "Value cannot be negative");
            }
            if (max < min)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(max),
                    "Maximum cannot be less than Minimum"
                );
            }
            if (amount < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(amount), "Amount must be positive");
            }
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
                {
                    throw new InvalidOperationException(
                        "WavelengthsSampling was changed while iterating it"
                    );
                }
                if (_nextIndex == _owner._amount)
                {
                    return false;
                }
                if (_nextIndex == 0)
                {
                    Current = _owner._min;
                }
                else if (_nextIndex == _owner._amount - 1)
                {
                    Current = _owner._max;
                }
                else
                {
                    Current = _owner._min + _nextIndex * _nextStep;
                }
                _nextIndex++;

                return true;
            }

            public void Dispose()
            {
            }

            public void Reset() => throw new NotImplementedException();
        }
    }
}
