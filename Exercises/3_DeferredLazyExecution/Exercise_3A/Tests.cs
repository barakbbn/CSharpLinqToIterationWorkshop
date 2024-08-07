﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using TestsUtils;

namespace Exercise_3A
{
    [TestFixture, Timeout(5000)]
    public class WavelengthsSamplingTests
    {
        [Test]
        public void Create_GetEnumerator_InputNotIterated()
        {
            // Using huge range of values
            var sut = CreateWavelengthsSampling(1, int.MaxValue, int.MaxValue);

            // If internally values prepared in advance when calling GetEnumerator()
            // The thread will stuck and this Test will fail after the defined timeout
            using (var enumerator = sut.GetEnumerator())
            {
                Assert.Pass();
            }
        }

        public void MoveNext_CalledOnce_InternalDataNotPreparedInAdvance()
        {
            // Using huge range of values
            var sut = CreateWavelengthsSampling(1, int.MaxValue, int.MaxValue);
            using (var enumerator = sut.GetEnumerator())
            {
                // If internally values prepared in advance when calling MoveNext()
                // The thread will stuck and this Test will fail after the defined timeout
                enumerator.MoveNext();
                Assert.Pass();
            }
        }

        private IEnumerable<double> CreateWavelengthsSampling(double min, double max, int amount)
        {
            var sut = (WavelengthsSampling)
                Activator.CreateInstance(typeof(WavelengthsSampling), min, max, amount);
            Assert.IsInstanceOf<IEnumerable<double>>(
                sut,
                "WavelengthsSampling doesn't implement interface IEnumerable<double>"
            );

            return (IEnumerable<double>)sut;
        }
    }

    public abstract class FizzBuzzSequenceTestsBase : CommonDeferredOnInputEnumerableTests<int>
    {
        protected abstract IEnumerable<string> CreateFizzBuzzSequence(IEnumerable<int> source);

        [Test]
        public void EdgeCase_Zero_ReturnZeroAsString()
        {
            var input = new List<int>() { 0 };
            var expected = new List<string>() { "0" };

            var sut = CreateFizzBuzzSequence(input);
            var actual = sut.ToArray();

            Warn.Unless(actual, Is.EqualTo(expected));
        }

        [Test]
        public void EmptyInput_ProducesNoValues()
        {
            var input = Enumerable.Empty<int>();
            var sut = CreateFizzBuzzSequence(input);
            var actual = sut.ToArray();
            Assert.IsEmpty(actual);
        }

        [Test]
        public void Enumerate_EnumerateAgain_ProducesSameResults()
        {
            var input = new List<int> { 1, 2, 3, 4, 5, 6, 7, 9, 10, 15, 20, 21 };
            var sut = CreateFizzBuzzSequence(input);
            var expected = sut.ToArray();
            var actual = sut.ToArray();
            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test]
        public void MoveNext_CalledOnce_InputNotFullyIterated()
        {
            var input = TestableEnumerable.Range(3);
            var sut = CreateFizzBuzzSequence(input);
            using (var enumerator = sut.GetEnumerator())
            {
                enumerator.MoveNext();
                Assert.False(input.MoveNextCompleted);
            }
        }

        [Test]
        public void Enumerate_MoveNextReturnedFalse_InputEnumeratorDisposed()
        {
            var input = TestableEnumerable.Range(3);
            var sut = CreateFizzBuzzSequence(input);
            using (var enumerator = sut.GetEnumerator())
            {
                while (enumerator.MoveNext())
                    ;
                Assert.NotZero(
                    input.DisposedCount,
                    "Expected 'input' internal IEnumerator to be disposed when MoveNext() returns false"
                );
            }
        }

        protected override object SutDeferredAction(IEnumerable<int> input)
        {
            return CreateFizzBuzzSequence(input);
        }

        protected override IEnumerable<int> CreateSimpleInputSequence()
        {
            return new List<int> { 15, 2, 3, 4, 5 };
        }
    }

    [TestFixture]
    public class FizzBuzzSequenceTests : FizzBuzzSequenceTestsBase
    {
        [Test]
        public void MixedInput_ReturnsExpectedValues()
        {
            var input = new List<int>()
            {
                1,
                2,
                3,
                4,
                5,
                6,
                7,
                8,
                9,
                10,
                11,
                12,
                13,
                14,
                15,
                16,
                17,
                18,
                19,
                20
            };
            var expected = new List<string>()
            {
                "1",
                "2",
                "Fizz",
                "4",
                "Buzz",
                "Fizz",
                "7",
                "8",
                "Fizz",
                "Buzz",
                "11",
                "Fizz",
                "13",
                "14",
                "FizzBuzz",
                "16",
                "17",
                "Fizz",
                "19",
                "Buzz"
            };

            var sut = CreateFizzBuzzSequence(input);
            var actual = sut.ToArray();

            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test]
        public void ChangeInput_ProducesCorrectResultsForChangedInput()
        {
            var input = new List<int> { 1, 3, 5, 15 };
            var sut = CreateFizzBuzzSequence(input);
            var forcedEnumeration = sut.ToArray();
            input.RemoveAt(0);
            input.Add(7);
            var expected = new List<string>() { "Fizz", "Buzz", "FizzBuzz", "7" };
            var actual = sut.ToArray();
            Assert.That(actual, Is.EqualTo(expected));
        }

        protected override IEnumerable<string> CreateFizzBuzzSequence(IEnumerable<int> source)
        {
            var sut = (FizzBuzzSequence)Activator.CreateInstance(typeof(FizzBuzzSequence), source);
            Assert.IsInstanceOf<IEnumerable<string>>(
                sut,
                "FizzBuzzSequence doesn't implement interface IEnumerable<string>"
            );
            return (IEnumerable<string>)sut;
        }
    }

    [TestFixture]
    public class FizzBuzzSequenceBonusTests : FizzBuzzSequenceTestsBase
    {
        [Test]
        public void MixedInput_ReturnsExpectedValues()
        {
            var input = new List<int>()
            {
                1,
                2,
                3,
                4,
                5,
                6,
                7,
                8,
                9,
                10,
                11,
                12,
                13,
                14,
                15,
                16,
                17,
                18,
                19,
                20
            };
            var expected = new List<string>()
            {
                "1",
                "2",
                "Fizz",
                "4",
                "Buzz",
                "Fizz",
                "7",
                "8",
                "Fizz",
                "Buzz",
                "11",
                "Fizz",
                "13",
                "14",
                "Fizz",
                "Buzz",
                "16",
                "17",
                "Fizz",
                "19",
                "Buzz"
            };

            var sut = CreateFizzBuzzSequence(input);
            var actual = sut.ToArray();

            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test]
        public void ChangeInput_ProducesCorrectResultsForChangedInput()
        {
            var input = new List<int> { 1, 3, 5, 15 };
            var sut = CreateFizzBuzzSequence(input);
            var forcedEnumeration = sut.ToArray();
            input.RemoveAt(0);
            input.Add(7);
            var expected = new List<string>() { "Fizz", "Buzz", "Fizz", "Buzz", "7" };
            var actual = sut.ToArray();
            Assert.That(actual, Is.EqualTo(expected));
        }

        protected override IEnumerable<string> CreateFizzBuzzSequence(IEnumerable<int> source)
        {
            var sut = (FizzBuzzSequence)
                Activator.CreateInstance(typeof(FizzBuzzSequence), source, true);
            Assert.IsInstanceOf<IEnumerable<string>>(
                sut,
                "FizzBuzzSequence doesn't implement interface IEnumerable<string>"
            );
            return (IEnumerable<string>)sut;
        }
    }

    [TestFixture]
    public class DistinctUntilChangedTests : CommonDeferredOnInputEnumerableTests<int>
    {
        [Test]
        public void MoveNext_CalledOnce_InputNotFullyIterated()
        {
            var input = TestableEnumerable.Range(3);
            var sut = CreateDistinctUntilChanged<int>(input);
            using (var enumerator = sut.GetEnumerator())
            {
                enumerator.MoveNext();
                Assert.False(input.MoveNextCompleted);
            }
        }

        [Test]
        public void Enumerate_MoveNextReturnedFalse_InputEnumeratorDisposed()
        {
            var input = TestableEnumerable.Range(3);
            var sut = CreateDistinctUntilChanged<int>(input);
            using (var enumerator = sut.GetEnumerator())
            {
                while (enumerator.MoveNext())
                    ;
                Assert.NotZero(
                    input.DisposedCount,
                    "Expected 'input' internal IEnumerator to be disposed when MoveNext() returns false"
                );
            }
        }

        private IEnumerable<T> CreateDistinctUntilChanged<T>(
            IEnumerable<T> input,
            IEqualityComparer<T> comparer = null
        )
        {
            var sut =
                (comparer == null)
                    ? Activator.CreateInstance(typeof(DistinctUntilChanged<T>), input)
                    : Activator.CreateInstance(typeof(DistinctUntilChanged<T>), input, comparer);

            Assert.IsInstanceOf<IEnumerable<T>>(
                sut,
                "DistinctUntilChanged doesn't implement interface IEnumerable"
            );

            return (IEnumerable<T>)sut;
        }

        protected override object SutDeferredAction(IEnumerable<int> input)
        {
            return CreateDistinctUntilChanged<int>(input);
        }

        protected override IEnumerable<int> CreateSimpleInputSequence()
        {
            return new[] { 1, 2, 3 };
        }
    }
}
