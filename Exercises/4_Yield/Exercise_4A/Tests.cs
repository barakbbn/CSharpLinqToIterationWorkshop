using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using TestsUtils;

namespace Exercise_4A
{
    public abstract class FizzBuzzSequenceTestsBase : CommonDeferredOnInputEnumerableTests<int>
    {
        protected abstract IEnumerable<string> RunExercise(IEnumerable<int> source);

        [Test]
        public void EdgeCase_Zero_ReturnZeroAsString()
        {
            var input = new List<int>() { 0 };
            var expected = new List<string>() { "0" };

            var sut = Exercises.FizzBuzzSequence(input);
            var actual = (sut as IEnumerable<string>).ToArray();

            Warn.Unless(actual, Is.EqualTo(expected));
        }

        [Test]
        public void EmptyInput_ProducesNoValues()
        {
            var input = Enumerable.Empty<int>();
            var sut = Exercises.FizzBuzzSequence(input);
            var actual = (sut as IEnumerable<string>).ToArray();
            Assert.IsEmpty(actual);
        }

        protected override object SutDeferredAction(IEnumerable<int> input)
        {
            return Exercises.FizzBuzzSequence(input);
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

            var sut = RunExercise(input);
            var actual = (sut as IEnumerable<string>).ToArray();

            Assert.That(actual, Is.EqualTo(expected));
        }

        protected override IEnumerable<string> RunExercise(IEnumerable<int> source) =>
            Exercises.FizzBuzzSequence(source);
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

            var sut = RunExercise(input);
            var actual = (sut as IEnumerable<string>).ToArray();

            Assert.That(actual, Is.EqualTo(expected));
        }

        protected override IEnumerable<string> RunExercise(IEnumerable<int> source) =>
            Exercises.FizzBuzzSequenceBonus(source);
    }

    [TestFixture]
    public class RemoveNullsTests : CommonDeferredOnInputEnumerableTests<string>
    {
        [Test]
        public void EmptyInput_ReturnEmptyResult()
        {
            var sut = new Exercises.RemoveNulls<string>(Enumerable.Empty<string>());
            var actual = sut.ToArray();
            Assert.IsEmpty(actual);
        }

        [Test]
        public void InputWithNulls_ReturnAllNonNullItems()
        {
            var input = new[] { null, "a", "b", "c", null };
            var expected = input.Where(x => x != null);
            var sut = new Exercises.RemoveNulls<string>(input);
            var actual = sut.ToArray();

            Assert.That(actual, Is.EqualTo(expected));
        }

        protected override IEnumerable<string> CreateSimpleInputSequence()
        {
            return new[] { "not null", null, "NOT NULL", null, "", null };
        }

        protected override object SutDeferredAction(IEnumerable<string> input)
        {
            return new Exercises.RemoveNulls<string>(input);
        }
    }

    [TestFixture]
    public class FibonacciTests
    {
        [Test]
        public void CountIsZero_ReturnEmptyResult()
        {
            var count = 0;
            var sut = Exercises.Fibonacci(count);
            var actual = sut.ToArray();
            Assert.IsEmpty(actual);
        }

        [Test]
        public void CountIsPositive_ReturnExpectedSequence()
        {
            var expected = new[] { 0, 1, 1, 2, 3, 5, 8, 13, 21, 34 };
            var sut = Exercises.Fibonacci(expected.Length);

            var actual = sut.ToArray();

            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test]
        public void CountIsNegative_Throws()
        {
            var count = -1;
            try
            {
                var sut = Exercises.Fibonacci(count);
                Warn.If(true, "Expected ArgumentOutOfRangelException");
            }
            catch (Exception ex)
            {
                Warn.Unless(
                    ex is ArgumentOutOfRangeException,
                    $"Expected ArgumentOutOfRangeException but got:\n  {ex}"
                );
            }
        }
    }

    [TestFixture]
    public class BinaryTreeNodeTests
    {
        [Test]
        public void Enumerate_ReturnItemsInBreadthFirstLeftToRightOrder()
        {
            var sut = new BinaryTreeNode<int>
            {
                Value = 5,
                Left = new BinaryTreeNode<int>
                {
                    Value = 2,
                    Left = new BinaryTreeNode<int> { Value = 1, },
                    Right = new BinaryTreeNode<int>
                    {
                        Value = 3,
                        Right = new BinaryTreeNode<int> { Value = 4, }
                    }
                },
                Right = new BinaryTreeNode<int>
                {
                    Value = 9,
                    Left = new BinaryTreeNode<int>
                    {
                        Value = 8,
                        Left = new BinaryTreeNode<int>
                        {
                            Value = 7,
                            Left = new BinaryTreeNode<int> { Value = 6, }
                        }
                    },
                }
            };

            var expected = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 };

            var actual = sut.ToArray();
            Assert.That(actual, Is.EqualTo(expected));
        }
    }
}
