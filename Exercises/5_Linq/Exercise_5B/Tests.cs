using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using NUnit.Framework;
using TestsUtils;

namespace Exercise_5B
{
    [TestFixture]
    public class ForEachTests : CommonImmediateTests
    {
        [Test]
        public void EmptyInput_ActionNotCalled()
        {
            var input = Enumerable.Empty<int>();
            bool actionCalled = false;
            Extensions.ForEach(input, x => actionCalled = true);
            Assert.False(actionCalled);
        }

        [Test]
        public void ActionCalledForEachItem()
        {
            var input = new[] { 1, 2, 3 };
            var expected = new List<int>(input.Length);
            Extensions.ForEach(input, x => expected.Add(x));
            Assert.That(input, Is.EqualTo(expected));
        }

        [Test]
        public void NullAction_Throw()
        {
            TestHelper.ShouldWarn_WhenParameterIsNull(
                "action",
                () =>
                {
                    Extensions.ForEach(Enumerable.Empty<int>(), null as Action<int>);
                }
            );
        }

        protected override void SutImmediateAction<T>(IEnumerable<T> input)
        {
            Extensions.ForEach(input, x => { });
        }
    }

    [TestFixture]
    public class FlattenTests : CommonDeferredOnInputEnumerableTests<IEnumerable<string>>
    {
        [Test]
        public void EmptyInput_EmptyResult()
        {
            var input = Enumerable.Empty<int[]>();
            var sut = Extensions.Flatten(input);
            var actual = sut.ToArray();
            Assert.IsEmpty(actual);
        }

        [Test]
        public void ReturnInnerItemsAsSingleSequence()
        {
            var input = new[] { Enumerable.Range(1, 3), Enumerable.Range(11, 3) };
            var expected = new[] { 1, 2, 3, 11, 12, 13 };
            var sut = Extensions.Flatten(input);
            var actual = sut.ToArray();
            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test]
        public void Take1_InputNotFullyIterated()
        {
            var innerInput = TestableEnumerable.Range(10);
            var input = TestableEnumerable.From(new[] { innerInput, TestableEnumerable.Range(10) });

            var sut = Extensions.Flatten(input);
            foreach (var item in sut)
            {
                break;
            }

            Assert.AreEqual(input.MoveNextCount, 1);
            Assert.AreEqual(innerInput.MoveNextCount, 1);
        }

        [Test]
        public void WithAllEmptyInnerSequences_ReturnsEmptySequence()
        {
            var input = new List<List<int>> { new List<int>(), new List<int>(), new List<int>() };

            var actual = Extensions.Flatten(input);

            Assert.IsEmpty(actual);
        }

        protected override IEnumerable<IEnumerable<string>> CreateSimpleInputSequence()
        {
            return new[]
            {
                System.Globalization.DateTimeFormatInfo.InvariantInfo.MonthNames,
                Enum.GetNames(typeof(DayOfWeek)),
            };
        }

        protected override object SutDeferredAction(IEnumerable<IEnumerable<string>> input)
        {
            return Extensions.Flatten(input);
        }
    }

    [TestFixture]
    public class ToStringTests : CommonImmediateTests
    {
        [Test]
        public void EmptyInput_ReturnsEmptyString()
        {
            var input = Enumerable.Empty<bool>();
            var actual = Extensions.ToString(input, "|");
            Assert.IsEmpty(actual);
        }

        [Test]
        public void InputWithOneItem_ReturnsStringWithOnlyThatItemAndWithoutTheSeparator()
        {
            var input = new[] { Math.PI };
            var expected = string.Join("|", input);
            var actual = Extensions.ToString(input, "|");
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void BehavesLikeStringJoin()
        {
            var input = new List<object> { 0, "", null, true, 3.14, 'a', (Name: "Me", Age: 10) };
            var separators = new[] { ",", "|", "", "\\", "XYZ", null };
            foreach (var separator in separators)
            {
                var expected = string.Join(separator, input);
                var actual = Extensions.ToString(input, separator);
                Assert.AreEqual(expected, actual);
            }
        }

        protected override void SutImmediateAction<T>(IEnumerable<T> input)
        {
            Extensions.ToString(input, "|");
        }
    }

    [TestFixture]
    public class HasAtLeastTests : CommonImmediateTests
    {
        [Test]
        public void CountEqualToLength_ReturnsTrue()
        {
            var count = 5;
            var input = Enumerable.Range(1, count);
            var actual = Extensions.HasAtLeast(input, count);
            Assert.True(actual);
        }

        [Test]
        public void CountIsLessThanInputLength_ReturnsTrue()
        {
            var count = 5;
            var input = Enumerable.Range(1, count);
            var actual = Extensions.HasAtLeast(input, count - 2);
            Assert.True(actual);
        }

        [Test]
        public void CountIsMoreThanInputLength_ReturnsFalse()
        {
            var count = 5;
            var input = Enumerable.Range(1, count);
            var actual = Extensions.HasAtLeast(input, count + 2);
            Assert.False(actual);
        }

        [Test]
        public void EmptyInput_WithPositiveCount_ReturnFalse()
        {
            var count = 5;
            var input = Enumerable.Empty<int>();
            var actual = Extensions.HasAtLeast(input, count);
            Assert.False(actual);
        }

        [Test]
        public void ZeroCount_ReturnTrue()
        {
            var input = Enumerable.Range(1, 5);
            var actual = Extensions.HasAtLeast(input, 0);
            Assert.True(actual);
        }

        [Test]
        public void NegativeCount_Throw()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                Extensions.HasAtLeast(Enumerable.Empty<int>(), -1);
                Extensions.HasAtLeast(Enumerable.Range(1, 5), -2);
            });
        }

        [Test]
        public void CountLessThanInputLength_InputNotIteratedMoreThanCount()
        {
            var expectedCount = 5;
            var inputCount = expectedCount + 2;
            var input = TestableEnumerable.Range(inputCount);
            Extensions.HasAtLeast(input, expectedCount);
            var msg =
                $"You should NOT iterate more than {expectedCount} items in order to figure out if it has at least {expectedCount}";
            if (input.MoveNextCount == inputCount)
            {
                msg = "Did you use Count() Linq extension method? not efficient!\n" + msg;
            }

            Warn.If(input.MoveNextCount > expectedCount, msg);
        }

        [Test, Description("Bonus")]
        public void InputIsIReadOnlyCollection_InputNotIterated()
        {
            var count = 5;
            var input = TestableReadOnlyCollection.Range(count);
            Extensions.HasAtLeast(input, count);
            Warn.Unless(
                input.MoveNextCount == 0,
                "You didn't take advantage on the IReadOnlyCollection.Count property that the 'input' implements"
            );
        }

        protected override void SutImmediateAction<T>(IEnumerable<T> input)
        {
            Extensions.HasAtLeast(input, 1);
        }
    }

    [TestFixture, Description("Bonus")]
    public class RemoveByKeysTests : CommonDeferredOnInputEnumerableTests<DateTime>
    {
        [Test]
        public void KeysExist_ReturnsItemsWithoutKeys()
        {
            var input = Enumerable.Range(1, 60).Select(i => new DateTime(2000, 1, 1).AddDays(i));
            var keys = new HashSet<DayOfWeek>(new[] { DayOfWeek.Friday, DayOfWeek.Saturday });
            var expected = input.Where(x => !keys.Contains(x.DayOfWeek));
            var sut = Extensions.RemoveByKeys(input, keys, x => x.DayOfWeek);
            var actual = sut.ToArray();
            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test]
        public void KeysNotExist_ReturnsAllItems()
        {
            var input = Enumerable.Range(1, 60).Select(i => new DateTime(2000, 1, 1).AddDays(i));
            var keys = new[] { 1990, 1995 };
            var sut = Extensions.RemoveByKeys(input, keys, x => x.Year);
            var actual = sut.ToArray();
            Assert.That(actual, Is.EqualTo(input));
        }

        [Test]
        public void KeysIteratedOnlyOnce()
        {
            var input = Enumerable.Range(1, 5);
            var keys = TestableEnumerable.Range(2);
            var sut = Extensions.RemoveByKeys(input, keys, x => x);
            sut.ToArray();
            Warn.Unless(
                keys.GetEnumeratorCount == 1,
                "keys parameter iterated more than once. expected you optimize it by using HashSet"
            );
        }

        [Test]
        public void KeysNotIteratedBeforeGetEnumeratorCalled()
        {
            var input = Enumerable.Range(1, 5);
            var keys = TestableEnumerable.Range(2);
            Extensions.RemoveByKeys(input, keys, x => x);
            Warn.Unless(
                keys.GetEnumeratorCount == 0,
                "keys parameter was iterated immediately and not deferred till iterating RemoveByKeys"
            );
        }

        protected override object SutDeferredAction(IEnumerable<DateTime> input)
        {
            var keys = new[] { 2000, 2002 };
            return Extensions.RemoveByKeys(input, keys, x => x.GetHashCode());
        }

        protected override IEnumerable<DateTime> CreateSimpleInputSequence()
        {
            return Enumerable.Range(1, 5).Select(year => new DateTime(2000 + year, 1, 1));
        }
    }

    [TestFixture, Description("Bonus")]
    public class InterleaveTests : CommonDeferredOnInputEnumerableTests<int>
    {
        [Test]
        public void FirstIsEmpty_ReturnSecond()
        {
            IEnumerable<int> first = Enumerable.Empty<int>();
            IEnumerable<int> second = Enumerable.Range(1, 3);
            var sut = Extensions.Interleave(first, second);
            var actual = sut.ToArray();
            Assert.That(actual, Is.EqualTo(second));
        }

        [Test]
        public void SecondIsEmpty_ReturnFirst()
        {
            IEnumerable<int> first = Enumerable.Range(1, 3);
            IEnumerable<int> second = Enumerable.Empty<int>();
            var sut = Extensions.Interleave(first, second);
            var actual = sut.ToArray();
            Assert.That(actual, Is.EqualTo(first));
        }

        [Test]
        public void ItemsInterleavedCorrectly()
        {
            IEnumerable<int> first = Enumerable.Range(1, 3);
            IEnumerable<int> second = Enumerable.Range(11, 3);
            var expected = new[] { 1, 11, 2, 12, 3, 13 };
            var sut = Extensions.Interleave(first, second);
            var actual = sut.ToArray();
            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test]
        public void FirstIsLonger_ReturnInterleavedItemsAndRestOfFirstAppended()
        {
            IEnumerable<int> first = Enumerable.Range(1, 5);
            IEnumerable<int> second = Enumerable.Range(11, 3);
            var expected = new[] { 1, 11, 2, 12, 3, 13, 4, 5 };
            var sut = Extensions.Interleave(first, second);
            var actual = sut.ToArray();
            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test]
        public void FirstIsShorter_ReturnInterleavedItemsAndRestOfSecondAppended()
        {
            IEnumerable<int> first = Enumerable.Range(1, 3);
            IEnumerable<int> second = Enumerable.Range(11, 5);
            var expected = new[] { 1, 11, 2, 12, 3, 13, 14, 15 };
            var sut = Extensions.Interleave(first, second);
            var actual = sut.ToArray();
            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test]
        public void SecondIsNull_Throw()
        {
            TestHelper.ShouldWarn_WhenParameterIsNull(
                "second",
                () =>
                {
                    IEnumerable<int> first = Enumerable.Empty<int>();
                    Extensions.Interleave(first, null);
                }
            );
        }

        [Test]
        public void Take1_FirstNotFullyIterated()
        {
            var first = TestableEnumerable.Range(10);
            var second = Enumerable.Range(11, 3);

            var sut = Extensions.Interleave(first, second);
            foreach (var item in sut)
            {
                break;
            }

            Assert.AreEqual(first.MoveNextCount, 1);
        }

        [Test]
        public void Take1_SecondNotIterated()
        {
            var first = Enumerable.Range(1, 3);
            var second = TestableEnumerable.Range(10);

            var sut = Extensions.Interleave(first, second);
            foreach (var item in sut)
            {
                break;
            }

            Assert.Zero(second.MoveNextCount);
        }

        [Test]
        public void Take2_SecondNotFullyIterated()
        {
            var first = Enumerable.Range(1, 3);
            var second = TestableEnumerable.Range(10);

            var sut = Extensions.Interleave(first, second);
            sut.Take(2).ToList();

            Assert.AreEqual(second.MoveNextCount, 1);
        }

        protected override object SutDeferredAction(IEnumerable<int> first)
        {
            var second = Enumerable.Empty<int>();
            return Extensions.Interleave(first, second);
        }

        protected override IEnumerable<int> CreateSimpleInputSequence()
        {
            return Enumerable.Range(0, 5);
        }
    }
}
