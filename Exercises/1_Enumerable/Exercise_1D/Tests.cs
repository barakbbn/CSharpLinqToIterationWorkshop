using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace Exercise_1D
{
    [TestFixture]
    public class DistinctUntilChangedTests
    {
        [Test]
        public void InputWithConsecutiveValues_NonConsecutiveValue()
        {
            var input = "Why do java programmers wear glasses? They can't C# ...";
            var expected = "Why do java programers wear glases? They can't C# .";
            var sut = CreateSut<char>(input);
            var actual = sut.ToArray();
            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test]
        public void InputWithNulls_NonConsecutiveValue()
        {
            var input = new object[] { null, null, true, true, false, false, true };
            var expected = new List<object>() { null, true, false, true };
            var sut = CreateSut<object>(input);
            var actual = sut.ToArray();
            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test]
        public void EmptyInput_ProducesNoValues()
        {
            var input = Enumerable.Empty<int>();
            var sut = CreateSut<int>(input);
            var actual = sut.ToArray();
            Assert.IsEmpty(actual);
        }

        [Test]
        public void Enumerate_EnumerateAgain_ProducesSameResults()
        {
            IEnumerable<int> input = new[] { 0, 1, 2, 2, 3, 3, 3, 4, 4, 4, 4, 3, 3, 3, 2, 2, 1, 0 };
            var sut = CreateSut<int>(input);
            var expected = sut.ToArray();
            var actual = sut.ToArray();
            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test]
        public void Enumerate_InputUnchanged()
        {
            var input = new[] { 0, 1, 2, 2, 3, 3, 3, 4, 4, 4, 4, 3, 3, 3, 2, 2, 1, 0 };
            var expected = input.Clone();
            var sut = CreateSut<int>(input);
            var forcedEnumeration = sut.ToArray();
            Assert.That(input, Is.EqualTo(expected));
        }

        [Test]
        public void Create_InputIsNull_Throw()
        {
            try
            {
                CreateDistinctUntilChanged<int>(null);
                Warn.If(true, "Expected ArgumentNullException");
            }
            catch (Exception ex)
            {
                Warn.Unless(
                    ex is ArgumentNullException,
                    $"Expected ArgumentNullException but got:\n  {ex}"
                );
            }
        }

        [Test]
        public void Enumerate_InputEnumeratorDisposed()
        {
            var input = new TestableEnumerable(3);
            var sut = CreateSut<int>(input);
            sut.ToArray();
            Warn.Unless(
                input.DisposedCount > 0,
                "Expected 'input' internal IEnumerator to be disposed"
            );
        }

        [Test]
        public void ChangeInput_ProducesCorrectResultsForChangedInput()
        {
            var input = new LinkedList<int>(new[] { 21, 12 });
            var sut = CreateSut<int>(input);
            sut.ToArray();
            input.RemoveFirst();
            input.AddLast(0);
            var actual = sut.ToArray();
            Assert.That(actual, Is.EqualTo(input));
        }

        [Test, Description("BONUS")]
        public void CustomEqualityComparer_NonConsecutiveValue()
        {
            var input = new List<string>()
            {
                "Apple",
                "apple",
                "APPLE",
                "Berry",
                "bErRy",
                "cherry",
                "Dragon Fruit",
                "Dragon-fruit"
            };
            var expected = new[] { "Apple", "Berry", "cherry", "Dragon Fruit", "Dragon-fruit" };
            var sut = CreateSut<string>(input, StringComparer.CurrentCultureIgnoreCase);
            var actual = sut.ToArray();
            Warn.Unless(actual, Is.EqualTo(expected).IgnoreCase);
        }

        internal class TestableEnumerable : IReadOnlyCollection<int>
        {
            class TestableEnumerator : IEnumerator<int>, IDisposable
            {
                TestableEnumerable _owner;

                public TestableEnumerator(TestableEnumerable owner)
                {
                    _owner = owner;
                }

                public int Current { get; private set; } = 0;

                object IEnumerator.Current => Current;

                public void Dispose()
                {
                    _owner.DisposedCount++;
                }

                public bool MoveNext()
                {
                    if (Current == _owner.Count)
                        return false;
                    Current++;
                    return true;
                }

                public void Reset()
                {
                    Current = 0;
                }
            }

            public TestableEnumerable(int count = 0)
            {
                Count = count;
            }

            public IEnumerator<int> GetEnumerator()
            {
                GetEnumeratorCount++;
                return new TestableEnumerator(this);
            }

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

            public int GetEnumeratorCount { get; private set; }
            public int DisposedCount { get; private set; }

            public int Count { get; private set; }
        }

        private IEnumerable<T> CreateSut<T>(
            IEnumerable<T> source,
            IEqualityComparer<T> comparer = null
        )
        {
            var sut = CreateDistinctUntilChanged(source, comparer);
            Assert.IsInstanceOf<IEnumerable<T>>(
                sut,
                "DistinctUntilChanged doesn't implement interface IEnumerable"
            );
            return (IEnumerable<T>)sut;
        }

        private IEnumerable<T> CreateDistinctUntilChanged<T>(
            IEnumerable<T> source,
            IEqualityComparer<T> comparer = null
        )
        {
            var sut =
                (comparer == null)
                    ? new DistinctUntilChanged<T>(source)
                    : new DistinctUntilChanged<T>(source, comparer);

            Assert.IsInstanceOf<IEnumerable<T>>(
                sut,
                "DistinctUntilChanged doesn't implement interface IEnumerable"
            );

            return (IEnumerable<T>)sut;
        }
    }
}
