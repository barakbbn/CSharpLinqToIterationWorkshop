## Exercise 3A - Let's get lazy &#x1F971; 

**Difficulty:** Experienced/Advanced  
**Time:** 60 min  
___

In this exercise we'll practice making the iteration logic as lazy as possible.  
The challenge is to refactor Exercises 1B,1C and 2A to defer the iteration logic till MoveNext() is called and to ensure not all data is prepared in advance.

### Instruction &#x1F4DA;
- Copy `FizzBuzzSequence` class from exercise 1B into this project and refactor it to be lazy
- Copy `WavelengthsSampling` class from exercise 1C into this project and refactor it to be lazy
- &#x1F381; Bonus: Copy `DistinctUntilChanged` class from exercise 1D into this project and refactor it to be lazy

### Guidance &#x2B50;
- Make sure implementation doesn't fetch all source sequence items or copy/prepare them to local collection in advance  
  This mainly apply to implementation of:
	- GetEnumerator()
	- Constructor
	- MoveNext()
- In case working on a source sequence (i.e. FizzBuzzSequence), make sure it's being disposed when calling Dispose()  
  Or better, Dispose it as soon as MoveNext about to return false
- &#x1F4A1; HINT: You probably need to call source.GetEnumerator() and iterate in primitive manner

### Ask yourself this &#x2753;
Which of the following sequences is Lazy, Semi-lazy or Eager

| Lazy   | Semi-lazy   | Eager   | *Sequence* |
| :----: | :---------: | :-----: | ---------- |
| <input type="checkbox"> | <input type="checkbox"> | <input type="checkbox"> | Filter out `null` elements from source collection | 
| <input type="checkbox"> | <input type="checkbox"> | <input type="checkbox"> | Shuffle source collection | 
| <input type="checkbox"> | <input type="checkbox"> | <input type="checkbox"> | Sort source collection | 
| <input type="checkbox"> | <input type="checkbox"> | <input type="checkbox"> | Reverse source collection (what if source collection is an Array) | 
| <input type="checkbox"> | <input type="checkbox"> | <input type="checkbox"> | Calculating distance of each numeric value in source collection, from the average of the numbers (n => n-avg) | 
| <input type="checkbox"> | <input type="checkbox"> | <input type="checkbox"> | Keeping only distinct (unique) values of source collection | 
