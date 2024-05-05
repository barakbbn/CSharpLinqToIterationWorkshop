using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Demo6
{
    //Gotcha - enumerating disposed data-source
    internal class Example2
    {
        public void Run()
        {
            var students = GetStudents();
            Console.WriteLine("Average grade is: {0}", students.Average(student => student.Grade));
        }

        private static IEnumerable<Student> GetStudents()
        {
            using (StreamReader reader = File.OpenText(@"Students.txt"))
            {
                return ReadStudents(reader);
            }
        }

        static IEnumerable<Student> ReadStudents(StreamReader reader)
        {
            for (string line = reader.ReadLine(); line != null; line = reader.ReadLine())
            {
                var fields = line.Split(',').Select(s => s.Trim()).ToArray();
                yield return new Student
                {
                    Name = fields[0], 
                    Email = fields[1],
                    Address = { City = fields[2], Country = fields[3] },
                    Grade = int.Parse(fields[4])
                };
            }
        }
    }
}