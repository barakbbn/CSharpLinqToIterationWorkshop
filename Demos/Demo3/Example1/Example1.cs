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
            foreach (var value in new Range(1, 3))
            {
                Console.WriteLine(value);
            }
        }
    }
}