using System;
using System.Collections.Generic;

namespace Demo3
{
    /// <summary>
    /// Refactor to use yield return
    /// </summary>
    public class Example2
    {
        public void Run()
        {
            var numbers = new List<int> { -1, 0, 1, 2, 3, 4, 5 };

            IEnumerable<int> positives = OnlyPositive(numbers); // 1, 2, 3, 4, 5
            IEnumerable<int> only3Positives = GetRange(positives, fromIndex: 0, count: 3); // 1, 2, 3
            IEnumerable<int> results = Calc(values: only3Positives, mathOperation: n => n * n); // 1, 4, 9

            Console.Write("Results:");
            foreach (var result in results)
            {
                Console.Write("  " + result);
            }
            Console.WriteLine();
        }

        public IEnumerable<int> OnlyPositive(IEnumerable<int> values)
        {
            foreach (var n in values)
            {
                if (n > 0)
                {
                    yield return n;
                }
            }
        }

        public IEnumerable<TOut> Calc<TIn, TOut>(
            IEnumerable<TIn> values,
            Func<TIn, TOut> mathOperation
        )
        {
            foreach (var item in values)
            {
                var result = mathOperation(item);
                yield return result;
            }
        }

        // TODO: implement
        public IEnumerable<T> GetRange<T>(IEnumerable<T> values, int fromIndex, int count)
        {
            int i = 0;
            foreach (var value in values)
            {
                if(i == fromIndex + count) break;
                if (i++ < fromIndex) continue;
                yield return value;
            }
        }
    }
}
