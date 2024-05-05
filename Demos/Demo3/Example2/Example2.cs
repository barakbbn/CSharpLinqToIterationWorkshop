using System;
using System.Collections.Generic;
using System.Drawing;

namespace Demo3
{
    /// <summary>
    /// Iterator blocks and extension methods
    /// </summary>
    public class Example2
    {
        public void Run()
        {
            Console.WriteLine("Iterating a range using Range method");
            Console.WriteLine("-------------------------");

            foreach (var value in Range(1, 3))
            {
                Console.WriteLine(value);
            }
            Console.WriteLine();


            Console.WriteLine("Iterating Conrners of a Rectangle");
            Console.WriteLine("-------------------------");

            var rect = new Rectangle(10, 100, 90, 100);
            Console.WriteLine($"Rectangle: {rect}");
            Console.WriteLine("Corners:");
            foreach (var value in GetCorners(rect))
            {
                Console.WriteLine(value);
            }
            Console.WriteLine();
        }

        public static IEnumerable<int> Range(int from, int count)
        {
            for (int max = @from + count; @from < max; @from++)
            {
                yield return @from;
            }
        }


        public static IEnumerable<Point> GetCorners(Rectangle rect)
        {
            yield return rect.Location; // Top Left
            yield return new Point(rect.Right, rect.Top); // Top Right
            yield return new Point(rect.Left, rect.Bottom); // Bottom Left
            yield return new Point(rect.Right, rect.Bottom); // Bottom Right
        }
    }
}