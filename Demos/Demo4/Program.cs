using System;
using System.Collections.Generic;

namespace Demo4
{
    interface IHelper
    {
        IEnumerable<T> Limit<T>(IEnumerable<T> values, int limit);
        IEnumerable<int> OnlyPositive(IEnumerable<int> values);
        IEnumerable<TOut> Calc<TIn, TOut>(IEnumerable<TIn> values, Func<TIn, TOut> mathOperation);
    }
    class Program
    {
        static void Main(string[] args)
        {
            IHelper helper;

            Console.WriteLine("Immediate Execution");
            helper = new ImmediateHelper();
            Run(helper);

            Console.ReadKey();
            Console.WriteLine();

            Console.WriteLine("Deferred Execution");
            helper = new DeferredHelper();
            Run(helper);
            Console.ReadKey();
        }

        static void Run(IHelper helper)
        {
            var numbers = new List<int> { -1, 0, 1, 2, 3, 4, 5 };
            IEnumerable<int> range = helper.Limit(numbers, 5); // -1, 0, 1, 2, 3
            IEnumerable<int> positives = helper.OnlyPositive(values: range); // 1, 2, 3
            IEnumerable<int> results = helper.Calc(values: positives, mathOperation: n => n * n); // 1, 4, 9

            Console.Write("  Results:");
            foreach (var result in results)
            {
                Console.Write("  " + result);
            }

            numbers.Insert(0, 10);

            Console.WriteLine();
            Console.Write("  Prepended 10 to Results:");

            foreach (var result in results)
            {
                Console.Write("  " + result);
            }
            Console.WriteLine();
        }


    }

}

