## Exercise 5B - LINQ Extension methods

**Difficulty:** Advanced  
**Time:** 60 hours  
___
In this exercise we'll practice using the extension methods from previous exercise  
to perform business logic tasks.  
You can use existing LINQ functionalities and other elements you done and learnt in previous exercises.  

### Instructions:  
> Review the code in Main() method and perform the following:

&#x1F4D1; Passengers need to check-in their baggage for a flight.  
There are two check-in counters, one for VIP passengers, and one for the rest (general).  
1. Implement method **CheckInPassengers()** that will perform check-in for the passengers.  
   The method returns the baggage of the checked-in passengers (CheckInCounter.CheckIn method)  
   [  
   &nbsp;&nbsp;&nbsp;&nbsp;Passenger 1 bags: [ &#x1F45C; ]  
   &nbsp;&nbsp;&nbsp;&nbsp;Passenger 2 bags: [ &#x1F6CD; &#x1F6CD; ]  
   &nbsp;&nbsp;&nbsp;&nbsp;Passenger 3 bags: [ &#x1F9F3; &#x1F9F3; &#x1F381; ]  
   ]

![image](airport.png)

&#x1F4D1; The baggage from the two check-in counters are travelled to a single main conveyor belt,  
therefore they must be combined into a single sequence.

2. Implement method **CombineAllBaggage()** that combine the baggage of the VIP and general passengers together,  
   by **concatenating** them together, and return a **flattened** sequence of baggage  
   &#x2757; Do not handle passengers without baggage



> &#x1F381; Bonus: Instead of **Concatenating** the baggage, interleave them together using **Interleave** extension method from previous exercise.

&#x1F4D1; In case there are many baggage, there might need to use a tractor to carry them to the airplane.

3. Implement method **ShouldLoadBaggageUsingTractor()** that determines if there are at least 20 baggage

&#x1F381; Bonus:   
It seems the flight was overbooked, and some passengers where bumped off the flight.  

4. Implement method **RemovePassengersBaggage()** that will remove the baggage of certain passengers by their IDs  

&#x1F4D1; Finally,
5. Implement method **PrintBaggage()** that loop over the baggage using **ForEach** extension method  
   and print to the console each baggage  
