using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Exercise_5B
{
    public static class Extensions
    {
        public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            // TODO:
            throw new NotImplementedException();
        }

        public static void ForEach<T>(this IEnumerable<T> source, Action<T, int> action)
        {
            // TODO:
            throw new NotImplementedException();
        }

        public static IEnumerable<T> Flatten<T>(this IEnumerable<IEnumerable<T>> source)
        {
            // TODO:
            throw new NotImplementedException();
        }

        public static string ToString<T>(this IEnumerable<T> source, string separator)
        {
            // TODO:
            throw new NotImplementedException();
        }

        public static bool HasAtLeast<T>(this IEnumerable<T> source, int count)
        {
            // TODO:
            throw new NotImplementedException();
        }

        public static IEnumerable<T> RemoveByKeys<T, K>(
            this IEnumerable<T> source,
            IEnumerable<K> keys,
            Func<T, K> keySelector,
            IEqualityComparer<K> comparer = null
        )
        {
            // TODO:
            throw new NotImplementedException();
        }

        // Bonus
        public static IEnumerable<T> Interleave<T>(this IEnumerable<T> first, IEnumerable<T> second)
        {
            // TODO:
            throw new NotImplementedException();
        }
    }

    public static class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Exercise 5B");
        }
    }
}
