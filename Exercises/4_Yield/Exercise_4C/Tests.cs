using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Exercise_4B;
using NUnit.Framework;

namespace Exercise_4C
{
    [TestFixture]
    public class Tests
    {
        [Test]
        public void Convert_SimpleConverter_ConvertedResults()
        {
            var input = new[] { 1, 2, 3 };
            Func<int, int> converter = x => x * 2;
            var expected = input.Select(converter);
            var actual = Program.Convert(input, converter);
            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test]
        public void Convert_ComplexConverter_ConvertedResults()
        {
            var input = new[] { "Sunday", "Mon", "" };
            Func<string, int> converter = x => x.Length;
            var expected = input.Select(converter);
            var actual = Program.Convert(input, converter);
            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test]
        public void Convert_EmptyInput_EmptyResult()
        {
            var actual = Program
                .Convert<int, int>(
                    Enumerable.Empty<int>(),
                    x =>
                    {
                        if (x != -99999)
                            throw new InvalidOperationException();
                        return x;
                    }
                )
                .ToArray();
            Assert.IsEmpty(actual);
        }

        [Test]
        public void Convert_NullInput_Throw()
        {
            try
            {
                Program.Convert<int, int>(null, x => x);
                Warn.If(true, "Expected ArgumentNullException");
            }
            catch (Exception ex)
            {
                Warn.Unless(
                    ex is ArgumentNullException,
                    $"Expected ArgumentNullException but got:\n  {ex}"
                );
            }
        }

        [Test]
        public void Convert_NullConverter_Throw()
        {
            var input = new[] { 1, 2, 3 };
            try
            {
                Program.Convert<int, int>(input, null);
                Warn.If(true, "Expected ArgumentNullException");
            }
            catch (Exception ex)
            {
                Warn.Unless(
                    ex is ArgumentNullException,
                    $"Expected ArgumentNullException but got:\n  {ex}"
                );
            }
        }

        [Test]
        public void StudentsByGrades_NullInput_Throw()
        {
            try
            {
                Program.StudentsByGrades(null, 0, 100);
                Warn.If(true, "Expected ArgumentNullException");
            }
            catch (Exception ex)
            {
                Warn.Unless(
                    ex is ArgumentNullException,
                    $"Expected ArgumentNullException but got:\n  {ex}"
                );
            }
        }

        [Test]
        public void StudentsByGrades_EmptyInput_EmptyResult()
        {
            var input = Enumerable.Empty<StudentInfo>();
            var actual = Program.StudentsByGrades(input, 0, 100);

            Assert.IsEmpty(actual);
        }

        [Test]
        public void StudentsByGrades_ForRangeOfGrades_ResultsOnlyWithGradesInRange()
        {
            var input = CreateTestStudents();
            int fromGrade = 50;
            int toGrade = 90;

            var expected = input.Where(s => s.Grade >= fromGrade && s.Grade <= toGrade);
            var actual = Program.StudentsByGrades(input, fromGrade, toGrade);
            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test]
        public void SideEffect_EmptyInput_ActionNotCalled()
        {
            var input = Enumerable.Empty<int>();
            bool actionCalled = false;
            var sut = Program.SideEffect(input, x => actionCalled = true);
            sut.ToArray();
            Assert.False(actionCalled);
        }

        [Test]
        public void SideEffect_ReturnSameAsInput()
        {
            var input = new[] { 1, 2, 3 };
            Action<int> emptyAction = x => { };
            var sut = Program.SideEffect(input, emptyAction);
            var actual = sut.ToArray();
            Assert.That(actual, Is.EqualTo(input));
        }

        [Test]
        public void SideEffect_ActionCalledForEachItem()
        {
            var input = new[] { 1, 2, 3 };
            var expected = new List<int>(input.Length);

            var sut = Program.SideEffect(input, expected.Add);
            var actual = sut.ToArray();
            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test]
        public void SideEffect_NullInput_Throw()
        {
            try
            {
                Program.SideEffect<int>(null, x => { });
                Warn.If(true, "Expected ArgumentNullException");
            }
            catch (Exception ex)
            {
                Warn.Unless(
                    ex is ArgumentNullException,
                    $"Expected ArgumentNullException but got:\n  {ex}"
                );
            }
        }

        [Test]
        public void SideEffect_NullAction_Throw()
        {
            var input = new[] { 1, 2, 3 };
            try
            {
                Program.SideEffect<int>(input, null);
                Warn.If(true, "Expected ArgumentNullException");
            }
            catch (Exception ex)
            {
                Warn.Unless(
                    ex is ArgumentNullException,
                    $"Expected ArgumentNullException but got:\n  {ex}"
                );
            }
        }

        private List<StudentInfo> CreateTestStudents()
        {
            return Enumerable
                .Range(0, 10)
                .Select(i => new StudentInfo
                {
                    Name = $"Test {i + 1}",
                    Email = $"test{i + 1}@unittest.com",
                    Address = { City = "Unit", Country = "Test" },
                    Grade = i * 10
                })
                .ToList();
        }
    }
}
