﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace Exercise_1C
{
    [TestFixture]
    public class WavelengthsSamplingTests
    {
        [Test]
        public void ValidInputs_ExpectedValues()
        {
            var min = 0;
            var max = 12;
            var amount = 9;

            var expected = new[] { 0.0, 1.5, 3.0, 4.5, 6.0, 7.5, 9.0, 10.5, 12.0 };

            var sut = CreateSut(min, max, amount);
            var actual = sut.ToArray();
            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test]
        public void AmountIsOne_ReturnMin()
        {
            var expectedMin = 1.0;
            var anyMax = 10.123;
            var expected = new[] { expectedMin };
            var sut = CreateSut(expectedMin, anyMax, 1);

            var actual = sut.ToArray();
            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test]
        public void EnumerateAgain_ProducesSameResults()
        {
            var min = 235.0;
            var max = 970.0;
            var amount = 245;
            var sut = CreateSut(min, max, amount);
            var expected = sut.ToArray();
            var actual = sut.ToArray();
            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test]
        public void ChangeAmount_Throw()
        {
            var min = 0;
            var max = 10;
            var amount = 2;

            var sut = CreateWavelengthsSampling(min, max, amount);
            Assert.IsInstanceOf<IEnumerable<double>>(sut, "WavelengthsSampling doesn't implement interface IEnumerable<double>");
            using (var enumerator = (sut as IEnumerable<double>).GetEnumerator())
            {
                var moved = enumerator.MoveNext();
                Assert.True(moved);
                var value = enumerator.Current;
                sut.ChangeAmount(amount + 1);
                Assert.Throws<InvalidOperationException>(() => enumerator.MoveNext());
            }
        }

        private IEnumerable<double> CreateSut(double min, double max, int amount)
        {
            var sut = CreateWavelengthsSampling(min, max, amount);
            Assert.IsInstanceOf<IEnumerable<double>>(sut, "WavelengthsSampling doesn't implement interface IEnumerable<double>");
            return (IEnumerable<double>)sut;
        }

        private WavelengthsSampling CreateWavelengthsSampling(double min, double max, int amount)
        {
            return new WavelengthsSampling(min, max, amount);
        }
    }
}
