using System;
using System.Collections.Generic;

namespace Demo4
{
    internal class ImmediateHelper : IHelper
    {
        public IEnumerable<T> Limit<T>(IEnumerable<T> values, int limit)
        {
            var results = new List<T>();
            var count = 0;
            foreach (var value in values)
            {
                if (count == limit) break;
                results.Add(value);
                count++;
            }

            return results;
        }

        public IEnumerable<int> OnlyPositive(IEnumerable<int> values)
        {
            var results = new List<int>();
            foreach (var n in values)
            {
                if (n > 0) results.Add(n);
            }

            return results;
        }


        public IEnumerable<TOut> Calc<TIn, TOut>(IEnumerable<TIn> values, Func<TIn, TOut> mathOperation)
        {
            var results = new List<TOut>();
            foreach (var item in values)
            {
                var result = mathOperation(item);
                results.Add(result);
            }

            return results;
        }
    }
}
