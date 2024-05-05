## Exercise 2A - Repeat a sequence Infinitely

**Difficulty:** Intermediate  
**Time:** 20 min  
___

In this exercise we'll practice implementation of IEnumerable and IEnumerator.  
The challenge is to write code that takes a given sequence with values (IEnumerable&lt;T&gt;)
and repeat it infinitely

**Example**  
```csharp
var months = System.Globalization.DateTimeFormatInfo.CurrentInfo.MonthNames;
var repeatedMonths = new EndlessCycle(months);
// Will generate the list of months over and over infinitely
```
### Instructions &#x1F4DA;
1. Implement a class **EndlessCycle&lt;T&gt;**
   - It should implements IEnumerable&lt;T&gt;
   - Constructor should accept a source IEnumerable&lt;T&gt; to work on
   - GetEnumerator() method should use a separate class EndlessCycleEnumerator
1. Implement a class EndlessCycleEnumerator&lt;T&gt;
   - It should implements IEnumerator&lt;T&gt;

### Guidance &#x2B50;
- Should not use LINQ functionality
- Should not use `yield` keyword
 
### Questions &#x2753;
- What if the source sequence is empty? does EndlessCycle is no longer infinite?