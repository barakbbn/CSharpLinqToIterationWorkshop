using System;
using System.Collections.Generic;
using System.Drawing;

namespace Demo3
{
    public class Example3
    {
        public void Run()
        {
            Console.WriteLine("Multiplication table");
            Console.WriteLine("====================");
            var oneTo10 = Example2.Range(1, 10);

            foreach (var a in oneTo10)
            {
                foreach (var b in oneTo10)
                {
                    Console.Write($"{(a * b),4}");
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }
    }
}
