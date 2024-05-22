using System;
using System.Collections.Generic;

namespace Demo2
{
    /// <summary>
    /// Template implementation of IEnumerable/IEnumerator
    /// </summary>
    public class Example2
    {
        public void Run()
        {
            var strings = new[]
            {
                "0",
                "1",
                "Two",
                "Three",
                "4",
                "5even",
                "Eight",
                "9"
            };

            var numbers = new OnlyNumbers(strings) as IEnumerable<int>;

            foreach (var value in numbers)
            {
                Console.WriteLine(value);
            }
        }
    }
}
