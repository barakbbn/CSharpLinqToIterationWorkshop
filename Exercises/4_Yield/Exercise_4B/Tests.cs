using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using NUnit.Framework;

namespace Exercise_4B
{
    [TestFixture]
    public class StudentsFileEnumeratorTests
    {
        private string _studentsFilePath;

        [Test]
        public void Enumerate_ProducesExpectedResults()
        {
            var studentsFilename = CreateTestStudentsFile(0, true);
            var filename = Path.GetFileName(studentsFilename);
            var grade = 50;
            var expected = new[]
            {
                new StudentInfo
                {
                    Name = "Test 1",
                    Email = "test1@unittest.com",
                    Address = { City = "Unit", Country = "Test" },
                    Grade = grade += 10
                },
                new StudentInfo
                {
                    Name = "Test 2",
                    Email = "test2@unittest.com",
                    Address = { City = "Unit", Country = "Test" },
                    Grade = grade += 10
                },
                new StudentInfo
                {
                    Name = "Test 3",
                    Email = "test3@unittest.com",
                    Address = { City = "Unit", Country = "Test" },
                    Grade = grade += 10
                },
                new StudentInfo
                {
                    Name = "Test 4",
                    Email = "test4@unittest.com",
                    Address = { City = "Unit", Country = "Test" },
                    Grade = grade += 10
                },
                new StudentInfo
                {
                    Name = "Test 5",
                    Email = "test5@unittest.com",
                    Address = { City = "Unit", Country = "Test" },
                    Grade = grade += 10
                },
            };

            WriteStudentsToFile(expected);

            var sut = CreateStudentsFileEnumerator(studentsFilename);
            var actual = (sut as IEnumerable<StudentInfo>).ToArray();

            Assert.That(
                actual,
                Is.EqualTo(expected).Using(new TestHelper.StudentInfoEqualityComparer())
            );
        }

        [Test]
        public void Enumerate_EmptyFile_ProducesNoValues()
        {
            var studentsFilename = CreateTestStudentsFile(0, false);
            var sut = CreateStudentsFileEnumerator(studentsFilename);
            var actual = (sut as IEnumerable<StudentInfo>).ToArray();
            Assert.IsEmpty(actual);
        }

        [Test]
        public void Enumerate_FileOnlyWithHeader_ProducesNoValues()
        {
            var studentsFilename = CreateTestStudentsFile(0, true);
            var sut = CreateStudentsFileEnumerator(studentsFilename);
            var actual = (sut as IEnumerable<StudentInfo>);
            Assert.IsEmpty(actual);
        }

        [Test]
        public void Enumerate_EnumerateAgain_ProducesSameResults()
        {
            var studentsFilename = CreateTestStudentsFile(5, true);
            var sut = CreateStudentsFileEnumerator(studentsFilename);
            var expected = (sut as IEnumerable<StudentInfo>);

            var actual = (sut as IEnumerable<StudentInfo>);

            Assert.That(
                actual,
                Is.EqualTo(expected).Using(new TestHelper.StudentInfoEqualityComparer())
            );
        }

        [Test]
        public void Enumerate_ChangeInput_ProducesCorrectResultsForChangedInput()
        {
            var studentsFilename = CreateTestStudentsFile(5);
            var sut = CreateStudentsFileEnumerator(studentsFilename);
            TestHelper.Enumerate(sut);

            var filename = Path.GetFileName(studentsFilename);
            var grade = 100;
            var expected = new[]
            {
                new StudentInfo
                {
                    Name = "Test 1",
                    Email = "test1@unittest.com",
                    Address = { City = "Unit", Country = "Test" },
                    Grade = grade -= 10
                },
                new StudentInfo
                {
                    Name = "Test 2",
                    Email = "test2@unittest.com",
                    Address = { City = "Unit", Country = "Test" },
                    Grade = grade -= 10
                },
                new StudentInfo
                {
                    Name = "Test 3",
                    Email = "test3@unittest.com",
                    Address = { City = "Unit", Country = "Test" },
                    Grade = grade -= 10
                }
            };

            WriteStudentsToFile(expected, false);

            var actual = (sut as IEnumerable<StudentInfo>);
            Assert.That(
                actual,
                Is.EqualTo(expected).Using(new TestHelper.StudentInfoEqualityComparer())
            );
        }

        [Test]
        public void Create_FileNotOpened()
        {
            var tempFilename = CreateTestStudentsFile();
            // Lock file access
            using (
                var stream = File.Open(tempFilename, FileMode.Open, FileAccess.Read, FileShare.None)
            )
            {
                Assert.DoesNotThrow(
                    () => CreateStudentsFileEnumerator(tempFilename),
                    "Expected file not the be accessed before iteration starts"
                );
            }
        }

        [Test]
        public void GetEnumerator_FileNotOpened()
        {
            var tempFilename = CreateTestStudentsFile();
            // Lock file access
            using (
                var stream = File.Open(tempFilename, FileMode.Open, FileAccess.Read, FileShare.None)
            )
            {
                Assert.DoesNotThrow(
                    () =>
                    {
                        var sut =
                            CreateStudentsFileEnumerator(tempFilename) as IEnumerable<StudentInfo>;
                        sut.GetEnumerator();
                    },
                    "Expected file not the be accessed before iteration starts"
                );
            }
        }

        [Test]
        public void MoveNext_CalledOnce_FileNotFullyIterated()
        {
            var tempFilename = CreateTestStudentsFile();
            var sut = new TestableStudentsFileEnumeratorTests(tempFilename);
            using (var enumerator = (sut as IEnumerable<StudentInfo>).GetEnumerator())
            {
                var hasNext = enumerator.MoveNext();
                Assert.True(hasNext);

                // If File was already eagerly read, it will only include the first record, not the next one we append
                using (var writer = new StreamWriter(tempFilename, true))
                {
                    writer.WriteLine("Unit Test2,test2@unit.com,Unit,Test,0");
                    writer.Flush();
                }

                hasNext = enumerator.MoveNext();
                Assert.True(
                    hasNext,
                    "Expected the file lines will be read only when needed and not in advance"
                );
                Assert.Positive(
                    sut.OpenStudentsFileCallCount,
                    "Expected to call OpenStudentsFile() method"
                );
            }
        }

        [Test]
        public void Enumerate_InputEnumeratorDisposed()
        {
            var tempFilename = CreateTestStudentsFile();

            var sut = new TestableStudentsFileEnumeratorTests(tempFilename);
            TestHelper.Enumerate(sut);
            Warn.If(
                sut._studentsStreamReader.BaseStream != null,
                "Expected students file to be closed/disposed when finished iterating all students"
            );
        }

        [Test]
        public void Enumerate_MoveNextReturnedFalse_InputEnumeratorDisposed()
        {
            var tempFilename = CreateTestStudentsFile();
            var sut = new TestableStudentsFileEnumeratorTests(tempFilename);
            using (var enumerator = (sut as IEnumerable<StudentInfo>).GetEnumerator())
            {
                while (enumerator.MoveNext())
                    ;
                Warn.If(
                    sut._studentsStreamReader.BaseStream != null,
                    "Expected students file to be closed/disposed when finished iterating all students"
                );
            }
        }

        [TearDown]
        public void TearDown()
        {
            if (_studentsFilePath != null)
                File.Delete(_studentsFilePath);
        }

        private string CreateTestStudentsFile(int numberOfStudents = 1, bool header = true)
        {
            return _studentsFilePath = TestHelper.CreateTestStudentsFile(
                Path.GetTempPath(),
                numberOfStudents,
                header,
                0
            );
        }

        private void WriteStudentsToFile(IEnumerable<StudentInfo> students, bool append = true)
        {
            var lines = TestHelper.StudentsToCsvLines(students);
            if (append)
            {
                File.AppendAllLines(_studentsFilePath, lines);
            }
            else
            {
                // Add header
                lines.Insert(0, StudentsFileEnumerator.CSV_HEADER);
                File.WriteAllLines(_studentsFilePath, lines);
            }
        }

        private StudentsFileEnumerator CreateStudentsFileEnumerator(string studentsFilename)
        {
            return new StudentsFileEnumerator(studentsFilename);
        }

        private class TestableStudentsFileEnumeratorTests : StudentsFileEnumerator
        {
            internal StreamReader _studentsStreamReader;

            public TestableStudentsFileEnumeratorTests(string studentsCsvFilePath)
                : base(studentsCsvFilePath) { }

            public int OpenStudentsFileCallCount { get; private set; }

            protected override StreamReader OpenStudentsFile(string filePath)
            {
                OpenStudentsFileCallCount++;
                var stream = File.Open(
                    filePath,
                    FileMode.Open,
                    FileAccess.Read,
                    FileShare.ReadWrite
                );
                return _studentsStreamReader = new StreamReader(stream);
            }
        }
    }

    internal static class TestHelper
    {
        public static void Enumerate(object obj)
        {
            var enumerable = obj as IEnumerable;
            if (enumerable != null)
            {
                enumerable.OfType<object>().ToList();
            }
        }

        internal class StudentInfoEqualityComparer : IEqualityComparer<StudentInfo>
        {
            public bool Equals(StudentInfo x, StudentInfo y)
            {
                if (x is null || y is null)
                    return (x is null && y is null);
                return x.Name.Equals(y.Name)
                    && x.Email.Equals(y.Email)
                    && x.Address.City.Equals(y.Address.City)
                    && x.Address.Country.Equals(y.Address.Country)
                    && x.Grade.Equals(y.Grade);
            }

            public int GetHashCode(StudentInfo obj)
            {
                if (obj is null)
                    return 0;
                return StudentToCsvLine(obj).GetHashCode();
            }
        }

        public static string CreateTestStudentsFile(
            string folderPath,
            int numberOfStudents,
            bool header,
            int dataGenerationSeed = 0
        )
        {
            IList<StudentInfo> students;
            return CreateTestStudentsFile(
                folderPath,
                numberOfStudents,
                header,
                dataGenerationSeed,
                out students
            );
        }

        public static string CreateTestStudentsFile(
            string folderPath,
            int numberOfStudents,
            bool header,
            int dataGenerationSeed,
            out IList<StudentInfo> students
        )
        {
            var filePath = Path.Combine(
                folderPath,
                Path.ChangeExtension(Path.GetRandomFileName(), "csv")
            );
            var random = new Random(dataGenerationSeed);
            students = new List<StudentInfo>();
            using (var stream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
            using (var writer = new StreamWriter(stream))
            {
                if (header)
                    writer.WriteLine(StudentsFileEnumerator.CSV_HEADER);
                for (int i = 1; i <= numberOfStudents; i++)
                {
                    var grade = random.Next(101);
                    var student = new StudentInfo
                    {
                        Name = $"Unit Test{i + dataGenerationSeed}",
                        Email = $"test{i + dataGenerationSeed}@unit.com",
                        Address = { City = "Unit", Country = "Test" },
                        Grade = grade
                    };
                    students.Add(student);
                    var line = StudentToCsvLine(student);
                    writer.WriteLine(line);
                }
                writer.Flush();
            }
            return filePath;
        }

        public static string StudentToCsvLine(StudentInfo student)
        {
            return $"{student.Name},{student.Email},{student.Address.City},{student.Address.Country},{student.Grade}";
        }

        public static IList<string> StudentsToCsvLines(IEnumerable<StudentInfo> students)
        {
            return students.Select(TestHelper.StudentToCsvLine).ToList();
        }
    }
}
