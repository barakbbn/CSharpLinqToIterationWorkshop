using System.Collections.Generic;
using System;

namespace Demo4
{
    internal class DeferredHelper : IHelper
    {
        public IEnumerable<T> Limit<T>(IEnumerable<T> values, int limit)
        {
            var count = 0;
            foreach (var value in values)
            {
                if (count == limit) break;
                yield return value;
                count++;
            }
        }

        public IEnumerable<int> OnlyPositive(IEnumerable<int> values)
        {
            foreach (var n in values)
            {
                if (n > 0) yield return n;
            }
        }


        public IEnumerable<TOut> Calc<TIn, TOut>(IEnumerable<TIn> values, Func<TIn, TOut> mathOperation)
        {
            foreach (var item in values)
            {
                var result = mathOperation(item);
                yield return result;
            }
        }
    }
}