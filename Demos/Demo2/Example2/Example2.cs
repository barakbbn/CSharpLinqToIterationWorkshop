using System;

namespace Demo2
{
    /// <summary>
    /// Custom collection that wraps another collection
    /// </summary>
    public class Example2
    {
        public void Run()
        {
            var numbers = new[]
            {
                "One", "Two", "Three", "Four", "Five", "Six", "Seven", "Eight", "Nine", "Ten"
            };

            var filterBy3Letters = new FilteringCollection<string>(numbers, value => value.Length == 3);

            foreach (string number in filterBy3Letters)
            {
                Console.WriteLine(number);
            }
        }

    }
}