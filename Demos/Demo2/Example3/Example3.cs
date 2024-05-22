using System;

namespace Demo2
{
    /// <summary>
    /// New standalone type of custom collection
    /// </summary>
    public class Example3
    {
        public void Run()
        {
            // Create 3x3 Table object
            const int rows = 3, columns = 3;
            var table3x3 = new Table<int>(rows, columns);
            // Fill table with value
            for (int row = 0; row < rows; row++)
            {
                for (int column = 0; column < columns; column++)
                {
                    table3x3[row, column] = row * 1000 + column;
                }    
            }


            foreach (int number in table3x3)
            {
                Console.WriteLine(number);
            }
        }
        
    }
}