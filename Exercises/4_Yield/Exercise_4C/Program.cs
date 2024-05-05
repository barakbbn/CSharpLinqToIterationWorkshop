using System;
using System.Collections;
using System.Collections.Generic;
using Exercise_4B;

namespace Exercise_4C
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Exercise 4C");
        }

        public static IEnumerable<TResult> Convert<T, TResult>(
            IEnumerable<T> source,
            Func<T, TResult> converter
        )
        {
            throw new NotImplementedException();
        }

        public static IEnumerable<StudentInfo> StudentsByGrades(
            IEnumerable<StudentInfo> source,
            int fromGrade,
            int tillGrade
        )
        {
            throw new NotImplementedException();
        }

        public static IEnumerable<T> SideEffect<T>(IEnumerable<T> source, Action<T> action)
        {
            throw new NotImplementedException();
        }
    }
}
