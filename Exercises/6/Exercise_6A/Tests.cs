using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using TestsUtils;

namespace Exercise_6A
{
    namespace ExtensionsTests
    {
        [TestFixture]
        public class ChunkTests : CommonDeferredOnInputEnumerableTests<DateTime>
        {
            public void Chunk_ReturnsChunksOfExpectedSize()
            {
                var input = CreateDates(10);

                var size = 3;
                var expected = new DateTime[][]
                {
                    new[] { input[0], input[1], input[2] },
                    new[] { input[3], input[4], input[5] },
                    new[] { input[6], input[7], input[8] },
                    new[] { input[9] },
                };

                var sut = Extensions.Chunk(input, size);
                var actual = sut.ToArray();

                Assert.That(actual, Is.EqualTo(expected));
            }

            [Test]
            public void SizeIsGreaterThanInput_ReturnSingleChunk()
            {
                var input = CreateDates(10);
                var size = input.Count + 1;
                var expected = new[] { input };
                var sut = Extensions.Chunk(input, size);
                var actual = sut.ToArray();

                Assert.That(actual, Is.EqualTo(expected));
            }

            [Test]
            public void SizeIsNotPositive_Throws()
            {
                try
                {
                    Extensions.Chunk(new DateTime[0], 0);
                    Warn.If(true, $"Expected {nameof(ArgumentOutOfRangeException)}");
                }
                catch (Exception ex)
                {
                    Warn.Unless(
                        ex is ArgumentOutOfRangeException,
                        $"Expected {nameof(ArgumentOutOfRangeException)} but got:\n  {ex}"
                    );
                }
            }

            public void SizeIsOne_ReturnsChunksForEachInputItem()
            {
                var input = CreateDates(10);
                const int SIZE_OF_ONE = 1;
                var expected = input.Select(item => new[] { item }).ToArray();

                var sut = Extensions.Chunk(input, SIZE_OF_ONE);
                var actual = sut.ToArray();

                Assert.That(actual, Is.EqualTo(expected));
            }

            [Test]
            public void EmptyInput_ReturnsZeroChunks()
            {
                var emptyInput = new DateTime[0];
                var size = 1;
                var sut = Extensions.Chunk(emptyInput, size);
                Assert.IsEmpty(sut);
            }

            [Test]
            public void IterateFirstChunk_OtherChunksNotCreated()
            {
                var input = TestableEnumerable.From(CreateDates(10));
                var size = 3;
                var sut = Extensions.Chunk(input, size);
                foreach (var chunk in sut)
                {
                    chunk.ToArray();
                    Warn.If(
                        input.MoveNextCount > size,
                        $"Input was iterated more than {size} times, while only the first chunk was iterated"
                    );
                    break;
                }
            }

            protected override IEnumerable<DateTime> CreateSimpleInputSequence()
            {
                return CreateDates(11);
            }

            protected override object SutDeferredAction(IEnumerable<DateTime> input)
            {
                return Extensions.Chunk(input, 3);
            }

            private List<DateTime> CreateDates(int count, string from = "2000-01-01")
            {
                var baseDate = DateTime.Parse(from);
                return Enumerable.Range(0, count).Select(i => baseDate.AddMonths(i)).ToList();
            }
        }

        [TestFixture]
        public class ChunkByLimitTests : CommonDeferredOnInputEnumerableTests<DateTime>
        {
            [Test]
            public void Chunk_LimitNeverReached_ReturnsChunksBySize()
            {
                var input = CreateDates(10);

                var size = 3;
                var limit = int.MaxValue;
                var expected = new DateTime[][]
                {
                    new[] { input[0], input[1], input[2] },
                    new[] { input[3], input[4], input[5] },
                    new[] { input[6], input[7], input[8] },
                    new[] { input[9] },
                };

                var sut = Extensions.ChunkByLimit(input, limit, size, dt => dt.Month);
                var actual = sut.ToArray();

                Assert.That(actual, Is.EqualTo(expected));
            }

            [Test]
            public void EmptyInput_ReturnsZeroChunks()
            {
                var emptyInput = new DateTime[0];
                var size = 1;
                var sut = Extensions.ChunkByLimit(emptyInput, 1, 1, _ => _.Hour);
                Assert.IsEmpty(sut);
            }

            [Test]
            public void SizeIsGreaterThanInput_ReturnsChunksWithItemsNotExceedingTheLimit()
            {
                var input = CreateDates(10);

                var size = input.Count + 1;
                var limit = 13;
                var expected = new DateTime[][]
                {
                    new[]
                    { // sum-of-months = 10
                        /* 1 */input[0],
                        /* 2 */input[1],
                        /* 3 */input[2],
                        /* 4 */input[3]
                    },
                    new[]
                    { // sum-of-months = 11
                        /* 5 */input[4],
                        /* 6 */input[5]
                    },
                    new[]
                    { // sum-of-months = 7
                        /* 7 */input[6]
                    },
                    new[]
                    { // sum-of-months = 8
                        /* 8 */input[7]
                    },
                    new[]
                    { // sum-of-months = 9
                        /* 9 */input[8]
                    },
                    new[]
                    { // sum-of-months = 10
                        /* 10 */input[9]
                    }
                };

                var sut = Extensions.ChunkByLimit(input, limit, size, dt => dt.Month);
                var actual = sut.ToArray();

                Assert.That(actual, Is.EqualTo(expected));
            }

            [Test]
            public void Chunk_WithLimitAndSize_ReturnsChunksWithItemsNotBreakingTheLimitAndNorMoreThanSpecifiedSize()
            {
                var input = CreateDates(10);

                var size = 3;
                var limit = 13;
                var expected = new DateTime[][]
                {
                    new[]
                    { // sum-of-months = 6 (max size of 3)
                        /* 1 */input[0],
                        /* 2 */input[1],
                        /* 3 */input[2]
                    },
                    new[]
                    { // sum-of-months = 9
                        /* 4 */input[3],
                        /* 5 */input[4]
                    },
                    new[]
                    { // sum-of-months = 13
                        /* 6 */input[5],
                        /* 7 */input[6]
                    },
                    new[]
                    { // sum-of-months = 8
                        /* 8 */input[7]
                    },
                    new[]
                    { // sum-of-months = 9
                        /* 9 */input[8]
                    },
                    new[]
                    { // sum-of-months = 10
                        /* 10 */input[9]
                    }
                };

                var sut = Extensions.ChunkByLimit(input, limit, size, dt => dt.Month);
                var actual = sut.ToArray();

                Assert.That(actual, Is.EqualTo(expected));
            }

            [Test]
            public void IterateFirstChunk_OtherChunksNotCreated()
            {
                var input = TestableEnumerable.From(CreateDates(10));
                var size = 3;
                var limit = 10;
                var sut = Extensions.ChunkByLimit(input, limit, size, dt => dt.Month);
                foreach (var chunk in sut)
                {
                    chunk.ToArray();
                    Warn.If(
                        input.MoveNextCount > size + 1,
                        $"Input was iterated more than {size} times, while only the first chunk was iterated"
                    );
                    break;
                }
            }

            [Test]
            public void SizeIsNotPositive_Throws()
            {
                try
                {
                    Extensions.ChunkByLimit(new DateTime[0], 1, 0, dt => dt.Month);
                    Warn.If(true, $"Expected {nameof(ArgumentOutOfRangeException)}");
                }
                catch (Exception ex)
                {
                    Warn.Unless(
                        ex is ArgumentOutOfRangeException,
                        $"Expected {nameof(ArgumentOutOfRangeException)} but got:\n  {ex}"
                    );
                }
            }

            [Test]
            public void SizeIsOne_ReturnsChunksForEachInputItem()
            {
                var input = CreateDates(10);
                const int SIZE_OF_ONE = 1;
                var limit = 10;

                var expected = input.Select(item => new[] { item }).ToArray();

                var sut = Extensions.ChunkByLimit(input, limit, SIZE_OF_ONE, dt => dt.Month);
                var actual = sut.ToArray();

                Assert.That(actual, Is.EqualTo(expected));
            }

            [Test]
            public void AllItemsExceedsLimit_ReturnsChunksForEachInputItem()
            {
                var input = CreateDates(10);
                int size = input.Count + 1;
                var limit = 0;

                var expected = input.Select(item => new[] { item }).ToArray();

                var sut = Extensions.ChunkByLimit(input, limit, size, dt => dt.Month);
                var actual = sut.ToArray();

                Assert.That(actual, Is.EqualTo(expected));
            }

            protected override IEnumerable<DateTime> CreateSimpleInputSequence()
            {
                return CreateDates(11);
            }

            protected override object SutDeferredAction(IEnumerable<DateTime> input)
            {
                return Extensions.Chunk(input, 3);
            }

            private List<DateTime> CreateDates(int count, string from = "2000-01-01")
            {
                var baseDate = DateTime.Parse(from);
                return Enumerable.Range(0, count).Select(i => baseDate.AddMonths(i)).ToList();
            }
        }
    }

    namespace ExerciseTests
    {
        [TestFixture]
        public class ChunkContainersTests : CommonDeferredOnInputEnumerableTests<ZimContainer>
        {
            [Test]
            public void Chunk_ReturnsChunksOfExpectedSize()
            {
                var input = CreateTestContainers(10);
                var size = 3;
                var expected = new ZimContainer[][]
                {
                    new[] { input[0], input[1], input[2] },
                    new[] { input[3], input[4], input[5] },
                    new[] { input[6], input[7], input[8] },
                    new[] { input[9] },
                };

                var sut = Program.ChunkContainers(input, size);
                var actual = sut.ToArray();

                Assert.That(actual, Is.EqualTo(expected));
            }

            [Test]
            public void SizeIsGreaterThanInput_ReturnSingleChunk()
            {
                var input = CreateTestContainers(10);
                var size = input.Count + 1;
                var expected = new[] { input };
                var sut = Program.ChunkContainers(input, size);
                var actual = sut.ToArray();

                Assert.That(actual, Is.EqualTo(expected));
            }

            [Test]
            public void SizeIsNotPositive_Throws()
            {
                try
                {
                    Program.ChunkContainers(new ZimContainer[0], 0);
                    Warn.If(true, $"Expected {nameof(ArgumentOutOfRangeException)}");
                }
                catch (Exception ex)
                {
                    Warn.Unless(
                        ex is ArgumentOutOfRangeException,
                        $"Expected {nameof(ArgumentOutOfRangeException)} but got:\n  {ex}"
                    );
                }
            }

            public void SizeIsOne_ReturnsChunksForEachInputItem()
            {
                var input = CreateTestContainers(10);
                const int SIZE_OF_ONE = 1;
                var expected = input.Select(item => new[] { item }).ToArray();

                var sut = Program.ChunkContainers(input, SIZE_OF_ONE);
                var actual = sut.ToArray();

                Assert.That(actual, Is.EqualTo(expected));
            }

            [Test]
            public void EmptyInput_ReturnsZeroChunks()
            {
                var emptyInput = new ZimContainer[0];
                var size = 1;
                var sut = Program.ChunkContainers(emptyInput, size);
                Assert.IsEmpty(sut);
            }

            [Test]
            public void IterateFirstChunk_OtherChunksNotCreated()
            {
                var input = TestableEnumerable.From(CreateTestContainers(10));
                var size = 3;
                var sut = Program.ChunkContainers(input, size);
                foreach (var chunk in sut)
                {
                    chunk.ToArray();
                    Warn.If(
                        input.MoveNextCount > size,
                        $"Input was iterated more than {size} times, while only the first chunk was iterated"
                    );
                    break;
                }
            }

            protected override IEnumerable<ZimContainer> CreateSimpleInputSequence()
            {
                return CreateTestContainers(10);
            }

            protected override object SutDeferredAction(IEnumerable<ZimContainer> containers)
            {
                return Program.ChunkContainers(containers, 2);
            }

            private List<ZimContainer> CreateTestContainers(int count)
            {
                return TestHelper.CreateTestContainers(count);
            }
        }

        [TestFixture]
        public class ChunkByWeightTests : CommonDeferredOnInputEnumerableTests<ZimContainer>
        {
            [Test]
            public void Chunk_LimitNeverReached_ReturnsChunksBySize()
            {
                var input = CreateTestContainers(10);

                var size = 3;
                var limit = int.MaxValue;
                var expected = new ZimContainer[][]
                {
                    new[] { input[0], input[1], input[2] },
                    new[] { input[3], input[4], input[5] },
                    new[] { input[6], input[7], input[8] },
                    new[] { input[9] },
                };

                var sut = Program.ChunkByWeight(input, limit, size);
                var actual = sut.ToArray();

                Assert.That(actual, Is.EqualTo(expected));
            }

            [Test]
            public void EmptyInput_ReturnsZeroChunks()
            {
                var emptyInput = new ZimContainer[0];
                var sut = Program.ChunkByWeight(emptyInput, 1, 1);
                Assert.IsEmpty(sut);
            }

            [Test]
            public void SizeIsGreaterThanInput_ReturnsChunksWithItemsNotExceedingTheLimit()
            {
                var input = CreateTestContainers(10);
                input.ForEach(c => c.Weight = 10);

                var size = input.Count + 1;
                var limit = 35;
                var expected = new ZimContainer[][]
                {
                    new[] { input[0], input[1], input[2], },
                    new[] { input[3], input[4], input[5] },
                    new[] { input[6], input[7], input[8] },
                    new[] { input[9] }
                };

                var sut = Program.ChunkByWeight(input, limit, size);
                var actual = sut.ToArray();

                Assert.That(actual, Is.EqualTo(expected));
            }

            [Test]
            public void Chunk_WithLimitAndSize_ReturnsChunksWithItemsNotBreakingTheLimitAndNorMoreThanSpecifiedSize()
            {
                var input = CreateTestContainers(10);

                var size = 3;
                var limit = 130;
                var expected = new ZimContainer[][]
                {
                    new[]
                    { // sum-of-months = 60 (max size of 3)
                        /* 10 */input[0],
                        /* 20 */input[1],
                        /* 30 */input[2]
                    },
                    new[]
                    { // sum-of-months = 90
                        /* 40 */input[3],
                        /* 50 */input[4]
                    },
                    new[]
                    { // sum-of-months = 130
                        /* 60 */input[5],
                        /* 70 */input[6]
                    },
                    new[]
                    { // sum-of-months = 80
                        /* 80 */input[7]
                    },
                    new[]
                    { // sum-of-months = 90
                        /* 90 */input[8]
                    },
                    new[]
                    { // sum-of-months = 100
                        /* 100 */input[9]
                    }
                };

                var sut = Program.ChunkByWeight(input, limit, size);
                var actual = sut.ToArray();

                Assert.That(actual, Is.EqualTo(expected));
            }

            [Test]
            public void IterateFirstChunk_OtherChunksNotCreated()
            {
                var input = TestableEnumerable.From(CreateTestContainers(10));
                var size = 3;
                var limit = 100;
                var sut = Program.ChunkByWeight(input, limit, size);
                foreach (var chunk in sut)
                {
                    chunk.ToArray();
                    Warn.If(
                        input.MoveNextCount > size + 1,
                        $"Input was iterated more than {size} times, while only the first chunk was iterated"
                    );
                    break;
                }
            }

            [Test]
            public void SizeIsNotPositive_Throws()
            {
                try
                {
                    Program.ChunkByWeight(new ZimContainer[0], 1, 0);
                    Warn.If(true, $"Expected {nameof(ArgumentOutOfRangeException)}");
                }
                catch (Exception ex)
                {
                    Warn.Unless(
                        ex is ArgumentOutOfRangeException,
                        $"Expected {nameof(ArgumentOutOfRangeException)} but got:\n  {ex}"
                    );
                }
            }

            [Test]
            public void SizeIsOne_ReturnsChunksForEachInputItem()
            {
                var input = CreateTestContainers(10);
                const int SIZE_OF_ONE = 1;
                var limit = 10;

                var expected = input.Select(item => new[] { item }).ToArray();

                var sut = Program.ChunkByWeight(input, limit, SIZE_OF_ONE);
                var actual = sut.ToArray();

                Assert.That(actual, Is.EqualTo(expected));
            }

            [Test]
            public void AllItemsExceedsLimit_ReturnsChunksForEachInputItem()
            {
                var input = CreateTestContainers(10);
                int size = input.Count + 1;
                var limit = 0;

                var expected = input.Select(item => new[] { item }).ToArray();

                var sut = Program.ChunkByWeight(input, limit, size);
                var actual = sut.ToArray();

                Assert.That(actual, Is.EqualTo(expected));
            }

            protected override IEnumerable<ZimContainer> CreateSimpleInputSequence()
            {
                return CreateTestContainers(10);
            }

            protected override object SutDeferredAction(IEnumerable<ZimContainer> containers)
            {
                return Program.ChunkByWeight(containers, 13, 4);
            }

            private List<ZimContainer> CreateTestContainers(int count)
            {
                return TestHelper.CreateTestContainers(count);
            }
        }

        [TestFixture]
        public class LoadContainerOntoClassAShipsTests
            : CommonDeferredOnInputEnumerableTests<ZimContainer>
        {
            [Test]
            public void LoadContainers_ReturnsExpectedShipsWithExpectedContainers()
            {
                var input = CreateTestContainers(20);

                // Prepare expected
                var destinations = Enum.GetValues(typeof(ShipDestination))
                    .OfType<ShipDestination>()
                    .ToArray();
                var shipsByDestination = destinations.ToDictionary(
                    _ => _,
                    _ => new List<ShipClassA>()
                );

                foreach (
                    var container in input.Where(c => c.ShipTo != ShipDestination.SECRET_LOCATION)
                )
                {
                    var ships = shipsByDestination[container.ShipTo];

                    var isNewShip =
                        ships.Count == 0
                        || ships[ships.Count - 1].ContainersOnboard.Count
                            == ShipClassA.MAX_CONTAINERS;

                    var ship = isNewShip
                        ? new ShipClassA { Destination = container.ShipTo }
                        : ships[ships.Count - 1];

                    ship.LoadContainers(new[] { container });
                    if (isNewShip)
                    {
                        ships.Add(ship);
                    }
                }

                var expected = shipsByDestination.Values.SelectMany(_ => _).ToList();

                var sut = Program.LoadContainerOntoClassAShips(input);
                var actual = sut.ToArray();

                Assert.That(
                    actual,
                    Is.EquivalentTo(expected)
                        .Using(new TestHelper.ShipWithContainersEqualityComparer())
                );
            }

            protected override IEnumerable<ZimContainer> CreateSimpleInputSequence()
            {
                return CreateTestContainers(10);
            }

            protected override object SutDeferredAction(IEnumerable<ZimContainer> input)
            {
                return Program.LoadContainerOntoClassAShips(input);
            }

            private List<ZimContainer> CreateTestContainers(int count)
            {
                return TestHelper.CreateTestContainers(count);
            }
        }

        [TestFixture]
        public class LoadContainerOntoClassBShipsTests
            : CommonDeferredOnInputEnumerableTests<ZimContainer>
        {
            [Test]
            public void LoadContainers_ReturnsExpectedShipsWithExpectedContainersAndWeight()
            {
                var input = CreateTestContainers(20);

                // Prepare expected
                var destinations = Enum.GetValues(typeof(ShipDestination))
                    .OfType<ShipDestination>()
                    .ToArray();
                var shipsByDestination = destinations.ToDictionary(
                    _ => _,
                    _ => new List<ShipClassB>()
                );

                foreach (var container in input.Where(c => c.ShipTo != ShipDestination.NotSet))
                {
                    var ships = shipsByDestination[container.ShipTo];
                    var newShip = new ShipClassB(container.ShipTo);
                    var lastShip = ships.Count == 0 ? null : ships[ships.Count - 1];
                    var needNewShip =
                        lastShip == null
                        || lastShip.ContainersOnboard.Count == ShipClassB.MAX_CONTAINERS
                        || (lastShip.ContainersOnboard.Sum(c => c.Weight) + container.Weight)
                            > ShipClassB.MAX_WEIGHT;

                    var ship = needNewShip ? newShip : lastShip;
                    ship.LoadContainers(new[] { container });
                    if (needNewShip)
                    {
                        ships.Add(newShip);
                    }
                }

                var expected = shipsByDestination.Values.SelectMany(_ => _).ToList();

                var sut = Program.LoadContainerOntoClassBShips(input);
                var actual = sut.ToArray();

                Assert.That(
                    actual,
                    Is.EquivalentTo(expected)
                        .Using(new TestHelper.ShipWithContainersEqualityComparer())
                );
            }

            protected override IEnumerable<ZimContainer> CreateSimpleInputSequence()
            {
                return CreateTestContainers(10);
            }

            protected override object SutDeferredAction(IEnumerable<ZimContainer> input)
            {
                return Program.LoadContainerOntoClassBShips(input);
            }

            private List<ZimContainer> CreateTestContainers(int count)
            {
                return TestHelper.CreateTestContainers(count);
            }
        }

        [TestFixture]
        public class ReplaceShipTests : CommonDeferredOnInputEnumerableTests<Ship>
        {
            [Test]
            public void ShipNotExists_ReturnsOriginalShips()
            {
                var input = CreateTestShips(5, 5);
                var nonExistingShipId = -999;

                var sut = Program.ReplaceShip(input, nonExistingShipId, new ShipClassA());
                var actual = sut.ToArray();

                Assert.That(actual, Is.EqualTo(input));
            }

            [Test]
            public void EmptyInput_ReturnsEmptySequence()
            {
                var sut = Program.ReplaceShip(new Ship[0], 1, new ShipClassA());
                var actual = sut.ToArray();

                Assert.IsEmpty(actual);
            }

            [Test]
            public void ShipExists_ReturnsNewShipInsteadOfReplaced()
            {
                var input = CreateTestShips(5, 5);
                var newShip = new ShipClassA();

                var expected = new List<Ship>(input);
                var shipIndex = 3;
                var oldShip = input[shipIndex];
                expected[shipIndex] = newShip;

                var sut = Program.ReplaceShip(input, oldShip.Id, newShip);
                var actual = sut.ToArray();

                Assert.That(actual, Is.EqualTo(expected));
            }

            [Test]
            public void ReturnsNewShipSetToSameDestinationAsTheReplacedShip()
            {
                var input = CreateTestShips(5, 5);
                var newShip = new ShipClassA();

                var expected = new List<Ship>(input);
                var shipIndex = 3;
                var oldShip = input[shipIndex];
                expected[shipIndex] = newShip;

                var sut = Program.ReplaceShip(input, oldShip.Id, newShip);
                var actual = sut.ToArray();

                Assert.AreEqual(oldShip.Destination, newShip.Destination);
            }

            [Test]
            public void ReturnsNewShipWithTheContainersOfTheReplacedShip()
            {
                var input = CreateTestShips(5, 5);
                var newShip = new ShipClassA();

                var expected = new List<Ship>(input);
                var shipIndex = 3;
                var oldShip = input[shipIndex];
                expected[shipIndex] = newShip;

                var sut = Program.ReplaceShip(input, oldShip.Id, newShip);
                var actual = sut.ToArray();

                Assert.That(
                    newShip.ContainersOnboard,
                    Is.EqualTo(oldShip.ContainersOnboard)
                        .Using(new TestHelper.ZimContainerEqualityComparer())
                );
            }

            protected override object SutDeferredAction(IEnumerable<Ship> input)
            {
                return Program.ReplaceShip(input, -999, new ShipClassA());
            }

            protected override IEnumerable<Ship> CreateSimpleInputSequence()
            {
                return CreateTestShips(classA: 10, classB: 10);
            }

            private List<Ship> CreateTestShips(int classA, int classB)
            {
                return TestHelper.CreateTestShips(classA, classB);
            }
        }

        [TestFixture]
        public class JoinAndGroupShipsByDestinationTests
        {
            [Test]
            public void ReturnAllShipsGroupedByDestination()
            {
                var classA = CreateTestShips(5, 0);
                var classB = CreateTestShips(0, 5);
                var expected = TestHelper
                    .ValidDestinations()
                    .ToDictionary(_ => _, _ => new List<Ship>());
                classA.ForEach(ship => expected[ship.Destination].Add(ship));
                classB.ForEach(ship => expected[ship.Destination].Add(ship));

                var sut = Program.JoinAndGroupShipsByDestination(classA, classB);

                foreach (var kvp in expected)
                {
                    var destination = kvp.Key;
                    var expectedShips = kvp.Value;
                    var actual = sut[destination];
                    Assert.That(
                        actual,
                        Is.EquivalentTo(expectedShips)
                            .Using(new TestHelper.ShipWithContainersEqualityComparer())
                    );
                }
            }

            private List<Ship> CreateTestShips(int classA, int classB)
            {
                return TestHelper.CreateTestShips(classA, classB);
            }
        }

        [TestFixture]
        public class OrderShipsInEachDestination
        {
            [Test]
            public void ReturnsShipsOrderedAsExpected()
            {
                var input = CreateTestShips(10, 5).GroupBy(ship => ship.Destination);
                var expected = input.ToDictionary(group => group.Key, group => group.ToList());
                foreach (var kvp in expected)
                {
                    kvp.Value.Sort(
                        (x, y) =>
                        {
                            var weightX = x.ContainersOnboard.Sum(c => c.Weight);
                            var weightY = y.ContainersOnboard.Sum(c => c.Weight);
                            if (weightX != weightY)
                            {
                                return weightY - weightX;
                            }
                            if (x.ContainersOnboard.Count != y.ContainersOnboard.Count)
                            {
                                return y.ContainersOnboard.Count - x.ContainersOnboard.Count;
                            }
                            return x.Id - y.Id;
                        }
                    );
                }

                var sut = Program.OrderShipsInEachDestination(input);

                foreach (var kvp in expected)
                {
                    var destination = kvp.Key;
                    var expectedShips = kvp.Value;
                    var actual = sut[destination];
                    Assert.That(
                        actual,
                        Is.EquivalentTo(expectedShips)
                            .Using(new TestHelper.ShipWithContainersEqualityComparer())
                    );
                }
            }

            private List<Ship> CreateTestShips(int classA, int classB)
            {
                return TestHelper.CreateTestShips(classA, classB);
            }
        }

        [TestFixture]
        public class OrderByDestinationTests
        {
            [Test]
            public void Order_ReturnKeysOrderedByName()
            {
                var input = CreateTestShips(10, 5)
                    .GroupBy(ship => ship.Destination)
                    .ToDictionary(group => group.Key);
                var expected = input.ToList();
                expected.Sort(
                    (x, y) =>
                        string.Compare(
                            x.Key.ToString(),
                            y.Key.ToString(),
                            StringComparison.InvariantCultureIgnoreCase
                        )
                );
                var sut = Program.OrderByDestination(input);

                Assert.That(sut, Is.EqualTo(expected));
            }

            private List<Ship> CreateTestShips(int classA, int classB)
            {
                return TestHelper.CreateTestShips(classA, classB);
            }
        }

        static class TestHelper
        {
            public static List<ZimContainer> CreateTestContainers(
                int count,
                ShipDestination? destination = null
            )
            {
                var containers = new List<ZimContainer>();

                var id = 1;
                var destinations = ValidDestinations();
                for (int i = 0; i < count; i++)
                {
                    var dest = destination ?? destinations[i % destinations.Length];
                    var weight = i * 10 + 10;
                    var container = new ZimContainer(id++) { Weight = weight, ShipTo = dest };
                    containers.Add(container);
                }
                return containers;
            }

            internal static List<Ship> CreateTestShips(int classA, int classB)
            {
                var destinations = ValidDestinations();
                var classADestinations = destinations
                    .Where(d => d != ShipDestination.SECRET_LOCATION)
                    .ToList();

                var classAShips = Enumerable
                    .Range(0, classA)
                    .Select(i =>
                    {
                        var destination = classADestinations[i % classADestinations.Count];
                        var containers = CreateTestContainers(i, destination);
                        return new ShipClassA(destination);
                    });
                var classBShips = Enumerable
                    .Range(0, classB)
                    .Select(i =>
                    {
                        var destination = destinations[i % destinations.Length];
                        var containers = CreateTestContainers(i, destination);
                        return new ShipClassB(destination);
                    });

                return classAShips.Concat<Ship>(classBShips).ToList();
            }

            internal class ZimContainerEqualityComparer : IEqualityComparer<ZimContainer>
            {
                public bool Equals(ZimContainer x, ZimContainer y)
                {
                    if (ReferenceEquals(x, y))
                        return true;
                    if (ReferenceEquals(x, null))
                        return false;
                    if (ReferenceEquals(y, null))
                        return false;

                    if (x.ShipTo != y.ShipTo)
                        return false;
                    if (x.Weight != y.Weight)
                        return false;

                    return true;
                }

                public int GetHashCode(ZimContainer obj)
                {
                    return (obj.ShipTo, obj.Weight).GetHashCode();
                }
            }

            internal class ShipWithContainersEqualityComparer : IEqualityComparer<Ship>
            {
                private IEqualityComparer<ZimContainer> _zimContainerComparer =
                    new ZimContainerEqualityComparer();

                public bool Equals(Ship x, Ship y)
                {
                    if (ReferenceEquals(x, y))
                        return true;
                    if (ReferenceEquals(x, null))
                        return false;
                    if (ReferenceEquals(y, null))
                        return false;
                    if (x.GetType() != y.GetType())
                        return false;

                    if (x.Destination != y.Destination)
                        return false;
                    if (x.ContainersOnboard.Count != y.ContainersOnboard.Count)
                        return false;

                    return Enumerable.SequenceEqual(
                        x.ContainersOnboard.OrderBy(c => c.Weight),
                        y.ContainersOnboard.OrderBy(c => c.Weight),
                        _zimContainerComparer
                    );
                }

                public int GetHashCode(Ship obj)
                {
                    return obj.ContainersOnboard.Aggregate(
                        0,
                        (hash, item) => hash ^ _zimContainerComparer.GetHashCode(item)
                    );
                }
            }

            public static ShipDestination[] ValidDestinations()
            {
                return Enum.GetValues(typeof(ShipDestination))
                    .OfType<ShipDestination>()
                    .Where(d => d != ShipDestination.NotSet)
                    .ToArray();
            }
        }
    }
}
