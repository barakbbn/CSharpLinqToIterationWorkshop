using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exercise_4A
{
    public class Exercises
    {
        public static IEnumerable<string> FizzBuzzSequence(IEnumerable<int> numbers)
        {
            foreach (var n in numbers)
            {
                string value = null;
                if (n != 0 && n % 3 == 0)
                {
                    value += "Fizz";
                }
                if (n != 0 && n % 5 == 0)
                {
                    value += "Buzz";
                }
                yield return value ?? n.ToString();
            }
        }

        // Bonus
        public static IEnumerable<string> FizzBuzzSequenceBonus(IEnumerable<int> numbers)
        {
            foreach (var n in numbers)
            {
                var isFizz = n != 0 && n % 3 == 0;
                var isBuzz = n != 0 && n % 5 == 0;
                if (isFizz)
                {
                    yield return "Fizz";
                }
                if (isBuzz)
                {
                    yield return "Buzz";
                }
                if (!isFizz && !isBuzz)
                {
                    yield return n.ToString();
                }
            }
        }

        public static IEnumerable<int> Fibonacci(int count)
        {
            var previous = 0;
            var current = 1;
            for (; count > 0; count--)
            {
                yield return previous;
                (previous, current) = (current, previous + current);
            }
        }

        public class RemoveNulls<T> : IEnumerable<T>
        {
            private readonly IEnumerable<T> _source;

            public RemoveNulls(IEnumerable<T> source)
            {
                _source = source;
            }

            public IEnumerator<T> GetEnumerator()
            {
                foreach (var item in _source)
                {
                    if (item != null)
                    {
                        yield return item;
                    }
                }
            }

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }
    }

    public class BinaryTreeNode<T> : IEnumerable<T>
    {
        public BinaryTreeNode<T> Left { get; set; }
        public BinaryTreeNode<T> Right { get; set; }
        public T Value { get; set; } = default(T);

        public override string ToString()
        {
            return Value?.ToString();
        }

        public IEnumerator<T> GetEnumerator()
        {
            if (Left != null)
            {
                foreach (var left in this.Left)
                {
                    yield return left;
                }
            }

            yield return this.Value;

            if (Right != null)
            {
                foreach (var right in this.Right)
                {
                    yield return right;
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public IEnumerator<T> WithEnumerator()
        {
            return new BinaryTreeNodeEnumerator<T>(this);
        }
    }

    public class BinaryTreeNodeEnumerator<T> : IEnumerator<T>
    {
        enum MoveNextState
        {
            Left,
            Value,
            Right,
            NoMoreItems
        }

        private BinaryTreeNode<T> _parent;
        private MoveNextState _state = MoveNextState.Left;
        private IEnumerator _leftEnumerator;
        private IEnumerator _rightEnumerator;

        public BinaryTreeNodeEnumerator(BinaryTreeNode<T> parent)
        {
            _parent = parent;
        }

        public T Current { get; private set; }

        object IEnumerator.Current => Current;

        public void Dispose()
        {
            _parent = null;
            if (_leftEnumerator != null)
            {
                using (_leftEnumerator as IDisposable)
                    ;
                _leftEnumerator = null;
            }
            if (_rightEnumerator != null)
            {
                using (_rightEnumerator as IDisposable)
                    ;
                _rightEnumerator = null;
            }
        }

        public bool MoveNext()
        {
            switch (_state)
            {
                case MoveNextState.Left:
                    if (_parent.Left == null)
                    {
                        _state = MoveNextState.Value;
                        goto case MoveNextState.Value;
                    }
                    else if (_leftEnumerator == null)
                    {
                        _leftEnumerator = ((IEnumerable)_parent.Left).GetEnumerator();
                    }
                    bool leftHasValue = _leftEnumerator.MoveNext();
                    if (!leftHasValue)
                    {
                        _state++;
                    }
                    else
                    {
                        Current = (T)_leftEnumerator.Current;
                        return true;
                    }
                    goto case MoveNextState.Value;

                case MoveNextState.Value:
                    Current = _parent.Value;
                    _state = MoveNextState.Right;
                    return true;

                case MoveNextState.Right:
                    if (_parent.Right == null)
                    {
                        _state = MoveNextState.NoMoreItems;
                        goto case default;
                    }
                    else if (_rightEnumerator == null)
                    {
                        _rightEnumerator = ((IEnumerable)_parent.Right).GetEnumerator();
                    }
                    bool rightHasValue = _rightEnumerator.MoveNext();
                    if (!rightHasValue)
                    {
                        _state++;
                    }
                    else
                    {
                        Current = (T)_rightEnumerator.Current;
                        return true;
                    }
                    goto case default;

                default:
                    return false;
            }
        }

        public void Reset() { }
    }
}
