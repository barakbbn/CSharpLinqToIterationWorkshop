## Exercise 1A: Count Down to Zero

**Difficulty:** Beginner  
**Time:** 15 min  
___

### Exercise 1 - Countdown Sequence

In this exercise we'll practice implementation of `IEnumerable` and `GetEnumerator()`.  
The challenge is create a Countdown sequence that counts from provided number to Zero

**Example**  

```csharp
var countdown = new CountdownSequence(3);
foreach(var value in countdown)
{
    Console.Write(value);
}
// 3 2 1 0

new CountdownSequence(0);  // -> [0]
new CountdownSequence(-1); // -> []  empty
```

### Instructions &#x1F4DA;
- Implement a class **CountdownSequence**
  - It should implements IEnumerable&lt;int&gt;
  - Constructor signature is: (int count)  
    - count parameter - The initial value to start counting down from, till zero  
  - GetEnumerator() method - Should return IEnumerator<int> with values from **count** till Zero  
- If the **count** parameter is **zero**, the countdown should only include zero.
- Negative **count** parameter is valid! if that case, nothing should be counted down (i.e. EMPTY sequence)  

```charp
public class CountdownSequence // TODO: Implement IEnumerable<int>
{
    public CountdownSequence(int count) {}
    // TODO: Implement GetEnumerator()
}
```

### Guidance &#x2B50;
- Should **not** use LINQ functionality
- Should **not** use `yield` keyword


### Exercise 2 - Debug Enumerable

n this exercise we'll practice implementation of `IEnumerable` and `IEnumerator`.  
Implements class `DebugEnumerable` perform `Console.WriteLine()` for each method/property implemented by `IEnumerable` and `IEnumerator`.  
It should accept a source IEnumerable as input, to iterate on.

### Instructions &#x1F4DA;
- Implement a class **DebugEnumerable**
  - It should implements IEnumerable&lt;T&gt;
  - Constructor signature is: (IEnumerable&lt;T&gt; source)  

```charp
public class DebugEnumerable // TODO: Implement IEnumerable<T>
{
    public DebugEnumerable(IEnumerable<T> source) {}

    // TODO: DebugEnumerator class
    private class DebugEnumerator // TODO: Implement IEnumerator<T>
    {
    }
}
```

### Guidance &#x2B50;
- Should **not** use LINQ functionality
- Should **not** use `yield` keyword
