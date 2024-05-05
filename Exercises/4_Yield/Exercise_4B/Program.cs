using System;
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


        // HINTS. you might want to breakdown the logic to small methods
        // private StudentInfo ParseStudent(string csvLine) {
        //     var csvFields = csvLine.Split(","); // csvFields[0] -> Name , csvFields[1] -> Email , csvFields[2] -> City , csvFields[3] -> Country , csvFields[4] -> Grade
        //     // TODO: do the rest
        //}
        // HINT: Remember the first line in the CSV file is header.
        // HINT: It could be that some lines in the CSV file are empty!
    }
}
