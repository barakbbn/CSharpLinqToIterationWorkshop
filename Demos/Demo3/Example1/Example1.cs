using System;
using System.Collections.Generic;

namespace Demo3
{
    public class Example1
    {
        //Show how yield is used
        public void Run()
        {
            Console.WriteLine("Iterating Range class using yield");
            Console.WriteLine("-------------------------");

            var range = new Range(1, 3);

            foreach (var value in range)
            {
                Console.WriteLine(value);
            }

            foreach (var value in range.Reversed)
            {
                Console.WriteLine(value);
            }
        }
    }
}
