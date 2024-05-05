using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace Exercise_1A
{
    [TestFixture]
    public class CountdownSequenceTests
    {
        [Test]
        public void CountIsNegative_ReturnEmptyResults()
        {
            var sut = CreateSut(-1) as IEnumerable<int>;
            var actual = new List<int>();
            foreach (var value in sut)
            {
                actual.Add(value);
            }
            Assert.IsEmpty(actual);
        }

        [Test]
        public void CountIsZero_ReturnedSequenceContainsOnlyZero()
        {
            var sut = CreateSut(0) as IEnumerable<int>;
            var actual = new List<int>();
            var expected = new[] { 0 };
            foreach (var value in sut)
            {
                actual.Add(value);
            }
            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test]
        public void CountIsPositive_ReturnsSequenceWithNumbersFromCountTillZero()
        {
            var count = 5;
            var sut = CreateSut(count) as IEnumerable<int>;
            var actual = new List<int>();
            var expected = new List<int>();
            while (count >= 0)
            {
                expected.Add(count--);
            }

            foreach (var value in sut)
            {
                actual.Add(value);
            }
            Assert.That(actual, Is.EqualTo(expected));
        }

        public void Enumerate_EnumerateAgain_ProducesSameResults()
        {
            var sut = CreateSut(5);
            var expected = (sut as IEnumerable<int>).ToArray();
            var actual = (sut as IEnumerable<int>).ToArray();
            Assert.That(actual, Is.EqualTo(expected));
        }

        private CountdownSequence CreateSut(int count)
        {
            return new CountdownSequence(count);
        }
    }
}
