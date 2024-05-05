using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Demo6
{
    //multiple enumeration of same IEnumerable
    public class Example3
    {
        public void Run()
        {
            IEnumerable<Student> students = GetStudents();

            foreach (Student student in students)
            {
                if (student.Grade < 55) student.Grade = 55; //You can never fail
                Console.WriteLine("{0} grade is: {1}", student.Name, student.Grade);
            }

            Console.WriteLine();

            // ReSharper warn about possible multiple enumerations
            foreach (Student student in students.Where(s => s.Grade < 55))
            {
                SendEmail(student.Email, "You are expelled");
            }
        }

        private static IEnumerable<Student> GetStudents()
        {
            using (StreamReader reader = File.OpenText(@"Students.txt"))
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

        private void SendEmail(string email, string message)
        {
            Console.WriteLine("Email to {0} - {1}", email, message);
        }
    }
}