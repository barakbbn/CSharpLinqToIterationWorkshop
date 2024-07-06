using System;
using System.Collections;
using System.Collections.Generic;

namespace Exercise_1A
{
    public class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Exercise 1A");
        }
    }

    // ==========
    // Exercise 1
    // ==========

    // TODO: Implement IEnumerable<int>
    public class CountdownSequence
    {
        public CountdownSequence(int count)
        {
            // TODO: Add proper implementation
            throw new NotImplementedException();
        }

        // TODO: Implement GetEnumerator()
        // Always have the explicit interface IEnumerable.GetEnumerator() call the local GetEnumerator()
    }





    // ==========
    // Exercise 2
    // ==========


    // TODO: Implement IEnumerable<T>
    public class  DebugEnumerable<T>
    {

        public DebugEnumerable(IEnumerable<T> source)
        {
            // TODO: Add proper implementation
            throw new NotImplementedException();
        }


        private class DebugEnumerator // TODO: Implement IEnumerator<T>
        {
            // TODO: add parameter to constructor  (DebugEnumerable<T> owner)
            public DebugEnumerator()
            {
                // TODO: Add proper implementation
                throw new NotImplementedException();
            }

            // TODO: Implement Dispose()
        }
    }
}
