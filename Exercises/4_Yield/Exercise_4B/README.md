## Exercise 4B - Students reader

**Difficulty:** Experienced/Advanced  
**Time:** 60 min
___
In this exercise we'll practice the use of `yield` keyword.  
The challenge is to lazily load students info from a CSV file and convert them to StudentInfo objects.  
In addition it should be done for several files

### Instructions &#x1F4DA;
- Implement a class `StudentsFileEnumerator` - Responsible for reading students info from a CSV file
  - It should implements `IEnumerable<StudentInfo>` interface
  - Constructor signature: (string studentsCsvFilePath)
  - GetEnumerator() should use `yield return`

### Guidance &#x2B50;
- Should not use LINQ functionality
- Make sure to close/dispose the file
- Lazily read students from the CSV file (not all at once)
- &#x2757; WARN: watch out from empty lines

<br>
<br>
<br>
<br>
<br>
<br>
<br>
<br>
<br>
<br>
<br>
<br>
<br>
<br>
<br>
<br>
<br>
<br>
<br>
<br>
<br>
<br>
<br>
<br>
<br>
<br>
<br>
<br>
<br>
<br>
<br>
<br>
<br>
<br>
<br>
<br>
<br>
<br>
<br>
<br>
<br>

> HINT: To open the CSV file as Stream: `File.OpenText(filePath);`
>       Remember to Dispose it when done (Or use **using** keyword)