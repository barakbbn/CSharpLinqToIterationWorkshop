using System;
using System.Collections.Generic;

namespace Demo3
{
    public class Example4
    {
        public void Run()
        {

            Console.WriteLine("Reading lines from Console, until Empty line");
            Console.WriteLine("-------------------------");
            foreach (var value in ReadConsoleLines("? "))
            {
                Console.WriteLine($">>> {value.ToUpper()}");
                if (string.IsNullOrEmpty(value)) break;
            }
            Console.WriteLine();
        }
        public static IEnumerable<string> ReadConsoleLines(string prompt = "")
        {
            while (true)
            {
                Console.Write(prompt);
                yield return Console.ReadLine();
            }
        }
    }
}
