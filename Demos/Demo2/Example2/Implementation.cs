using System;
using System.Collections;
using System.Collections.Generic;
using TReturn = System.Object;

namespace Demo2
{
    // 1. declare a class that implements IEnumerable<TReturn>
    // 2. Use IDE to implement IEnumerable

    public class ImplementationOfIEnumerable : IEnumerable<TReturn>
    {
        // Return new custom Enumerator(this) - Use IDE to create the class (as nested class) and also constructor
        public IEnumerator<TReturn> GetEnumerator() => new ImplementationOfIEnumerator(parent: this);

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator(); // This line always looks the same (Copy/Paste code)

        // 3. Use IDE to implement IEnumerator
        private class ImplementationOfIEnumerator : IEnumerator<TReturn>
        {
            private ImplementationOfIEnumerable _owner;

            // Optionally, use state machine logic to manage the flow of iteration
            private int _state; // 0 - Not started/initialized , -1 - Completed , positive - in progress

            public ImplementationOfIEnumerator(ImplementationOfIEnumerable parent)
            {
                this._owner = parent;
            }

            public TReturn Current { get; private set; } // Preferred implementation that looks the same (Copy/Paste code)

            object IEnumerator.Current => Current; // This line always looks the same (Copy/Paste code)

            public void Dispose()
            {
                // Empty implementation or Dispose logic. e.g. `this.sourceEnumerator?.Dispose()`
            }

            // ===================
            public bool MoveNext()
            {
                // Use `this._owner` properties and fields if needed

                // when there is a value to return, assign it to `Current` and return `true`
                // Otherwise return `false`

                // If you choose so, use `this._state` for manage the logic
                return false;
            }

            // ===================

            public void Reset() => throw new NotImplementedException(); // This line always looks the same (Copy/Paste code)
        }
    }
}
