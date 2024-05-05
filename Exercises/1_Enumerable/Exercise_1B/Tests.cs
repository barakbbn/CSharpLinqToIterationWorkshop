using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using TestsUtils;

namespace Exercise_1B
{
    public abstract class FizzBuzzSequenceTestsBase
    {
        protected abstract FizzBuzzSequence CreateFizzBuzzSequence(IEnumerable<int> source);

        [Test]
        public void DivisibleBy3_ReturnsFizz()
        {
            var input = new List<int>() { 3, 6, 9 };
            var expected = new List<string>() { "Fizz", "Fizz", "Fizz" };

            var sut = CreateFizzBuzzSequence(input);
            var actual = (sut as IEnumerable<string>).ToArray();

            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test]
        public void DivisibleBy5_ReturnsFizz()
        {
            var input = new List<int>() { 5, 10, 20 };
            var expected = new List<string>() { "Buzz", "Buzz", "Buzz" };

            var sut = CreateFizzBuzzSequence(input);
            var actual = (sut as IEnumerable<string>).ToArray();

            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test]
        public void NonDivisibleNumbers_ReturnsNumberAsString()
        {
            var input = new List<int>() { 1, 2, 4 };
            var expected = new List<string>() { "1", "2", "4" };

            var sut = CreateFizzBuzzSequence(input);
            var actual = (sut as IEnumerable<string>).ToArray();

            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test]
        public void EdgeCase_Zero_ReturnZeroAsString()
        {
            var input = new List<int>() { 0 };
            var expected = new List<string>() { "0" };

            var sut = CreateFizzBuzzSequence(input);
            var actual = (sut as IEnumerable<string>).ToArray();

            Warn.Unless(actual, Is.EqualTo(expected));
        }

        [Test]
        public void EmptyInput_ProducesNoValues()
        {
            var input = Enumerable.Empty<int>();
            var sut = CreateFizzBuzzSequence(input);
            var actual = (sut as IEnumerable<string>).ToArray();
            Assert.IsEmpty(actual);
        }

        [Test]
        public void Enumerate_EnumerateAgain_ProducesSameResults()
        {
            var input = new List<int> { 1, 2, 3, 4, 5, 6, 7, 9, 10, 15, 20, 21 };
            var sut = CreateFizzBuzzSequence(input);
            var expected = (sut as IEnumerable<string>).ToArray();
            var actual = (sut as IEnumerable<string>).ToArray();
            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test]
        public void Enumerate_InputUnchanged()
        {
            var input = new List<int> { 1, 3, 5, 15 };
            var expected = new List<int>(input);
            var sut = CreateFizzBuzzSequence(input);
            var forcedEnumeration = (sut as IEnumerable<string>).ToArray();
            Assert.That(input, Is.EqualTo(expected));
        }
    }

    [TestFixture]
    public class FizzBuzzSequenceTests : FizzBuzzSequenceTestsBase
    {
        [Test]
        public void DivisibleBy3And5_ReturnsFizzBuzz()
        {
            var input = new List<int>() { 15, 30, 45 };
            var expected = new List<string>() { "FizzBuzz", "FizzBuzz", "FizzBuzz" };

            var sut = CreateFizzBuzzSequence(input);
            var actual = (sut as IEnumerable<string>).ToArray();

            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test]
        public void ChangeInput_ProducesCorrectResultsForChangedInput()
        {
            var input = new List<int> { 1, 3, 5, 15 };
            var sut = CreateFizzBuzzSequence(input);
            var forcedEnumeration = (sut as IEnumerable<string>).ToArray();
            input.RemoveAt(0);
            input.Add(7);
            var expected = new List<string>() { "Fizz", "Buzz", "FizzBuzz", "7" };
            var actual = (sut as IEnumerable<string>).ToArray();
            Assert.That(actual, Is.EqualTo(expected));
        }

        protected override FizzBuzzSequence CreateFizzBuzzSequence(IEnumerable<int> source)
        {
            return (FizzBuzzSequence)Activator.CreateInstance(typeof(FizzBuzzSequence), source);
        }
    }

    [TestFixture]
    public class FizzBuzzSequenceBonusTests : FizzBuzzSequenceTestsBase
    {
        [Test]
        public void DivisibleBy3And5_ReturnsFizzBuzz()
        {
            var input = new List<int>() { 15, 30, 45 };
            var expected = new List<string>() { "Fizz", "Buzz", "Fizz", "Buzz", "Fizz", "Buzz" };

            var sut = CreateFizzBuzzSequence(input);
            var actual = (sut as IEnumerable<string>).ToArray();

            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test]
        public void ChangeInput_ProducesCorrectResultsForChangedInput()
        {
            var input = new List<int> { 1, 3, 5, 15 };
            var sut = CreateFizzBuzzSequence(input);
            var forcedEnumeration = (sut as IEnumerable<string>).ToArray();
            input.RemoveAt(0);
            input.Add(7);
            var expected = new List<string>() { "Fizz", "Buzz", "Fizz", "Buzz", "7" };
            var actual = (sut as IEnumerable<string>).ToArray();
            Assert.That(actual, Is.EqualTo(expected));
        }

        protected override FizzBuzzSequence CreateFizzBuzzSequence(IEnumerable<int> source)
        {
            return (FizzBuzzSequence)
                Activator.CreateInstance(typeof(FizzBuzzSequence), source, true);
        }
    }
}
