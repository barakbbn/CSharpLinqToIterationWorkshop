using System.Collections.Generic;
using System;

namespace Demo4
{
    internal class DeferredHelper : IHelper
    {
        public IEnumerable<T> Limit<T>(IEnumerable<T> values, int limit)
        {
            // TODO: refactor ImmediateHelper.Limit using yield return
            throw new NotImplementedException();
        }

        public IEnumerable<int> OnlyPositive(IEnumerable<int> values)
        {
            // TODO: refactor ImmediateHelper.OnlyPositive using yield return
            throw new NotImplementedException();
        }


        public IEnumerable<TOut> Calc<TIn, TOut>(IEnumerable<TIn> values, Func<TIn, TOut> mathOperation)
        {
            // TODO: refactor ImmediateHelper.Calc using yield return
            throw new NotImplementedException();
        }

    }
}