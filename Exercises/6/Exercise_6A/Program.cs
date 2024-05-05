using System;
using System.Collections.Generic;
using System.Linq;

namespace Exercise_6A
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Exercise 6A");

            // =========== Challenge 1 ====================
            Console.WriteLine("-- Challenge 1 --");

            GenerateContainers(50)
                .LoadContainerOntoClassAShips()
                .ForEach(ship => ship.SailToDestination());

            // =========== Challenge 1 BONUS ==============
            Console.WriteLine("-- Challenge 1: BONUS --");
            GenerateContainers(50)
                .LoadContainerOntoClassAShipsBonus()
                .ForEach(ship => ship.SailToDestination());

            // =========== Challenge 2 ====================
            Console.WriteLine("-- Challenge 2 --");
            GenerateContainers(200)
                .LoadContainerOntoClassBShips()
                .ForEach(ship => ship.SailToDestination());

            // =========== Challenge 3 ====================
            Console.WriteLine("-- Challenge 3 --");
            var faultyShipId = Ship.NextId + 2;
            GenerateContainers(50)
                .LoadContainerOntoClassAShips()
                .ReplaceShip(faultyShipId, new ShipClassA())
                .ForEach(ship => ship.SailToDestination());

            // =========== Challenge 4 ====================
            Console.WriteLine("-- Challenge 4 --");
            var shipsClassA = GenerateContainers(50)
                .LoadContainerOntoClassAShips()
                .ReplaceShip(2, new ShipClassA());
            var shipsClassB = GenerateContainers(50).LoadContainerOntoClassBShips();
            ILookup<ShipDestination, Ship> shipsByDestination =
                shipsClassA.JoinAndGroupShipsByDestination(shipsClassB);
            shipsByDestination
                .OrderShipsInEachDestination()
                .OrderByDestination()
                .ForEach(shipsForDestination =>
                {
                    Console.WriteLine(
                        $"Shipping {shipsForDestination.Value.Count()} ships to {shipsForDestination.Key} ..."
                    );
                    shipsForDestination.Value.ForEach(ship => ship.SailToDestination());
                });

            Console.ReadLine();
        }

        static IEnumerable<ZimContainer> GenerateContainers(int size)
        {
            var random = new Random(0);
            var id = 1;
            var validDestinations = Enum.GetValues(typeof(ShipDestination))
                .OfType<ShipDestination>()
                .Where(d => d != ShipDestination.NotSet)
                .ToArray();

            for (int i = 0; i < size; i++)
            {
                yield return new ZimContainer(id++)
                {
                    Weight = random.Next(1, 101) / 20 * 20,
                    ShipTo = validDestinations[random.Next(0, validDestinations.Length)]
                };
            }
        }

        public static IEnumerable<ShipClassA> LoadContainerOntoClassAShips(
            this IEnumerable<ZimContainer> containers
        )
        {
            // TODO:
            throw new NotImplementedException();
        }

        public static IEnumerable<IReadOnlyCollection<ZimContainer>> ChunkContainers(
            this IEnumerable<ZimContainer> containers,
            int size
        )
        {
            // TODO:
            throw new NotImplementedException();
        }

        public static IEnumerable<ShipClassA> LoadContainerOntoClassAShipsBonus(
            this IEnumerable<ZimContainer> containers
        )
        {
            // TODO:
            throw new NotImplementedException();
        }

        public static IEnumerable<ShipClassB> LoadContainerOntoClassBShips(
            this IEnumerable<ZimContainer> containers
        )
        {
            // TODO:
            throw new NotImplementedException();
        }

        public static IEnumerable<IReadOnlyCollection<ZimContainer>> ChunkByWeight(
            this IEnumerable<ZimContainer> containers,
            int maxWeight,
            int maxContainers
        )
        {
            // TODO:
            throw new NotImplementedException();
        }

        public static IEnumerable<T> ReplaceShip<T>(
            this IEnumerable<T> ships,
            int shipId,
            T newShip
        )
            where T : Ship
        {
            // TODO:
            throw new NotImplementedException();
        }

        public static ILookup<ShipDestination, Ship> JoinAndGroupShipsByDestination(
            this IEnumerable<Ship> ships,
            IEnumerable<Ship> moreShips
        )
        {
            // TODO:
            throw new NotImplementedException();
        }

        public static IReadOnlyDictionary<
            ShipDestination,
            IEnumerable<Ship>
        > OrderShipsInEachDestination(this IEnumerable<IGrouping<ShipDestination, Ship>> ships)
        {
            // TODO:
            throw new NotImplementedException();
        }

        public static IOrderedEnumerable<Ship> OrderShipsByPriority(this IEnumerable<Ship> ships)
        {
            // TODO:
            throw new NotImplementedException();
        }

        public static IEnumerable<
            KeyValuePair<ShipDestination, TDontCare>
        > OrderByDestination<TDontCare>(
            this IEnumerable<KeyValuePair<ShipDestination, TDontCare>> groupsOfShips
        )
        {
            // TODO:
            throw new NotImplementedException();
        }
    }

    public static class Extensions
    {
        // =========== Copy from Exercise 5B ====================
        public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            // TODO:
            throw new NotImplementedException();
        }

        // =========== BONUS 2 ====================
        public static IEnumerable<IReadOnlyCollection<T>> Chunk<T>(
            this IEnumerable<T> source,
            int size
        )
        {
            // TODO:
            throw new NotImplementedException();
        }

        // =========== BONUS 3 ====================
        public static IEnumerable<IReadOnlyCollection<T>> ChunkByLimit<T>(
            this IEnumerable<T> source,
            int limit,
            int maxChunkSize,
            Func<T, int> selector
        )
        {
            // TODO: BONUS
            throw new NotImplementedException();
        }

        // =========== BONUS 4 ====================
        public static IEnumerable<IReadOnlyCollection<T>> ChunkByLimit<T>(
            this IEnumerable<T> source,
            long limit,
            int maxChunkSize,
            Func<T, long> selector
        )
        {
            // TODO: BONUS
            throw new NotImplementedException();
        }

        public static IEnumerable<IReadOnlyCollection<T>> ChunkByLimit<T>(
            this IEnumerable<T> source,
            decimal limit,
            int maxChunkSize,
            Func<T, decimal> selector
        )
        {
            // TODO: BONUS
            throw new NotImplementedException();
        }
    }

    public enum ShipDestination
    {
        NotSet,
        Usa,
        FarEast,
        Europe,
        Afrika,
        SECRET_LOCATION
    }

    public class ZimContainer
    {
        public ZimContainer(int id)
        {
            Id = id;
        }

        public int Id { get; set; }

        public int Weight { get; set; }

        public ShipDestination ShipTo { get; set; }
    }

    public abstract class Ship
    {
        public static int NextId { get; protected set; } = 1;

        protected LinkedList<ZimContainer> _containersOnboard = new LinkedList<ZimContainer>();

        public int Id { get; }
        public ShipDestination Destination { get; set; }
        public IReadOnlyCollection<ZimContainer> ContainersOnboard
        {
            get { return _containersOnboard; }
        }

        public Ship()
        {
            Id = NextId++;
        }

        public Ship(ShipDestination destination)
            : this()
        {
            Destination = destination;
        }

        public abstract void LoadContainers(IEnumerable<ZimContainer> containers);

        public abstract void SailToDestination();

        protected virtual void VerifyDestination() { }

        public override string ToString()
        {
            return $"${GetType().Name} {Id}, Destination: {Destination}";
        }
    }

    public class ShipClassA : Ship
    {
        public const int MAX_CONTAINERS = 10;

        public ShipClassA()
            : base() { }

        public ShipClassA(ShipDestination destination)
            : base(destination)
        {
            Destination = destination;

            VerifyDestination();
        }

        protected override void VerifyDestination()
        {
            base.VerifyDestination();
            if (Destination == ShipDestination.SECRET_LOCATION)
            {
                throw new InvalidOperationException(
                    $"Ship not allowed to sail to destination {ShipDestination.SECRET_LOCATION}"
                );
            }
        }

        public override void LoadContainers(IEnumerable<ZimContainer> containers)
        {
            VerifyDestination();
            foreach (var container in containers)
            {
                if (container.ShipTo != Destination)
                {
                    throw new InvalidOperationException(
                        $"Container is on the wrong ship destination"
                    );
                }
                _containersOnboard.AddLast(container);
            }
        }

        public override void SailToDestination()
        {
            VerifyDestination();
            if (ContainersOnboard.Count > MAX_CONTAINERS)
            {
                throw new InvalidOperationException(
                    "Ship had sunk as result of too many containers"
                );
            }
            Console.WriteLine(
                $"Ship A {Id} is sailing to {Destination} with {ContainersOnboard.Count}/{MAX_CONTAINERS} containers and total weight of {ContainersOnboard.Sum(c => c.Weight)} ..."
            );
        }
    }

    public class ShipClassB : Ship
    {
        public const int MAX_CONTAINERS = 20;
        public const int MAX_WEIGHT = 1000;

        public ShipClassB()
            : base() { }

        public ShipClassB(ShipDestination destination)
            : base(destination) { }

        public int TotalWeight { get; protected set; }

        public override void LoadContainers(IEnumerable<ZimContainer> containers)
        {
            VerifyDestination();
            foreach (var container in containers)
            {
                if (container.ShipTo != Destination)
                {
                    throw new InvalidOperationException(
                        $"Container is on the wrong ship destination"
                    );
                }
                _containersOnboard.AddLast(container);
                TotalWeight += container.Weight;
            }
        }

        public override void SailToDestination()
        {
            VerifyDestination();
            if (ContainersOnboard.Count > MAX_CONTAINERS)
            {
                throw new InvalidOperationException(
                    "Ship had sunk as result of too many containers"
                );
            }
            if (TotalWeight > MAX_WEIGHT)
            {
                throw new InvalidOperationException("Ship had sunk as result of overweight");
            }
            Console.WriteLine(
                $"Ship B {Id} is sailing to {Destination} with {ContainersOnboard.Count}/{MAX_CONTAINERS} containers and total weight of {TotalWeight}/{MAX_WEIGHT} ..."
            );
        }
    }
}
