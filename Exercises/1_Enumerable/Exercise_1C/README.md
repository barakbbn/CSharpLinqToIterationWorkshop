## Exercise 1C: Wavelengths Sampling

**Difficulty:** Intermediate/Experienced  
**Time:** 45 min  
___

In this exercise we'll practice implementation of IEnumerable and IEnumerator.  
The challenge is to write code that for a given range of wavelengths, generates desired amount of sampled wavelengths.  

**Example**  
```csharp
// Basically, Wavelengthsampling is just a range of numbers with a step/gap between each.

new Wavelengthsampling(min=10, max=100, amount=10);
// Will generate 10 values between 10 and 100 (inclusive):
// 10, 20, 30, 40, 50, 60, 70, 80, 90, 100

new Wavelengthsampling(235.0, 970.0, 245);  
// Will generate 245 values: 235.0, 238.012, 241.025, ... , 963.975, 966.988, 970
```

### Instructions &#x1F4DA;
- Implement a class **WavelengthsSampling** and **WavelengthsSamplingEnumerator**
  ```csharp
  public class WavelengthsSampling // TODO: Implement IEnumerable<double>
  {
      public WavelengthsSampling(double min, double max, int amount) {}
      // TODO: Implement GetEnumerator() 
  }
  public class WavelengthsSamplingEnumerator // TODO: Implement IEnumerator<double>
  {
  } 
  ``` 
- &#x1F381; Bonus:   
  Implement method `WavelengthsSampling.ChangeAmount()`
  -  If while iterating the sampled wavelengths, `ChangeAmount()` is being called with different amount then current,  
     Then next call to `WavelengthsSamplingEnumerator.MoveNext()` should throws `InvalidOperationException`

### Guidance &#x2B50;
- Should not use LINQ functionality
- Should not use `yield` keyword
- &#x1F4A1; HINT: To calculate the gap between the wavelengths, do: **(max - min) / (amount -1)**

### Questions &#x2753;
1. Is it correct to say that the generated wavelengths are sorted?
