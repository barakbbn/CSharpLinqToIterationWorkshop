using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace Exercise_4B
{
    public class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Exercise 4B");
        }
    }

    public class StudentInfo
    {
        public int Grade { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public Address Address { get; set; } = new Address();

        public override string ToString()
        {
            return $"{Name}, {Email}, Address: {Address.City}, {Address.Country}, Grade: {Grade}";
        }
    }

    public class Address
    {
        public string Country { get; set; }
        public string City { get; set; }
    }

    public class StudentsFileEnumerator : IEnumerable<StudentInfo>
    {
        public const string CSV_HEADER = "Name,Email,City,Country,Grade";

        private readonly string _studentsCsvFilePath;

        public StudentsFileEnumerator(string studentsCsvFilePath)
        {
            if (string.IsNullOrWhiteSpace(studentsCsvFilePath))
            {
                throw new ArgumentException(
                    "File path cannot be null or empty.",
                    nameof(studentsCsvFilePath)
                );
            }
            _studentsCsvFilePath = studentsCsvFilePath;
        }

        // ===============================================
        // !!!!! USE THIS METHOD TO OPEN THE STUDENTS FILE !!!!!!
        // unit-tests depends on it to verify correct implementation
        protected virtual StreamReader OpenStudentsFile(string filePath)
        {
            return File.OpenText(filePath);
        }

        public IEnumerator<StudentInfo> GetEnumerator()
        {
            using (var reader = OpenStudentsFile(_studentsCsvFilePath))
            {
                var header = reader.ReadLine();
                for (string line = reader.ReadLine(); line != null; line = reader.ReadLine())
                {
                    if (string.IsNullOrEmpty(line))
                    {
                        continue;
                    }
                    yield return ParseStudent(line);
                }
            }
        }

        private StudentInfo ParseStudent(string line)
        {
            var fields = line.Split(',');
            return new StudentInfo
            {
                Name = fields[0].Trim(),
                Email = fields[1].Trim(),
                Address = { City = fields[2].Trim(), Country = fields[3].Trim() },
                Grade = int.Parse(fields[4].Trim())
            };
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
