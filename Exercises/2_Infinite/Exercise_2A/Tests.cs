using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using TestsUtils;

namespace Exercise_2A
{
    [TestFixture, Timeout(5000)]
    public class EndlessCycleTests : CommonDeferredOnInputEnumerableTests<int>
    {
        [Test]
        public void Enumerate_SourceSequenceIsCycledManyTimes()
        {
            var input = new[] { 0, 1 };
            int size = 1000 * input.Length;
            var expected = new List<int>(size);
            while (expected.Count < size)
                expected.AddRange(input);
            var actual = new List<int>(size);

            var sut = CreateEndlessCycle<int>(input);
            foreach (var v in (sut as IEnumerable<int>))
            {
                actual.Add(v);
                if (actual.Count == size)
                {
                    break;
                }
            }

            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test]
        public void Enumerate_EmptySource_ProducesNoValues()
        {
            var input = Enumerable.Empty<int>();
            var sut = CreateEndlessCycle<int>(input);
            using (var enumerator = (sut as IEnumerable<int>).GetEnumerator())
            {
                var actual = enumerator.MoveNext();
                Assert.False(actual);
            }
        }

        [Test]
        public void Enumerate_EnumerateAgain_ProducesSameResults()
        {
            IEnumerable<int> input = new[] { 0, 1 };
            var size = 9;
            var expected = new List<int>(size);
            var actual = new List<int>(size);

            var sut = CreateEndlessCycle<int>(input);

            foreach (var v in (sut as IEnumerable<int>))
            {
                expected.Add(v);
                if (expected.Count == size)
                {
                    break;
                }
            }
            foreach (var v in (sut as IEnumerable<int>))
            {
                actual.Add(v);
                if (actual.Count == size)
                {
                    break;
                }
            }

            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test]
        public void Enumerate_SourceUnchanged()
        {
            var input = new[] { 0, 1 };
            var expected = input.Clone();
            var sut = CreateEndlessCycle<int>(input);

            using (var enumerator = (sut as IEnumerable<int>).GetEnumerator())
            {
                for (var i = 0; i < 10; i++)
                {
                    enumerator.MoveNext();
                    var current = enumerator.Current;
                }
            }
            Assert.That(input, Is.EqualTo(expected));
        }

        [Test]
        public void ChangeSource_ProducesCorrectResultsForChangedSource()
        {
            var input = new LinkedList<int>(new[] { 0, 1 });
            int size = 10 * input.Count;

            var sut = CreateEndlessCycle<int>(input);
            var counter = size;
            foreach (var v in (sut as IEnumerable<int>))
            {
                counter--;
                if (counter == 0)
                    break;
            }

            input.RemoveFirst();
            input.AddLast(2);
            size = 10 * input.Count;

            var expected = new List<int>(size);
            while (expected.Count < size)
                expected.AddRange(input);

            var actual = new List<int>(size);
            foreach (var v in (sut as IEnumerable<int>))
            {
                actual.Add(v);
                if (actual.Count == size)
                    break;
            }

            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test, Description("BONUS")]
        public void Enumerate_MoveNextReturnedFalse_InputEnumeratorDisposed()
        {
            var input = TestableEnumerable.Range(0);
            var sut = CreateEndlessCycle<int>(input);
            using (var enumerator = (sut as IEnumerable<int>).GetEnumerator())
            {
                enumerator.MoveNext();
                Warn.If(
                    input.DisposedCount == 0,
                    "Expected 'input' internal IEnumerator to be disposed when MoveNext() returned false"
                );
            }
        }

        private EndlessCycle<T> CreateEndlessCycle<T>(IEnumerable<T> input)
        {
            return new EndlessCycle<T>(input);
        }

        protected override object SutDeferredAction(IEnumerable<int> input)
        {
            return CreateEndlessCycle<int>(input);
        }

        protected override IEnumerable<int> CreateSimpleInputSequence()
        {
            return new List<int> { 3, 2, 1, 0 };
        }
    }
}
