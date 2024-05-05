using System;
using System.Collections.Generic;
using System.IO;
using Exercise_4B;

namespace Exercise_4C
{
    public class Program
    {
        private static readonly string HORIZONTAL_LINE = new string('-', 50);

        static void Main(string[] args)
        {
            Console.WriteLine("Exercise 4B");
            Console.WriteLine(HORIZONTAL_LINE);

            double average = 0.0;
            int count = 0,
                sum = 0;

            var students = LoadAllStudents();

            var sameStudents = SideEffect(students, Console.WriteLine);

            // Bonus: dirty trick to calculate average grade
            var stillSameStudents = SideEffect(
                sameStudents,
                s =>
                {
                    count++;
                    sum += s.Grade;
                    average = sum / count;
                }
            );

            var failedStudentsEmails = Convert(
                StudentsByGrades(stillSameStudents, 0, 50),
                student => student.Email
            );

            // Bonus: Distinct emails
            var set = new HashSet<string>(failedStudentsEmails); // This will execute/consume the IEnumerable chain
            Console.WriteLine(HORIZONTAL_LINE);

            string chainedEmails = string.Join(";" + Environment.NewLine, set);
            Console.WriteLine($"Average Grade: {average}");
            Console.WriteLine(HORIZONTAL_LINE);
            Console.WriteLine(chainedEmails);
            Console.WriteLine(HORIZONTAL_LINE);

            Console.ReadLine();
        }

        private static IEnumerable<StudentInfo> LoadAllStudents()
        {
            foreach (var studentsFile in Directory.EnumerateFiles("Students", "*.csv"))
            {
                var students = new StudentsFileEnumerator(studentsFile);
                foreach (var student in students)
                {
                    yield return student;
                }
            }
        }

        public static IEnumerable<TResult> Convert<T, TResult>(
            IEnumerable<T> source,
            Func<T, TResult> converter
        )
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }
            if (converter == null)
            {
                throw new ArgumentNullException("converter");
            }
            return ConvertInternal(source, converter);
        }

        private static IEnumerable<TResult> ConvertInternal<T, TResult>(
            IEnumerable<T> source,
            Func<T, TResult> converter
        )
        {
            foreach (var item in source)
            {
                yield return converter(item);
            }
        }

        public static IEnumerable<StudentInfo> StudentsByGrades(
            IEnumerable<StudentInfo> students,
            int fromGrade,
            int tillGrade
        )
        {
            if (students == null)
            {
                throw new ArgumentNullException(nameof(students));
            }
            return StudentsByGradesInternal(students, fromGrade, tillGrade);
        }

        private static IEnumerable<StudentInfo> StudentsByGradesInternal(
            IEnumerable<StudentInfo> students,
            int fromGrade,
            int tillGrade
        )
        {
            foreach (var student in students)
            {
                if (student.Grade >= fromGrade && student.Grade <= tillGrade)
                {
                    yield return student;
                }
            }
        }

        public static IEnumerable<T> SideEffect<T>(IEnumerable<T> source, Action<T> action)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }
            if (action == null)
            {
                throw new ArgumentNullException("action");
            }
            return SideEffectInternal(source, action);
        }

        private static IEnumerable<T> SideEffectInternal<T>(IEnumerable<T> source, Action<T> action)
        {
            foreach (var item in source)
            {
                action(item);
                yield return item;
            }
        }
    }
}
