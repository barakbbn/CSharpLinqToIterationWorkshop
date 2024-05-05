## &#x1F381; BONUS Exercise 4C - Querying Students

**Difficulty:** Experienced  
**Time:** 60 min  
___
In this exercise we'll practice the use of **`yield`** keyword by simply implementing the logic in local method instead of creating a dedicated class for it.  
For that we reuse the `StudentsFileEnumerator` class from exercise 4B.


### Instructions &#x1F4DA;
Implement the following methods using the `yield return` keyword  

1. Implement method **Convert()** .   
   It applies a given function to each element of an IEnumerable, returning a new IEnumerable of converted elements.
   ```csharp
   public static IEnumerable<TResult> Convert<T, TResult>(IEnumerable<T> source, Func<T, TResult> converter)
   ```
   &#x1F4A1; Example
   ```csharp
   var numbers = new[]{1, 2, 3};
   var converted = Convert(numbers, n => n * n); // Power of 2
   // {1, 4, 9}
   ```
   ___
1. Implement method **StudentsByGrades** that take only students within a range of grades (filtering out the rest),  
   ```csharp
   public static IEnumerable<StudrentInfo> StudentsByGrades(IEnumerable<StudentInfo> source, int fromGrade, int tillGrade)
   ```
   &#x1F4A1; Example  
   ```csharp
   var students = new StudentsReader("Students\\Semester1.csv");
   var topStudents = StudentsByGrade(students, 90, 100);
   ```
   ___
1. Implement method **SideEffect()** that run an action on each item in the sequence. it should return the same sequence.
   ```csharp
   public static IEnumerable<T> SideEffect<T>(IEnumerable<T> source, Action<T> action)
   ```
   &#x1F4A1; Example  
   ```csharp
   var maxGrade = 0;
   var students = new StudentsReader("Students\\Semester1.csv");

   var sameStudents = SideEffect(students, student => maxGrade = Math.Max(maxGrade, student.Grade) );
   var stillSameStudents = SideEffect(sameStudents, student => 
                           {
                               if (student.Name == "Son of Mayor") student.Grade = 100;
                           });

   //Must execute the sequence so SideEffect will actually do its job
   foreach(var student in stillSameStudents)
   {
       Console.WriteLine(student);
   }
   ```
   ___
4. Using the above methods, perform the following in the Main() method:
   - Loop each students CSV file in `Students` sub-folder
   - Create StudentsFileEnumerator instance on the students CSV file.  

     **Perform the following on StudentsFileEnumerator instance (`IEnumerable<StudentInfo>`)**  
     > Without iterating the source sequences more than once

   - For each StudentInfo, write to the Console
   - Take only the emails of all failed students (Grade < 55)
   - **Convert** the emails into a single string delimited by semicolon (like in Outlook *TO:* field)  
     `a1@mail.com;b2@mail.com;c3@mail.com`
5. &#x1F381; BONUS: Find a dirty trick to calculate average grade without iterating the students again (SideEffect)
6. &#x1F381; BONUS: Some students complain they got emailed more than once, can you fix it


### Guidance &#x2B50;
- Should use `yield return` expression
- Should not use LINQ functionality
