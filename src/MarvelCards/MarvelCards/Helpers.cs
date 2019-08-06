using System;
using System.Collections.Generic;
using System.Text;

namespace MarvelCards
{
    public class Helpers
    {
        public static double LimitToRange(double value, double inclusiveMinimum, double inlusiveMaximum)
        {
            if (value >= inclusiveMinimum)
            {
                return value <= inlusiveMaximum ? value : inlusiveMaximum;
            }

            return inclusiveMinimum;
        }
    }
}
