using System;
using System.Collections;
using System.Collections.Generic;

namespace Demo2
{
    class Table<T> : IEnumerable<T>
    {
        private readonly T[,] _cells;

        public Table(int rows, int columns)
        {
            Rows = rows;
            Columns = columns;
            _cells = new T[rows, columns];
        }

        public int Rows { get; }
        public int Columns { get; }

        public IEnumerator<T> GetEnumerator()
        {
            return new TableEnumerator(this);
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public T this[int row, int column]
        {
            get { return _cells[row, column]; }
            set { _cells[row, column] = value; }
        }

        class TableEnumerator : IEnumerator<T>
        {
            private readonly Table<T> _owner;
            private int _currentRow, _currentColumn;

            public TableEnumerator(Table<T> owner)
            {
                _owner = owner;
                _currentRow = 0;
                _currentColumn = -1; //-1 indicate iteration not started yet
            }

            public bool MoveNext()
            {
                //is ended
                if (_currentRow == _owner.Rows) return false;

                _currentColumn++;
                if (_currentColumn == _owner.Columns) // If moved past last column, proceed to start of next row
                {
                    _currentColumn = 0;
                    _currentRow++;
                }

                //is ended
                if (_currentRow == _owner.Rows) return false;

                Current = _owner[_currentRow, _currentColumn];

                return true;
            }

            //What is the best practice? 
            //to remember the current value in a field/property (_currentValue), 
            //or the fetch it from the owner Table object every time, according to current row & column (_owner[_currentRow, _currentColumn])
            public T Current { get; private set; }

            object IEnumerator.Current => Current;

            public void Dispose()
            {
            }

            public void Reset() => throw new NotImplementedException();
        }
    }
}