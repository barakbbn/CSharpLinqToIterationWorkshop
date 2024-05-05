using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;
using static Exercise_5B.Program;

namespace Exercise_5B
{
    public static class Extensions
    {
        public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            ForEach<T>(source, (x, i) => action(x));
        }

        public static void ForEach<T>(this IEnumerable<T> source, Action<T, int> action)
        {
            int index = 0;
            foreach (var item in source)
            {
                action(item, index++);
            }
        }

        public static IEnumerable<T> Flatten<T>(this IEnumerable<IEnumerable<T>> source)
        {
            return source.SelectMany(_ => _);
        }

        public static string ToString<T>(this IEnumerable<T> source, string separator)
        {
            return string.Join(separator, source);
        }

        public static bool HasAtLeast<T>(this IEnumerable<T> source, int count)
        {
            if (count < 0)
            {
                throw new ArgumentOutOfRangeException("count");
            }

            if (count == 0)
            {
                return true;
            }
            var collection = source as IReadOnlyCollection<T>;
            if (collection != null)
            {
                return collection.Count >= count;
            }
            var counter = 0;
            foreach (var item in source)
            {
                counter++;
                if (counter == count)
                {
                    return true;
                }
            }
            return false;
        }

        public static IEnumerable<T> RemoveByKeys<T, K>(
            this IEnumerable<T> source,
            IEnumerable<K> keys,
            Func<T, K> keySelector,
            IEqualityComparer<K> comparer = null
        )
        {
            // Should we return source.Where(item => hashSet.Contains(keySelector(item))) or perform yield return?
            // Without yield keyword, the method won't become deferred, so the creation of HashSet below will happen as soon as we call this method!
            // Which is not lazy/deferred enough.
            // * can hack code so source.Where() will lazily create the HashSet, but then, code won't be simpler than yield return
            // * Another way if to perform foreach on source.where(...) and yield return each item from it.
            var hashSet = keys as HashSet<K> ?? new HashSet<K>(keys, comparer);

            foreach (var item in source)
            {
                var key = keySelector(item);
                if (!hashSet.Contains(key))
                {
                    yield return item;
                }
            }
        }

        public static IEnumerable<T> Interleave<T>(this IEnumerable<T> first, IEnumerable<T> second)
        {
            if (first == null)
            {
                throw new ArgumentNullException(nameof(first));
            }
            if (second == null)
            {
                throw new ArgumentNullException(nameof(second));
            }
            return InterleaveInternal(first, second);
        }

        private static IEnumerable<T> InterleaveInternal<T>(
            this IEnumerable<T> first,
            IEnumerable<T> second
        )
        {
            var it1HasNext = true;
            var it2HasNext = true;
            using (
                IEnumerator<T> it1 = first.GetEnumerator(),
                    it2 = second.GetEnumerator()
            )
            {
                while (it1HasNext || it2HasNext)
                {
                    it1HasNext &= it1.MoveNext();
                    if (it1HasNext)
                    {
                        yield return it1.Current;
                    }
                    it2HasNext &= it2.MoveNext();
                    if (it2HasNext)
                    {
                        yield return it2.Current;
                    }
                }
            }
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
