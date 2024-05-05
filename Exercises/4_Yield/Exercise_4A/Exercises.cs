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
            throw new NotImplementedException();
        }

        // Bonus
        public static IEnumerable<string> FizzBuzzSequenceBonus(IEnumerable<int> numbers)
        {
            return new[] { "Not Implemented Exception" };
        }

        public static IEnumerable<int> Fibonacci(int count)
        {
            throw new NotImplementedException();
        }

        public class RemoveNulls<T> : IEnumerable<T>
        {
            // TODO: Implement Constructor
            public RemoveNulls(IEnumerable<T> source)
            {
                throw new NotImplementedException();
            }

            public IEnumerator<T> GetEnumerator()
            {
                // TODO: Implement using yield return
                throw new NotImplementedException();
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
            // TODO: Implement using yield return
            //       First, yield return all left nodes' *value* (not the node itself)
            //       Then, yield return this node value
            //       Lastly, yield return all right nodes' *value*
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
