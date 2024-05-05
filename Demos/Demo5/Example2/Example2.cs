using System;
using System.Collections.Generic;

namespace Demo5.Example2
{
    /// <summary>
    /// Iterator blocks and extension methods
    /// </summary>
    public class Example2
    {
        public void Run()
        {
            Console.WriteLine("Chaining iterators with extension methods");
            Console.WriteLine("-------------------------");

            var oneHundredOddRandomNumbers = new Random().AsEnumerable().Odds().Limit(100);

            foreach (var i in oneHundredOddRandomNumbers)
            {
                Console.Write("{0}, ", i);
            }
        }


    }

    public static class MyExtensions
    {
        public static IEnumerable<int> AsEnumerable(this Random random)
        {
            while (true)
            {
                yield return random.Next();
            }
        }
        public static IEnumerable<int> Odds(this IEnumerable<int> source)
        {
            foreach(var i in source)
            {
                if (i % 2 == 1) yield return i;
            }
        }

        public static IEnumerable<int> Limit(this IEnumerable<int> source, int count)
        {
            foreach (var i in source)
            {
                if(count-- > 0) yield return i;
            }
        }
    }
}