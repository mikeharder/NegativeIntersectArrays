using System;
using System.Collections.Generic;
using System.Text;

namespace NegativeIntersectArrays
{
    public static class SH_Implementation
    {
        public static int[] NegativeIntersectArrays(int[] source, IReadOnlyList<int[]> exceptions)
        {
            var sourceLength = source.Length;
            if (sourceLength == 0)
                return new int[0];

            // Pointer into initial array
            Span<int> matches = new Span<int>(source, 0, source.Length);

            int lastWorkingLength = sourceLength;
            var maxSearch = source[sourceLength - 1];
            for (var i = 0; i < exceptions.Count; i++)
            {
                var arr1Len = sourceLength;
                var arr2Len = exceptions[i].Length;
                var baseArray = source;
                var comparedArray = exceptions[i];
                int baseArrayIndex = 0, comparedArrayIndex = 0;
                int writeIndex = 0;

                if (comparedArray.Length == 0 || comparedArray[0] > maxSearch)
                {
                    continue; // this entire range is out of the range of the first, nothing will be found..
                }

                while (baseArrayIndex < arr1Len && comparedArrayIndex < arr2Len)
                {
                    var baseVal = baseArray[baseArrayIndex];
                    var compVal = comparedArray[comparedArrayIndex];
                    if (compVal < baseVal)
                    {
                        comparedArrayIndex++;
                    }
                    else if (baseVal == compVal)
                    {
                        baseArrayIndex++;
                        comparedArrayIndex++;
                    }
                    else
                    {
                        matches[writeIndex++] = baseVal;
                        baseArrayIndex++;
                    }
                }

                while (baseArrayIndex < arr1Len) // if the baseArray is longer than the array we have to except, we need to copy in the rest of the items
                {
                    matches[writeIndex++] = baseArray[baseArrayIndex];
                    baseArrayIndex++;
                }
                matches = new Span<int>(source, 0, writeIndex);
            }

            return matches.ToArray();
        }
    }
}
