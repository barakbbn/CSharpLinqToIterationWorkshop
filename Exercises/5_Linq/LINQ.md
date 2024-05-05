# LINQ Operations Summary

| Operation | Description | Syntax | Example |
|---|---|---|---|
| All | Determines if all elements meet a condition | `All(predicate)` | `allEven = numbers.All(n => n % 2 == 0);` |
| **Any** | Determines sequences contains any element| `Any()` | `isNotEmpty = numbers.Any();` |
|  | Determines if any element meets a condition | `Any(predicate)` | `hasPositive = numbers.Any(n => n > 0);` |
| **Average**| Average of numeric sequence | `Average()` | `average = numbers.Average();` |
|  | Average of values based on selector |`Average(selector)` | `averageLength = words.Average(word => word.Length);` |
| **Count** | Number of elements | `Count()` | `count = numbers.Count();` |
| | Number of elements meeting a condition | `Count(predicate)` | `positiveCount = numbers.Count(n => n > 0);` |
| **Distinct** | Unique elements | `Distinct()` | `uniqueNumbers = numbers.Distinct();` |
| Except | Elements in first sequence not in second | `Except(otherSequence)` | `difference = numbers1.Except(numbers2);` |
| **First** | First element | `First()` | `firstNumber = numbers.First();` |
| | First element meeting a condition | `First(predicate)` | `firstPositive = numbers.First(n => n > 0);` |
| FirstOrDefault | First or default | `FirstOrDefault()` | `firstOrDefault = numbers.FirstOrDefault();` |
| **GroupBy** | Groups by key | `GroupBy(keySelector)` | `wordsByLength = words.GroupBy(word => word.Length);` |
| Intersect | Elements in both sequences | `Intersect(otherSequence)` | `commonNumbers = numbers1.Intersect(numbers2);` |
| Last | Last element | `Last()` | `lastNumber = numbers.Last();` |
| | Last element meeting a condition | `Last(predicate)` | `lastOdd = numbers.Last(n => n % 2 == 1);` |
| LastOrDefault | Last or default | `LastOrDefault()` | `lastOrDefault = numbers.LastOrDefault();` |
| Max | Maximum element | `Max()` | `maxValue = numbers.Max();` |
|  | Maximum value based on selector | `Max(selector)`|`longestWord = words.Max(word => word.Length);` |
| Min | Minimum element | `Min()` | `minValue = numbers.Min();` |
| | Minimum value based on selector | `Min(selector)` | `smallestLength = words.Min(word => word.Length);` |
| **OrderBy** | Orders ascending | `OrderBy(keySelector)` | `sortedProducts = products.OrderBy(product => product.Price);` |
| **OrderByDescending** | Orders descending | `OrderByDescending(keySelector)` | `sortedNumbers = numbers.OrderByDescending(n => n);` |
| **Select** | Projects each element | `Select(selector)` | `lengths = words.Select(word => word.Length);` |
| **SelectMany** | Flattens sequences of sequences | `SelectMany(selector)` | `allChars = words.SelectMany(word => word.ToCharArray());` |
| Sum | Sum of numeric sequence | `Sum()` | `total = numbers.Sum();` |
| **ToArray** | Convert to array | `ToArray()` | `array = numbers.ToArray();` |
| **ToDictionary** | Convert to Dictionary | `ToDictionary(keySelector, valueSelector)` | `dictionary = numbers.ToDictionary(n => n, n => n * 2);` |
| **ToList** | Convert to List | `ToList()` | `list = numbers.ToList();` |
| Union | Combine sequences (remove duplicates) | `Union(otherSequence)` | `combinedNumbers = numbers1.Union(numbers2);` |
| **Where** | Filters based on condition | `Where(predicate)` | `evenNumbers = numbers.Where(n => n % 2 == 0);` |
