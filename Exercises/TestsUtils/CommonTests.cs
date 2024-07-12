using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NUnit.Framework;

namespace TestsUtils
{
    public abstract class CommonDeferredOnInputEnumerableTests<T>
    {
        protected abstract object SutDeferredAction(IEnumerable<T> input);
        protected abstract IEnumerable<T> CreateSimpleInputSequence();

        private IEnumerable SafeSutDeferredAction(IEnumerable<T> input)
        {
            var sut = SutDeferredAction(input);
            Assert.IsInstanceOf<IEnumerable>(
                sut,
                $"{sut.GetType().Name} doesn't implement interface IEnumerable"
            );

            return (IEnumerable)sut;
        }

        [Test]
        public void Enumerate_InputEnumeratorDisposed()
        {
            var source = CreateSimpleInputSequence();
            var input = TestableEnumerable.From(source);
            var sut = SafeSutDeferredAction(input);
            TestHelper.Enumerate(sut);
            Warn.Unless(
                input.DisposedCount > 0,
                "Expected 'input' internal IEnumerator to be disposed"
            );
        }

        [Test]
        public void Take1_InputEnumeratorDisposed()
        {
            var source = CreateSimpleInputSequence();
            var input = TestableEnumerable.From(source);
            var sut = SafeSutDeferredAction(input);
            foreach (var item in sut)
            {
                break;
            }
            Warn.Unless(
                input.DisposedCount > 0,
                "Expected 'input' internal IEnumerator to be disposed"
            );
        }

        [Test]
        public void InputIsNull_Throw()
        {
            TestHelper.ShouldWarn_WhenParameterIsNull(null, () => SafeSutDeferredAction(null));
        }

        [Test]
        public void GetEnumerator_InputNotIterated()
        {
            var source = CreateSimpleInputSequence();
            TestHelper.GetEnumerator_InputNotIterated(source, SafeSutDeferredAction);
        }

        [Test]
        public virtual void Enumerate_InputNotEmpty_InputIteratedOnlyOnce()
        {
            var source = CreateSimpleInputSequence();
            var input = TestableEnumerable.From(source);
            var sut = SafeSutDeferredAction(input);
            TestHelper.Enumerate(sut);
            Assert.AreEqual(
                1,
                input.GetEnumeratorCount,
                $"Expected input.GetEnumerator() to be called once, but actually was called {input.GetEnumeratorCount} times"
            );
        }

        [Test]
        public virtual void MoveNext_PreviousMoveNextReturnedFalse_ReturnFalse()
        {
            var source = CreateSimpleInputSequence();
            var input = TestableEnumerable.From(source);
            var sut = SafeSutDeferredAction(input);
            var enumerator = sut.GetEnumerator();
            while (enumerator.MoveNext())
            {
                // just consuming the enumerator
            }
            var actual = enumerator.MoveNext();

            Assert.False(
                actual,
                $"Expected IEnumerator.MoveNext to return false, after it already returned false once."
            );
        }
    }

    public abstract class CommonImmediateTests
    {
        protected abstract void SutImmediateAction<T>(IEnumerable<T> input);

        [Test]
        public void InputIsNull_Throw()
        {
            TestHelper.ShouldWarn_WhenParameterIsNull(null, () => SutImmediateAction<int>(null));
        }
    }

    public static class TestHelper
    {
        public static void Enumerate(object obj)
        {
            var enumerable = obj as IEnumerable;
            if (enumerable != null)
            {
                enumerable.OfType<object>().ToList();
            }
        }

        public static void ShouldWarn_WhenParameterIsNull(string parameterName, Action test)
        {
            try
            {
                test();
                Warn.If(true, "Expected ArgumentNullException");
            }
            catch (ArgumentNullException ex)
            {
                if (!string.IsNullOrEmpty(parameterName))
                {
                    Warn.Unless(
                        ex.ParamName == parameterName,
                        $"Expected ArgumentNullException for parameter {parameterName}:\n  {ex}"
                    );
                }
            }
            catch (TargetInvocationException ex)
            {
                Warn.Unless(
                    ex.InnerException is ArgumentNullException,
                    $"Expected ArgumentNullException but got:\n  {ex}"
                );
            }
            catch (Exception ex)
            {
                Warn.Unless(
                    ex is ArgumentNullException,
                    $"Expected ArgumentNullException but got:\n  {ex}"
                );
            }
        }

        public static void GetEnumerator_InputNotIterated<T>(
            IEnumerable<T> source,
            Func<IEnumerable<T>, object> sutDeferredAction
        )
        {
            var input = TestableEnumerable.From(source);
            var sut = sutDeferredAction(input);
            var enumerator = sut as IEnumerable;
            using (enumerator as IDisposable)
            {
                enumerator.GetEnumerator();
                Assert.Zero(
                    input.MoveNextCount,
                    "Input sequence was iterated before any item was requested"
                );
            }
        }

        public static void MoveNext_CalledOnce_InputNotFullyIterated<T>(
            IEnumerable<T> source,
            Func<IEnumerable<T>, object> sutDeferredAction
        )
        {
            var input = TestableEnumerable.From(source);
            var sut = sutDeferredAction(input);
            using (var enumerator = (sut as IEnumerable<int>).GetEnumerator())
            {
                enumerator.MoveNext();
                Assert.False(
                    input.MoveNextCompleted,
                    "Input was fully consumed while only 1 items was iterated"
                );
            }
        }
    }

    public class TestableEnumerable
    {
        public static TestableEnumerable<int> Range(int count)
        {
            return new TestableEnumerable<int>(Enumerable.Range(0, count));
        }

        public static TestableEnumerable<T> From<T>(IEnumerable<T> input)
        {
            return new TestableEnumerable<T>(input);
        }
    }

    public class TestableEnumerable<T> : IEnumerable<T>
    {
        private readonly IEnumerable<T> _input;

        public TestableEnumerable(IEnumerable<T> input)
        {
            if (input == null)
                throw new ArgumentNullException("input");
            _input = input;
        }

        public IEnumerator<T> GetEnumerator()
        {
            GetEnumeratorCount++;
            return new TestableEnumerator(this);
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public int GetEnumeratorCount { get; private set; }
        public int DisposedCount { get; private set; }

        public int MoveNextCount { get; private set; }
        public bool MoveNextCompleted { get; private set; }

        public class TestableEnumerator : IEnumerator<T>, IDisposable
        {
            private readonly TestableEnumerable<T> _owner;
            private IEnumerator<T> _enumerator;

            public bool MoveNextCompleted
            {
                get { return _owner.MoveNextCompleted; }
                private set { _owner.MoveNextCompleted = value; }
            }
            public int DisposedCount
            {
                get { return _owner.DisposedCount; }
                private set { _owner.DisposedCount = value; }
            }

            public TestableEnumerator(TestableEnumerable<T> owner)
            {
                _owner = owner;
            }

            public T Current { get; private set; } = default(T);

            object IEnumerator.Current => Current;

            public void Dispose()
            {
                if (_enumerator != null)
                    _enumerator.Dispose();
                DisposedCount++;
            }

            public bool MoveNext()
            {
                _owner.MoveNextCount++;
                if (MoveNextCompleted)
                    return false;

                if (_enumerator == null)
                    _enumerator = _owner._input.GetEnumerator();

                if (!_enumerator.MoveNext())
                {
                    MoveNextCompleted = true;
                    _enumerator.Dispose();
                    _enumerator = null;
                    return false;
                }

                Current = _enumerator.Current;
                return true;
            }

            public void Reset()
            {
                Current = default(T);
                MoveNextCompleted = false;
            }
        }
    }

    public class TestableReadOnlyCollection
    {
        public static TestableReadOnlyCollection<int> Range(int count)
        {
            return new TestableReadOnlyCollection<int>(Enumerable.Range(0, count).ToList());
        }

        public static TestableReadOnlyCollection<T> From<T>(IReadOnlyCollection<T> input)
        {
            return new TestableReadOnlyCollection<T>(input);
        }
    }

    public class TestableReadOnlyCollection<T> : TestableEnumerable<T>, IReadOnlyCollection<T>
    {
        private readonly IReadOnlyCollection<T> _collection;

        public TestableReadOnlyCollection(IReadOnlyCollection<T> input)
            : base(input)
        {
            _collection = input;
        }

        public int Count => _collection.Count;
    }
}
