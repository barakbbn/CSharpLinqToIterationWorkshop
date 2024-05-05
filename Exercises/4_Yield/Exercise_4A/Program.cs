using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exercise_4A
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Exercise 4A");
            Console.WriteLine("-----------");
            try
            {
                RunExercise_FizzBuzzSequence();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            Console.WriteLine();

            try
            {
                RunExercise_Fibonacci();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            Console.WriteLine();

            try
            {
                RunExercise_RemoveNulls();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            Console.WriteLine();

            try
            {
                RunExercise_BinaryTree();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            Console.WriteLine();
            Console.ReadLine();
        }

        private static void RunExercise_FizzBuzzSequence()
        {
            Console.WriteLine("FizzBuzz Sequence");

            string StringJoin(IEnumerable values)
            {
                var formatted = new List<string>();
                foreach (var value in values)
                {
                    formatted.Add(value.ToString().PadRight(8));
                }
                return string.Join(",", formatted);
            }

            var numbers = new List<int> { 2, 3, 4, 5, 6, 8, 9, 10, 15, 0 };

            Console.WriteLine($"Numbers : {StringJoin(numbers)}");

            var fizzBuzz = Exercises.FizzBuzzSequence(numbers);
            Console.WriteLine($"FizzBuzz: {StringJoin(fizzBuzz)}");
            var fizzBuzzBonus = Exercises.FizzBuzzSequenceBonus(numbers);
            Console.WriteLine($"Bonus   : {StringJoin(fizzBuzzBonus)}");
        }

        private static void RunExercise_Fibonacci()
        {
            Console.WriteLine("Fibonacci Sequence");

            var fibonacci = Exercises.Fibonacci(10);
            Console.WriteLine($"Fibonacci(10): {string.Join(", ", fibonacci)}");
        }

        private static void RunExercise_RemoveNulls()
        {
            Console.WriteLine("Remove Nulls");

            int? nullable = null;
            var input = new object[]
            {
                "Alpha",
                "Bravo",
                "Charlie",
                1,
                2,
                3,
                nullable.ToString(),
                null,
                nullable
            };
            var first = true;
            Console.Write("With nulls   : ");
            foreach (var value in input)
            {
                Console.Write(
                    $"{(first ? "" : ", ")}{(value == null ? "null" : ("\"" + value + "\""))}"
                );
                first = false;
            }
            Console.WriteLine();
            Console.Write($"Without nulls: ");
            first = true;
            var nullSafe = new Exercises.RemoveNulls<object>(input);

            foreach (var value in nullSafe)
            {
                Console.Write(
                    $"{(first ? "" : ", ")}{(value == null ? "null" : ("\"" + value + "\""))}"
                );
                first = false;
            }
            Console.WriteLine();
        }

        private static void RunExercise_BinaryTree()
        {
            Console.WriteLine("Binary Tree");
            var tree = CreateBinaryTree();
            foreach (var value in tree)
            {
                Console.WriteLine($"{value:d}");
            }
        }

        public static BinaryTreeNode<DateTime> CreateBinaryTree()
        {
            return new BinaryTreeNode<DateTime>
            {
                Value = new DateTime(2020, 1, 1),
                Left = new BinaryTreeNode<DateTime>
                {
                    Value = new DateTime(2010, 1, 1),
                    Left = new BinaryTreeNode<DateTime>
                    {
                        Value = new DateTime(2000, 1, 1),
                        Left = new BinaryTreeNode<DateTime> { Value = new DateTime(2002, 1, 1) },
                        Right = new BinaryTreeNode<DateTime> { Value = new DateTime(2005, 1, 1) }
                    },
                    Right = new BinaryTreeNode<DateTime>
                    {
                        Value = new DateTime(2015, 1, 1),
                        Left = new BinaryTreeNode<DateTime> { Value = new DateTime(2012, 1, 1) },
                        Right = new BinaryTreeNode<DateTime> { Value = new DateTime(2013, 1, 1) }
                    }
                },
                Right = new BinaryTreeNode<DateTime>
                {
                    Value = new DateTime(2019, 1, 1),
                    Left = new BinaryTreeNode<DateTime>
                    {
                        Value = new DateTime(2017, 1, 1),
                        Right = new BinaryTreeNode<DateTime> { Value = new DateTime(2018, 1, 1) }
                    },
                    Right = new BinaryTreeNode<DateTime>
                    {
                        Value = new DateTime(2022, 1, 1),
                        Left = new BinaryTreeNode<DateTime> { Value = new DateTime(2021, 1, 1) },
                    }
                }
            };
        }
    }
}
