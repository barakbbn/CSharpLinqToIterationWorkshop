## Exercise 5B - LINQ Extension methods

In this exercise we'll practice implementation of various extension methods on IEnumerable.  
You can use existing LINQ functionalities and other elements you done and learnt in previous exercises.  

### Instructions:  
0. create class for holding the extensions methods
   ```csharp
   public static class Extensions {}
   ```
   ___  
   **Difficulty:** Intermediate  
   **Time:** 15 min  

1. **ForEach()** - Performs an action on each item in an IEnumerable sequence.  
   Similar to C# built-in `foreach` keyword
   ```csharp
   public static void ForEach<T>(this IEnumerable<T> source, Action<T> action);
   public static void ForEach<T>(this IEnumerable<T> source, Action<T, int> action);
   //                                                              /     \
   //                                                Item from source,   Index of the item
   ```
   Example  
   ```csharp
   var months = System.Globalization.DateTimeFormatInfo.CurrentInfo.MonthNames;
   months.ForEach((month, i) => Console.WriteLine($"{month} ({i+1})"));
   // January (1)
   // February (2)
   // March (3)
   // ...
   ```
   ___

   **Difficulty:** Intermediate/Experienced  
   **Time:** 20 min  
1. **ToString()** - Converts the sequence into a string with the items delimited by a separator.
   ```csharp
   public static string ToString<T>(this IEnumerable<T> source, string separator)
   ```
   Example  
   ```csharp
   var fencedNumbers = Enumerable.Range(0, 10).ToString("|");
   Console.WriteLine(fencedNumbers);
   // "0|1|2|3|4|5|6|7|8|9"
   ```

   > &#x1F4A1; HINT: there is built-in functionality in .NET that converts a collection into a delimited string
___
   **Difficulty:** Experienced  
   **Time:** 30 min 
1. **HasAtLeast()** - Determines if a sequence has at least the number of specified items, in an optimized manner.
   ```csharp
   public static bool HasAtLeast<T>(this IEnumerable<T> source, int count)
   ```
   Example  
   ```csharp
   var numbers = Enumerable.Range(0, 10);
   bool hasAtLeast = numbers.HasAtList(5);
   ```
   ___
   **Difficulty:** Experienced  
   **Time:** 20 min 
1. **Flatten()** - Flat/unwrap IEnumerable of IEnumerable&lt;T&gt; into a single IEnumerable&lt;T&gt;.
   ```csharp
   public static IEnumerable<T> Flatten<T>(this IEnumerable<IEnumerable<T>> source)
   ```
   Example  
   ```csharp
   var strings = new[]{ "123", "ABC", "!@#" };
   var characters = strings.Flatten();
   // {'1','2','3','A','B','C','!','@','#'}
  
   IEnumerable<Task<string[]>> tasks;
   tasks = Enumerable.Range(0, 5).Select(page => Task.Run(() => LoadResults(page)));
   string[][] collectionOfResults = await Task.WhenAll(tasks);
   string[] allResults = collectionOfResults.Flatten().ToArray();
   ```
   > &#x1F4A1; HINT: `foreach` inside `foreach` OR `SelectMany`  
    ___
   **Difficulty:** Advanced  
   **Time:** 45 min  
1. &#x1F381; Bonus: **RemoveByKeys()** - Remove/Skip/Filter-out items that have the same key in the list of keys-to-remove.
   ```csharp
   public static IEnumerable<T> RemoveByKeys<T, K>(this IEnumerable<T> source, IEnumerable<K> keys, Func<T, K> keySelector, IEqualityComparer<K> comparer = null)
   ```
   Example  
   ```csharp
   IEnumerable<StudentInfo> students = new StudentsFileEnumerator("Students\\Semester1.csv");
   var studentsToRemove = new[]{"Ebony Santana", "Beau Stokes", "Colby Page"};
   var newStudentsList = students.RemoveByKey(studentsToRemove, student => student.Name);
   ```
   ___
   **Difficulty:** Advanced  
   **Time:** 45 min 
1. &#x1F381; Bonus: **Interleave()** - Combines two sequences by alternating items from each sequence.
   ```csharp
   public static IEnumerable<T> Interleave<T>(this IEnumerable<T> first, IEnumerable<T> second)
   ```
   Example  
   ```csharp
   void FairQueue<T>(Queue<T> q1, Queue<T> q2)
   {
     var fairQueue = q1.Interleave(q2); 
   }

   var ilCars = new[]{new Car("111-11-111"), new Car("111-22-333"), new Car("101-01-010") };
   var usCars = new[]{new Car("123-yo-yo"), new Car("mr-b1g-b33") };
   var fairQueue = ilCars.Interleave(usCars);
   // {"111-11-111", "123-yo-yo", "111-22-333", "mr-b1g-b33", "101-01-010" }
   ```
