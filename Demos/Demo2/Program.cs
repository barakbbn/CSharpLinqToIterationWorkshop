using System;

namespace Demo2
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Choose Example to run:");
            Console.WriteLine("[1] - Table like data structure");
            Console.WriteLine("[2] - Filtering Collection (like Where)");
            Console.WriteLine("[3] - Range of numbers Collection");

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
                        Console.WriteLine("Please choose 1, 2, 3");
                        break;
                }
            }

            Console.WriteLine("----------");
            Console.WriteLine("Bye Bye");
            Console.ReadKey();
        }
    }
}
