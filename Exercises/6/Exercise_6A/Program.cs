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
            var batchesOfShips = containers
                .Where(container => container.ShipTo != ShipDestination.SECRET_LOCATION)
                .GroupBy(c => c.ShipTo)
                .Select(group =>
                    group
                        .ChunkContainers(ShipClassA.MAX_CONTAINERS)
                        .Select(chunk =>
                        {
                            var ship = new ShipClassA(group.Key);
                            ship.LoadContainers(chunk);
                            return ship;
                        })
                );

            foreach (var batch in batchesOfShips)
            {
                foreach (var ship in batch)
                {
                    yield return ship;
                }
            }
        }

        public static IEnumerable<IReadOnlyCollection<ZimContainer>> ChunkContainers(
            this IEnumerable<ZimContainer> containers,
            int size
        )
        {
            var chunk = new List<ZimContainer>(size);
            foreach (var item in containers)
            {
                chunk.Add(item);
                if (chunk.Count == size)
                {
                    yield return chunk;
                    chunk = new List<ZimContainer>(size);
                }
            }
            if (chunk.Count > 0)
            {
                yield return chunk;
            }
        }

        public static IEnumerable<ShipClassA> LoadContainerOntoClassAShipsBonus(
            this IEnumerable<ZimContainer> containers
        )
        {
            return containers
                .Where(container => container.ShipTo != ShipDestination.SECRET_LOCATION)
                .GroupBy(c => c.ShipTo)
                .SelectMany(
                    group => group.ChunkContainers(ShipClassA.MAX_CONTAINERS),
                    (group, chunk) =>
                    {
                        var ship = new ShipClassA(chunk.First().ShipTo);
                        ship.LoadContainers(chunk);
                        return ship;
                    }
                );
        }

        public static IEnumerable<ShipClassB> LoadContainerOntoClassBShips(
            this IEnumerable<ZimContainer> containers
        )
        {
            var batchesOfContainers = containers
                .GroupBy(c => c.ShipTo)
                .SelectMany(group =>
                    group.ChunkByWeight(ShipClassB.MAX_WEIGHT, ShipClassB.MAX_CONTAINERS)
                );

            foreach (var chunk in batchesOfContainers)
            {
                var ship = new ShipClassB(chunk.First().ShipTo);
                ship.LoadContainers(chunk);
                yield return ship;
            }
        }

        public static IEnumerable<IReadOnlyCollection<ZimContainer>> ChunkByWeight(
            this IEnumerable<ZimContainer> containers,
            int maxWeight,
            int maxContainers
        )
        {
            int sum = 0;
            var chunk = new List<ZimContainer>(maxContainers);

            foreach (var container in containers)
            {
                var nextSum = sum + container.Weight;
                if (nextSum > maxWeight || chunk.Count == maxContainers)
                {
                    bool isEmpty = chunk.Count == 0;
                    if (isEmpty)
                    {
                        chunk.Add(container);
                    }
                    yield return chunk;
                    chunk = new List<ZimContainer>(maxContainers);
                    if (!isEmpty)
                    {
                        chunk.Add(container);
                        sum = container.Weight;
                    }
                    else
                    {
                        sum = 0;
                    }
                }
                else
                {
                    sum = nextSum;
                    chunk.Add(container);
                }
            }

            if (chunk.Count > 0)
            {
                yield return chunk;
            }
        }

        public static IEnumerable<T> ReplaceShip<T>(
            this IEnumerable<T> ships,
            int shipId,
            T newShip
        )
            where T : Ship
        {
            foreach (var ship in ships)
            {
                if (ship.Id == shipId)
                {
                    newShip.Destination = ship.Destination;
                    newShip.LoadContainers(ship.ContainersOnboard);
                    yield return newShip;
                }
                else
                {
                    yield return ship;
                }
            }
        }

        public static ILookup<ShipDestination, Ship> JoinAndGroupShipsByDestination(
            this IEnumerable<Ship> ships,
            IEnumerable<Ship> moreShips
        )
        {
            return ships.Concat(moreShips).ToLookup(ship => ship.Destination);
        }

        public static IReadOnlyDictionary<
            ShipDestination,
            IEnumerable<Ship>
        > OrderShipsInEachDestination(this IEnumerable<IGrouping<ShipDestination, Ship>> ships)
        {
            var orderedGroupsOfShips = ships.Select(group => new
            {
                Destination = group.Key,
                OrderedShips = group.OrderShipsByPriority()
            });
            var asDictionary = orderedGroupsOfShips.ToDictionary(
                g => g.Destination,
                g => (IEnumerable<Ship>)g.OrderedShips
            );

            return asDictionary;
        }

        public static IOrderedEnumerable<Ship> OrderShipsByPriority(this IEnumerable<Ship> ships)
        {
            return ships
                .OrderByDescending(ship =>
                    ship.ContainersOnboard.Sum(container => container.Weight)
                )
                .ThenByDescending(ship => ship.ContainersOnboard.Count)
                .ThenBy(ship => ship.Id);
        }

        public static IEnumerable<
            KeyValuePair<ShipDestination, TDontCare>
        > OrderByDestination<TDontCare>(
            this IEnumerable<KeyValuePair<ShipDestination, TDontCare>> groupsOfShips
        )
        {
            return groupsOfShips.OrderBy(
                group => group.Key.ToString(),
                StringComparer.InvariantCultureIgnoreCase
            );
        }
    }

    public static class Extensions
    {
        // =========== Copy from Exercise 5B ====================
        public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            foreach (var item in source)
            {
                action(item);
            }
        }

        // =========== BONUS 2 ====================
        public static IEnumerable<IReadOnlyCollection<T>> Chunk<T>(
            this IEnumerable<T> source,
            int size
        )
        {
            var chunk = new List<T>(size);
            foreach (var item in source)
            {
                chunk.Add(item);
                if (chunk.Count == size)
                {
                    yield return chunk;
                    chunk = new List<T>(size);
                }
            }
            if (chunk.Count > 0)
            {
                yield return chunk;
            }
        }

        // =========== BONUS 3 ====================
        public static IEnumerable<IReadOnlyCollection<T>> ChunkByLimit<T>(
            this IEnumerable<T> source,
            int limit,
            int maxChunkSize,
            Func<T, int> selector
        )
        {
            return ChunkByLimit(source, (decimal)limit, maxChunkSize, x => (decimal)selector(x));
        }

        // =========== BONUS 4 ====================
        public static IEnumerable<IReadOnlyCollection<T>> ChunkByLimit<T>(
            this IEnumerable<T> source,
            long limit,
            int maxChunkSize,
            Func<T, long> selector
        )
        {
            return ChunkByLimit(source, (decimal)limit, maxChunkSize, x => (decimal)selector(x));
        }

        public static IEnumerable<IReadOnlyCollection<T>> ChunkByLimit<T>(
            this IEnumerable<T> source,
            decimal limit,
            int maxChunkSize,
            Func<T, decimal> selector
        )
        {
            decimal sum = 0;
            List<T> chunk = new List<T>(maxChunkSize);

            foreach (var item in source)
            {
                var nextValue = selector(item);
                var nextSum = sum + nextValue;
                if (nextSum > limit || chunk.Count == maxChunkSize)
                {
                    bool isEmpty = chunk.Count == 0;
                    if (isEmpty)
                    {
                        chunk.Add(item);
                    }
                    yield return chunk;

                    chunk = new List<T>(maxChunkSize);
                    if (!isEmpty)
                    {
                        chunk.Add(item);
                        sum = nextValue;
                    }
                    else
                    {
                        sum = 0;
                    }
                }
                else
                {
                    sum = nextSum;
                    chunk.Add(item);
                }
            }

            if (chunk.Count > 0)
            {
                yield return chunk;
            }
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
