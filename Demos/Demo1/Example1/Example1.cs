using System;
using System.Collections.Generic;

namespace Demo1
{
    class Example1
    {
        public void Run()
        {
            IEnumerable<int> enumerable = new[] {3, 2, 1, 0};

            Console.WriteLine("Iterating using Enumerator");
            Console.WriteLine("-------------------------");
            //Without foreach
            using (IEnumerator<int> enumerator = enumerable.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    int value = enumerator.Current;
                    Console.WriteLine(value);
                }
            }

            Console.WriteLine();
            Console.WriteLine("\n\nIterating using foreach");
            Console.WriteLine("-------------------------");
            //With foreach
            foreach (var value in enumerable)
            {
                Console.WriteLine(value);
            }
        }
    }
}
