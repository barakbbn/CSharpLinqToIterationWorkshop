using System;
using System.Collections;
using System.Collections.Generic;

namespace Demo2
{
    // Iterate strings and return only those which are numeric
    public class OnlyNumbers //: IEnumerable<int>
    {
        private readonly IEnumerable<string> _source;

        public OnlyNumbers(IEnumerable<string> source)
        {
            _source = source;
        }
    }
}
