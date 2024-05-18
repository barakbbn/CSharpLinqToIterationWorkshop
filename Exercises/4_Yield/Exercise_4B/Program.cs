using System;
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

    // TODO: Implement IEnumerable<StudentInfo>
    public class StudentsFileEnumerator
    {
        public const string CSV_HEADER = "Name,Email,City,Country,Grade";

        // TODO: Implement Constructor
        public StudentsFileEnumerator(string studentsCsvFilePath)
        {
            throw new NotImplementedException();
        }

        // ===============================================
        // !!!!! USE THIS METHOD TO OPEN THE STUDENTS FILE !!!!!!
        // unit-tests depends on it to verify correct implementation
        protected virtual StreamReader OpenStudentsFile(string filePath)
        {
            // TODO: Open students file and return StreamReader;
            throw new NotImplementedException();
            // HINT: To open the CSV file as Stream, search the Internet (or scroll down-down in README.md)
        }

        // ===============================================

        public IEnumerator<StudentInfo> GetEnumerator()
        {
            // 1. Open student file: OpenStudentFile() - Remember to dispose the StreamReader when done
            // 1.1 Skip 1st line, it's a csv header
            // 2. Read lines from the StreamReader in a loop (Ignore empty lines)
            //    3. For each CSV line, parse it into an instance of a StudentInfo
            //    4. yield return each student

            throw new NotImplementedException();

            // Need some more assistance?
            // 1. Try out an AI Chat (copilot.microsoft.com , gemini.google.com, deepai.org, chatgpt.com)
            // 2. Scroll way way down to ReadCsvLines() method, see how it is implemented (it doesn't use `yield` keyword, but your code should)
            // 3. See how ParseStudent() method is implemented
            // 4. Ask the instructor for guidance


            // Still struggle, see GetStudentsEnumerator() method (it doesn't use `yield` keyword, but your code should)
        }

        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //

        private IEnumerable<string> ReadCsvLines(StreamReader reader)
        {
            reader.ReadLine(); // Skip header line
            var csvLines = new List<string>();
            string csvLine;
            while ((csvLine = reader.ReadLine()) != null)
            {
                if (!string.IsNullOrEmpty(csvLine))
                {
                    csvLines.Add(csvLine);
                }
            }

            return csvLines;
        }

        private StudentInfo ParseStudent(string csvLine)
        {
            var csvFields = csvLine.Split(','); // csvFields[0] -> Name , csvFields[1] -> Email , csvFields[2] -> City , csvFields[3] -> Country , csvFields[4] -> Grade

            var student = new StudentInfo
            {
                Name = csvFields[0],
                Email = csvFields[1],
                Address = new Address { City = csvFields[2], Country = csvFields[3], },
                Grade = int.Parse(csvFields[4])
            };

            return student;
        }

        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //

        private IEnumerator<StudentInfo> GetStudentsEnumerator()
        {
            string filename = "???"; // TODO: get filename passed to the constructor

            var students = new List<StudentInfo>();

            using (var reader = OpenStudentsFile(filename))
            {
                reader.ReadLine(); // Skip CSV Header line
                string csvLine;
                do
                {
                    csvLine = reader.ReadLine();
                    if (!string.IsNullOrEmpty(csvLine))
                    {
                        var student = ParseStudent(csvLine);
                        students.Add(student);
                    }
                } while (csvLine != null);
            }

            return students.GetEnumerator();
        }
    }
}
