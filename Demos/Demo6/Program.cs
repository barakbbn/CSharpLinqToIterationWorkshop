using System;

namespace Demo6
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Choose Example to run:");
            Console.WriteLine("[1] - Deferred code validation");
            Console.WriteLine("[2] - Disposed data source");
            Console.WriteLine("[3] - Immutable data");

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
                    case ConsoleKey.Escape:
                        return;

                    default:
                        promptUser = true;
                        Console.WriteLine("Please choose 1, 2");
                        break;
                }
            }

            Console.WriteLine("----------");
            Console.WriteLine("Bye Bye");
            Console.ReadKey();
        }
    }
}
