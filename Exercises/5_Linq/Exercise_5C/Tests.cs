using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using NUnit.Framework;
using TestsUtils;

namespace Exercise_5C
{
    [TestFixture]
    public class ExerciseTests
    {
        [Test]
        public void CheckInPassengers_ReturnsAllPassengersBaggage()
        {
            var passengers = CreateTestPassengers(5, false);
            var checkInCounter = new CheckInCounter("TEST");
            var expected = passengers.Select(passenger => passenger.Baggage);
            var actual = Program.CheckInPassengers(passengers, checkInCounter);
            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test]
        public void CombineAllBaggage_ReturnsFlattenedBaggageAfterConcatenation()
        {
            var vipPassengers = CreateTestPassengers(5, true);
            var vipBaggage = vipPassengers.Select(p => p.Baggage).ToList();
            var generalPassengers = CreateTestPassengers(8, false);
            var generalBaggage = generalPassengers.Select(p => p.Baggage).ToList();

            IEnumerable<Baggage> Concat(IList<IList<Baggage>> first, IList<IList<Baggage>> second)
            {
                var firstBaggage = first.Where(b => b.Count > 0).ToList();
                var secondBaggage = second.Where(b => b.Count > 0).ToList();

                foreach (var passengerBaggage in firstBaggage)
                {
                    foreach (var baggage in passengerBaggage)
                    {
                        yield return baggage;
                    }
                }

                foreach (var passengerBaggage in secondBaggage)
                {
                    foreach (var baggage in passengerBaggage)
                    {
                        yield return baggage;
                    }
                }
            }

            var expectedConcat = Concat(vipBaggage, generalBaggage);

            var actual = Program.ConcatAllBaggage(vipBaggage, generalBaggage);
            Assert.That(
                actual,
                Is.EqualTo(expectedConcat),
                "Make sure you filtered out empty baggage lists"
            );
        }

        [Test, Description("Bonus")]
        public void CombineAllBaggage_ReturnsFlattenedBaggageAfterInterleave()
        {
            var vipPassengers = CreateTestPassengers(5, true);
            var vipBaggage = vipPassengers.Select(p => p.Baggage).ToList();
            var generalPassengers = CreateTestPassengers(8, false);
            var generalBaggage = generalPassengers.Select(p => p.Baggage).ToList();

            IEnumerable<Baggage> Interleave(
                IList<IList<Baggage>> first,
                IList<IList<Baggage>> second
            )
            {
                var firstBaggage = first.Where(b => b.Count > 0).ToList();
                var secondBaggage = second.Where(b => b.Count > 0).ToList();
                var maxCount = Math.Max(firstBaggage.Count, secondBaggage.Count);

                int i = 0;
                for (; i < maxCount; i++)
                {
                    if (firstBaggage.Count > i)
                    {
                        foreach (var baggage in firstBaggage[i])
                        {
                            yield return baggage;
                        }
                    }
                    if (secondBaggage.Count > i)
                    {
                        foreach (var baggage in secondBaggage[i])
                        {
                            yield return baggage;
                        }
                    }
                }
            }

            var expectedInterleave = Interleave(vipBaggage, generalBaggage).ToArray();

            var actual = Program.InterleaveAllBaggage(vipBaggage, generalBaggage).ToArray();
            Warn.Unless(
                actual,
                Is.EqualTo(expectedInterleave),
                "Make sure you filtered out empty baggage lists"
            );
        }

        [Test]
        public void ShouldLoadBaggageUsingTractor_InputLengthLessThan20_ReturnsFalse()
        {
            var lengthLessThan3 = 2;
            var input = Enumerable.Range(0, lengthLessThan3).Select(i => new Baggage());
            var actual = Program.ShouldLoadBaggageUsingTractor(input, 3);
            Assert.False(actual);
        }

        [Test]
        public void ShouldLoadBaggageUsingTractor_InputLengthEquals20_ReturnsTrue()
        {
            var lengthEquals3 = 3;
            var input = Enumerable.Range(0, lengthEquals3).Select(i => new Baggage());
            var actual = Program.ShouldLoadBaggageUsingTractor(input, lengthEquals3);
            Assert.True(actual);
        }

        [Test]
        public void ShouldLoadBaggageUsingTractor_InputLengthGreaterThan20_ReturnsFalse()
        {
            var lengthGreaterThan3 = 4;
            var input = Enumerable.Range(0, lengthGreaterThan3).Select(i => new Baggage());
            var actual = Program.ShouldLoadBaggageUsingTractor(input, 3);
            Assert.True(actual);
        }

        [Test]
        public void RemovePassengersBaggage_ReturnsBaggageOfAllOtherPassengers()
        {
            var input = CreateTestPassengers(10, false).SelectMany(p => p.Baggage);
            var keys = input.Select(b => b.PassengerID).Where(id => id % 3 == 0).Distinct();
            var expected = input.Where(b => !keys.Contains(b.PassengerID));
            var actual = Program.RemovePassengersBaggage(input, keys);
            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test]
        public void PrintBaggage_PrintCorrectBaggage()
        {
            var testWriter = new StringWriter();
            var input = CreateTestPassengers(10, false).SelectMany(p => p.Baggage).ToList();

            foreach (var baggage in input)
            {
                testWriter.WriteLine(baggage);
            }
            testWriter.Flush();
            var expected = testWriter.ToString();

            testWriter = new StringWriter();
            Console.SetOut(testWriter);
            Program.PrintBaggage(input);
            testWriter.Flush();

            var actual = testWriter.ToString();
            Assert.AreEqual(expected, actual);
        }

        private List<Passenger> CreateTestPassengers(int count, bool isVip)
        {
            var passengers = new List<Passenger>();
            for (int i = 0; i < count; i++)
            {
                var passengerId = i + (isVip ? 100 : 100000) + 1;
                var passenger = new Passenger
                {
                    PassengerID = passengerId,
                    Baggage = Enumerable
                        .Range(1, i)
                        .Select(n => new Baggage
                        {
                            BaggageID = passengerId + n,
                            PassengerID = passengerId
                        })
                        .ToList()
                };
                passengers.Add(passenger);
            }

            return passengers;
        }
    }
}
