using System;
using System.Collections.Generic;
using System.Linq;
using Exercise_5B;

namespace Exercise_5C
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Exercise 5B");

            // Generate random VIP passengers
            var vipPassengers = GeneratePassengers(10);
            // Generate random regular passengers
            var generalPassengers = GeneratePassengers(1000);
            // ------------------------------------------------------

            // Create Check-in Counters object used to check-in passengers and their baggage
            var generalCheckInCounter = new CheckInCounter("All Passengers");
            var vipCheckInCounter = new CheckInCounter("V.I.P");
            // ------------------------------------------------------

            // Perform Check-in to the vip passengers
            var vipBaggage = CheckInPassengers(vipPassengers, vipCheckInCounter);
            // Perform Check-in to the regular passengers
            var generalBaggage = CheckInPassengers(generalPassengers, generalCheckInCounter);
            // ------------------------------------------------------

            // combining the checked-in baggage into one collection
            IEnumerable<Baggage> allPassengersBaggage = CombineAllBaggage(
                vipBaggage,
                generalBaggage
            );
            // ------------------------------------------------------

            // check if there are too many baggage to load manually, and need to use a Tractor to transport them
            var useTractor = ShouldLoadBaggageUsingTractor(allPassengersBaggage, minMount: 20);
            if (useTractor)
            {
                Console.WriteLine($"Loading baggage using a Tractor");
            }
            else
            {
                Console.WriteLine($"Loading baggage by hand");
            }
            // ------------------------------------------------------

            // Random passengers IDs to remove from the flight
            var bumpedPassengers = new[] { 1020, 1040, 180, 130, 1001 };
            // Remove the baggage of the removed passengers
            IEnumerable<Baggage> remainingBaggage;
            try
            {
                remainingBaggage = RemovePassengersBaggage(allPassengersBaggage, bumpedPassengers);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                remainingBaggage = allPassengersBaggage;
            }
            // ------------------------------------------------------

            Console.WriteLine("Loading baggage into airplane ...");
            PrintBaggage(remainingBaggage);
            // ------------------------------------------------------
            Console.ReadLine();
        }

        public static IEnumerable<IList<Baggage>> CheckInPassengers(
            IEnumerable<Passenger> passengers,
            CheckInCounter checkInCounter
        )
        {
            return passengers.Select(checkInCounter.CheckIn);
        }

        public static IEnumerable<Baggage> CombineAllBaggage(
            IEnumerable<IList<Baggage>> vipBaggage,
            IEnumerable<IList<Baggage>> generalBaggage
        )
        {
            return ConcatAllBaggage(vipBaggage, generalBaggage);
            // BONUS: Call InterleaveAllBaggage() instead of ConcatAllBaggage()
        }

        public static IEnumerable<Baggage> ConcatAllBaggage(
            IEnumerable<IList<Baggage>> vipBaggage,
            IEnumerable<IList<Baggage>> generalBaggage
        )
        {
            return vipBaggage
                .Where(bags => bags.Count > 0)
                .Concat(generalBaggage.Where(bags => bags.Count > 0))
                .Flatten();
        }

        // Bonus
        public static IEnumerable<Baggage> InterleaveAllBaggage(
            IEnumerable<IList<Baggage>> vipBaggage,
            IEnumerable<IList<Baggage>> generalBaggage
        )
        {
            return vipBaggage
                .Where(bags => bags.Count > 0)
                .Interleave(generalBaggage.Where(bags => bags.Count > 0))
                .Flatten();
        }

        public static bool ShouldLoadBaggageUsingTractor(IEnumerable<Baggage> baggage, int minMount)
        {
            return baggage.HasAtLeast(minMount);
        }

        // BONUS
        public static IEnumerable<Baggage> RemovePassengersBaggage(
            IEnumerable<Baggage> baggage,
            IEnumerable<int> passengersIds
        )
        {
            return baggage.RemoveByKeys(passengersIds, bag => bag.PassengerID);
        }

        public static void PrintBaggage(IEnumerable<Baggage> baggage)
        {
            baggage.ForEach(bag => Console.WriteLine(bag));
        }

        public static IEnumerable<Passenger> GeneratePassengers(int initialId)
        {
            var random = new Random(initialId);
            var passengersCount = random.Next(3, 21);
            for (int i = 0; i < passengersCount; i++)
            {
                var passengerId = initialId + i * 10;
                var passenger = new Passenger { PassengerID = passengerId, };
                var baggageCount = random.Next(4);
                passenger.Baggage = Enumerable
                    .Range(1, baggageCount)
                    .Select(baggageNumber => new Baggage
                    {
                        BaggageID = baggageNumber + passengerId,
                        PassengerID = passengerId
                    })
                    .ToList();

                yield return passenger;
            }
        }
    }

    public class Passenger : IEquatable<Passenger>
    {
        public int PassengerID { get; set; }
        public IList<Baggage> Baggage { get; set; }

        public bool Equals(Passenger other)
        {
            return other?.PassengerID == PassengerID;
        }
    }

    public class Baggage : IEquatable<Baggage>
    {
        public int BaggageID { get; set; }
        public int PassengerID { get; set; }

        public bool Equals(Baggage other)
        {
            return other?.BaggageID == BaggageID;
        }

        public override string ToString()
        {
            return $"Baggage: {BaggageID}, Passenger: {PassengerID}";
        }
    }

    public class CheckInCounter
    {
        public string Label { get; private set; }

        public CheckInCounter(string label)
        {
            Label = label;
        }

        public IList<Baggage> CheckIn(Passenger passenger)
        {
            var baggage = passenger.Baggage;
            passenger.Baggage = new List<Baggage>();
            return baggage;
        }
    }
}
