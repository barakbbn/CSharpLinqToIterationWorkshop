using System;
using System.Collections;
using System.Collections.Generic;

namespace Exercise_1B
{
    public class Program
    {
        static void Main(string[] args) { }
    }

    // TODO: Implement IEnumerable<string>
    public class FizzBuzzSequence
    {
        private bool _bonus;

        public FizzBuzzSequence(IEnumerable<int> source)
        {
            // TODO: You'll need the 'source' parameter to work on
            throw new NotImplementedException();
        }

        public FizzBuzzSequence(IEnumerable<int> source, bool bonus)
            : this(source)
        {
            _bonus = bonus;
        }
    }

    // TODO: Create class FizzBuzzEnumerator that implements IEnumerator<string>

    // Bonus
    // TODO: Create class FizzBuzzBonusEnumerator that implements IEnumerator<string>

}