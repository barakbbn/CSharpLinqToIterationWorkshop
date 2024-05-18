## Exercise 4A - Yield keyword

**Difficulty:** Experienced/Advanced  
**Time:** 60 min  
___
In this exercise we'll practice the use of `yield` keyword.  

### Exercise 1 - Remove Nulls from input sequence &#x1F4DA;

The class implementation should take an input sequence and yield return only non-null values from it
- Implement class RemoveNulls. 
  - It should implements interface IEnumerable&lt;T&gt;
  - GetEnumerator<T> method should use yield keyword

```csharp
public class RemoveNulls<T> : IEnumerable<T>
{
    // TODO: Implement
    public RemoveNulls(IEnumerable<T> source) {}
}
```

### Exercise 2 - FizzBuzzSequence refactored &#x1F4DA;

Implement FizzBuzzSequence from exercise 1B using a method instead of a class

```csharp
class Exercises
{
    public static IEnumerable<string> FizzBuzzSequence(IEnumerable<int> source)
    {
        // TODO: use yield return
    }

    // Bonus
    public static IEnumerable<string> FizzBuzzSequenceBonus(IEnumerable<int> numbers) { }
}
```


### Exercise 3 - Fibonacci sequence &#x1F4DA;
The [Fibonacci Sequence](https://www.mathsisfun.com/numbers/fibonacci-sequence.html) is the series of numbers: 0, 1, 1, 2, 3, 5, 8, 13, 21, 34, ...  
The next number is found by adding up the two numbers before it:

the 2 is found by adding the two numbers before it (1+1),  
the 3 is found by adding the two numbers before it (1+2),  
the 5 is (2+3),  
and so on!

Implement Fibonacci() method that yield the values of Fibonacci sequence

```csharp
class Exercises
{
    public static IEnumerable<int> Fibonacci(int count)
    {
        // TODO: use yield return
    }
}
```



**Example**  

```csharp
var source = new object[] { 0, 1, null,  2, 2, null };
var nonNull = new RemoveNulls(source);
// Will Generate: 0, 1, 2, 2
```

### &#x1F381; Bonus: Exercise 4 - Binary Tree

Complete implementation of `BinaryTreeNode` class
- Implement interface IEnumerable&lt;T&gt;
  - GetEnumerator<T>() method should use yield keyword
  - First it should yield return all the left side, 
  - Then the `Value` property of the current node
  - And lastly, the right side of the tree
  > Keep in mind that the node's Left or Right properties can be `null` and therefore should be ignored

**Example**  

```csharp
var tree = new BinaryTreeNode<string>
{
    Value = "Top",
    Left = new BinaryTreeNode<string>
    {
        Value = "Left.1",
        Left = new BinaryTreeNode<string> { Value = "Left.1.1" }
    },
    Right = new BinaryTreeNode<string>
    {
        Value = "Right.1",
        Left = new BinaryTreeNode<string> { Value = "Left.2.1" },
        Right = new BinaryTreeNode<string> { Value = "Right.1.1" }
    }
}

foreach(var value in tree)
{
    Console.WriteLine(value);
}
// Output:
// Left.1.1
// Left.1
// Top
// Left.2.1
// Right.1
// Right.1.1
```
