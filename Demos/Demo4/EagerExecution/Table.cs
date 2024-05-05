using System.Collections;
using System.Collections.Generic;

namespace Demo4.EagerExecution
{
    public class Table<T> : IEnumerable<T>
    {
        private readonly Cell[] _cells;

        public Table(int rows, int columns)
        {
            Rows = rows;
            Columns = columns;
            _cells = new Cell[rows * columns];
        }

        public int Rows { get; }
        public int Columns { get; }

        public IEnumerator<T> GetEnumerator()
        {
            T[] values = new T[Rows * Columns];
            for (int i = 0; i < _cells.Length; i++)
            {
                values[i] = _cells[i].Value;
            }

            return (IEnumerator<T>)values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public T this[int row, int column]
        {
            get
            {
                int index = row*Columns + column;
                Cell cell = _cells[index] ?? (_cells[index] = new Cell(row, column));
                return cell.Value;
            }
            set
            {
                int index = row * Columns + column;
                Cell cell = _cells[index] ?? (_cells[index] = new Cell(row, column));
                cell.Value = value;
            }
        }

        public class Cell
        {
            public Cell(int row, int column)
            {
                Row = row;
                Column = column;
            }

            public int Row { get; }
            public int Column { get; }
            public T Value { get; set; }
        }
    }
}