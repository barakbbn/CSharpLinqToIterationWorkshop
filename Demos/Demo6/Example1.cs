using System;
using System.Collections.Generic;

namespace Demo6
{
    //Gotcha - Iterator block preconditions check is deferred
    internal class Example1
    {
        public void Run()
        {
            var items = Take<int>(null, 100);

            foreach (int item in items)
            {
                Console.WriteLine(item);
            }
        }

        private static IEnumerable<T> Take<T>(IEnumerable<T> source, int count)
        {
            if (source == null) throw new ArgumentNullException();
            if (count <= 0) yield break;

            int counter = 0;
            foreach (var item in source)
            {
                yield return item;
                if (++counter == count) yield break;
            }
        }
    }
}