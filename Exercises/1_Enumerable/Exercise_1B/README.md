## Exercise 1B: Fizz Buzz Divisibility Game

**Difficulty:** Beginner  
**Time:** 20 min  
___

In this exercise we'll practice implementation of `IEnumerable` and `IEnumerator`.  
The challenge is to write code that takes a given sequence of numbers (IEnumerable&lt;int&gt;)  
and for each number:  
- Return **"Fizz"** if the number is divisible by 3.  
- Return **"Buzz"**" if the number is divisible by 5.  
- Return **"FizzBuzz"** if the number is divisible by both 3 and 5.  
- Otherwise, return the number itself as string

**Example**  

```csharp
var source = new[]{1, 2, 3, 4, 5, 6, 15 ,9 ,10, 11};
var fizzBuzz = new FizzBuzzSequence(source);
// Will Generate: "1", "2", "Fizz", 4, "Buzz", "6", "FizzBuzz", "Fizz", "Buzz", "11"
//                           --3--      --5--        ---15---    --9--  --10--
```

### Instructions &#x1F4DA;  
- Implement a class **FizzBuzzSequence** and **FizzBuzzEnumerator**
  ```csharp
  public class FizzBuzzEnumeratorSequence // TODO: Implement IEnumerable<string>
  {
      public FizzBuzzSequence(IEnumerable<int> source) {}
      // TODO: Implement GetEnumerator() 
  }
  public class FizzBuzzEnumerator // TODO: Implement IEnumerator<string>
  {
  } 
  ``` 

### Guidance &#x2B50;
- Should not use LINQ functionality
- Should not use `yield` keyword
- &#x2757; Edge case: if number is **ZERO**, it should returned as **"0"** (Zero as string)

### &#x1F381; Bonus

**Difficulty:** Intermediate  
**Time:** 20 min  

Change implementation of **FizzBuzzEnumerator** that when the number is divisible both by 3 and 5,  
It will produce 2 values, "Fizz" then "Buzz" (instead of single value "FizzBuzz")
```csharp
var source = new[]{1, 3, 4, 5, 15 ,9 ,10, 11};
var fizzBuzz = new FizzBuzzSequence(source);
// Will Generate: "1", "Fizz", "4", "Buzz", "Fizz", "Buzz", "Fizz", "Buzz", "11"
//                      --3--        --5--  |---- 15 ----|   --9--  --10--
```