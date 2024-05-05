## &#x1F381; BONUS Exercise 1D: Remove consecutive duplicated values

**Difficulty:** Intermediate  
**Time:** 30 min  
___

In this exercise we'll practice implementation of `IEnumerable` and `IEnumerator`.  
The challenge is to write code that takes a given sequence with values (IEnumerable&lt;T&gt;)
and reduce any consecutive runs of same value into a single appearance of that value.  
> Also called Distinct until changed  

**Example**  

```csharp
var source = new[]{0, 1,   2, 2,   3, 3, 3,   2, 2,   1, 0};
var nonRepeating = new DistinctUntilChanged(source);
// Will Generate: 0, 1, 2, 3, 2, 1, 0
```

### Instructions &#x1F4DA;  
- Implement a class **DistinctUntilChanged&lt;T&gt;** and **DistinctUntilChangedEnumarator&lt;T&gt;**
  ```csharp
  public class DistinctUntilChanged<T> // TODO: Implement IEnumerable<T>
  {
      public DistinctUntilChanged(IEnumerable<T> source) {}
      // TODO: Implement GetEnumerator() 
  }
  public class DistinctUntilChangedEnumarator<T> // TODO: Implement IEnumerator<T>
  {
  } 
  ``` 

### Guidance &#x2B50;
- Use `_comparer` field to compare items for equality
- Should not use LINQ functionality
- Should not use `yield` keyword
 
### &#x1F381; Bonus
- Add optional parameter to constructor `IEqualityComparer comparer = null` to determine elements equality
  - Use it to perform case-insensitive string comparison: 
  ```csharp
  var source = new[] {"apple", "APPLE", "Berry", "berry", "Kiwi", "Apple", "ORANGE", "ORANGE"};
  var comparer = ???
  var nonRepeating = new DistinctUntilChanged(source, comparer);
  // Expected result:
  // apple, Berry, Kiwi, Apple, ORANGE
  ```