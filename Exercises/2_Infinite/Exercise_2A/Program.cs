using System;
using System.Collections;
using System.Collections.Generic;

namespace Exercise_2A
{
    public class Program
    {
        static void Main(string[] args) { }
    }

    // TODO: Implement IEnumerable<T>
    public class EndlessCycle<T>
    {
        public EndlessCycle(IEnumerable<T> source)
        {
            // TODO: Add proper implementation
            throw new NotImplementedException();
        }
    }

    // TODO: Create class EndlessCycleEnumerator that implements IEnumerator<T>
}
