using System;

namespace Demo2
{
    /// <summary>
    /// Special type of sequence No exactly a collection)
    /// </summary>
    public class Example1
    {
        public void Run()
        {
            var range = new Range(1, 5);

            foreach (int number in range)
            {
                Console.WriteLine(number);
            }
        }

    }
}