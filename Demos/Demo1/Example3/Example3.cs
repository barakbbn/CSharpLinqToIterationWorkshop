using System;
using System.Collections.Generic;

namespace Demo1
{
    public class Example3
    {
        public void Run()
        {
            Console.WriteLine("foreach inside foreach on same IEnumerable");
            Console.WriteLine("------------------------------------------");

            IEnumerable<int> enumerable = new[] { 3, 2, 1, 0 };

            foreach (var a in enumerable)
            {
                foreach (var b in enumerable)
                {
                    Console.WriteLine($"{a} + {b} = {a + b}");
                }
                Console.WriteLine();
            }
        }
    }
}
