using System;
using System.Collections.Generic;
using System.Linq;

namespace Demo3
{
    public class Program
    {
        public static void Main()
        {
            {
                Console.WriteLine("Choose Example to run:");
                Console.WriteLine("[1] - IEnumerator.GetEnumerator with yield");
                Console.WriteLine("[2] - yield with methods");
                Console.WriteLine("[3] - nested iteration on same IEnumerable");
                Console.WriteLine("[3] - Infinite IEnumerable");

                var promptUser = true;
                while (promptUser)
                {
                    promptUser = false;
                    var choice = Console.ReadKey();
                    Console.WriteLine();
                    Console.WriteLine("----------");

                    switch (choice.Key)
                    {
                        case ConsoleKey.D1:
                            new Example1().Run();
                            break;
                        case ConsoleKey.D2:
                            new Example2().Run();
                            break;
                        case ConsoleKey.D3:
                            new Example3().Run();
                            break;
                        case ConsoleKey.D4:
                            new Example4().Run();
                            break;
                        case ConsoleKey.Escape:
                            return;

                        default:
                            promptUser = true;
                            Console.WriteLine("Please choose 1, 2, 3, 4");
                            break;
                    }
                }

                Console.WriteLine("----------");
                Console.WriteLine("Bye Bye");
                Console.ReadKey();
            }
        }
    }
}
