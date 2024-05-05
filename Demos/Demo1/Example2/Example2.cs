using System;
using System.Collections.Generic;

namespace Demo1
{
    public class Example2
    {
        //Show how foreach works
        public void Run()
        {
            var enumerable = new TestEnumerable();

            Console.WriteLine("Iterating using Enumerator");
            Console.WriteLine("-------------------------");
            //Without foreach
            using (IEnumerator<string> enumerator = enumerable.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    string value = enumerator.Current;
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