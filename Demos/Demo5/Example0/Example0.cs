using System;
using System.Collections.Generic;

namespace Demo5.Example0
{
    public class Example0
    {
        public void Run()
        {
            IEnumerable<DayOfWeek> enumerable = WorkingDays();
            var rator1 = (IEnumerable<DayOfWeek>)enumerable.GetEnumerator();
            var rator2 = (IEnumerable<DayOfWeek>)enumerable.GetEnumerator();
            foreach (var workingDay1 in rator1)
            {
                foreach (var workingDay2 in rator2)
                {
                    Console.WriteLine($"{workingDay1} : {workingDay2}");
                }
            }
        }

        IEnumerable<DayOfWeek> WorkingDays()
        {
            Console.WriteLine("yield return DayOfWeek.Sunday");
            yield return DayOfWeek.Sunday;
            Console.WriteLine("yield return DayOfWeek.Monday");
            yield return DayOfWeek.Monday;
            Console.WriteLine("yield return DayOfWeek.Tuesday");
            yield return DayOfWeek.Tuesday;
            Console.WriteLine("yield return DayOfWeek.Wednesday");
            yield return DayOfWeek.Wednesday;
            Console.WriteLine("yield return DayOfWeek.Thursday");
            yield return DayOfWeek.Thursday;
            Console.WriteLine("Finished WorkingDays enumeration");
        }
    }
}