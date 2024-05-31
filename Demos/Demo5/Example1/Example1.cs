using System;

namespace Demo5
{
    public class Example1
    {
        //Show infinite iteration
        public void Run()
        {
            const int limit = 30;
            int count = 0;

            #region manual break of iteration
            Console.WriteLine("Iterating infinite sequence, stopping after {0} items", limit);
            Console.WriteLine("-------------------------");
            #endregion

            foreach (var value in new RandomSequence())
            {
                Console.Write("{0} , ", value);
                count++;
                if (count >= limit) break;
            }
        }
    }
}