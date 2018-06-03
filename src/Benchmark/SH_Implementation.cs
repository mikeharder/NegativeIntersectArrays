using System;
using System.Collections.Generic;

namespace NegativeIntersectArrays
{
    public static class SH_Implementation
    {
        public static System.Buffers.ArrayPool<int> arrayPool = System.Buffers.ArrayPool<int>.Create();

        //public static ObjectPool<int[]> temporaryArrays =
        //    new DefaultObjectPool<int[]>(
        //        new DefaultPooledObjectPolicy<int[]>(), 20
        //        );

        static int[] emptyArray = new int[0];
        public static int[] NegativeIntersectArrays(int[] readonlySource, IReadOnlyList<int[]> exceptions)
        {
            var sourceLength = readonlySource.Length;
            if (sourceLength == 0)
                return emptyArray;

            var source = arrayPool.Rent(sourceLength);
            //var source = new int[sourceLength];
            Buffer.BlockCopy(readonlySource, 0, source, 0, sourceLength * sizeof(int));

            int lastWorkingLength = sourceLength;
            var maxSearch = source[sourceLength - 1];
            for (var i = 0; i < exceptions.Count; i++)
            {
                var arr1Len = lastWorkingLength;
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
                        ++baseArrayIndex;
                        ++comparedArrayIndex;
                    }
                    else
                    {
                        source[writeIndex++] = baseVal;
                        ++baseArrayIndex;
                    }
                }

                while (baseArrayIndex < arr1Len) // if the baseArray is longer than the array we have to except, we need to copy in the rest of the items
                {
                    source[writeIndex++] = baseArray[baseArrayIndex];
                    baseArrayIndex++;
                }
                lastWorkingLength = writeIndex;
            }
            //return new Span<int>(source, 0, lastWorkingLength).ToArray();
            var returnable = new int[lastWorkingLength];
            Buffer.BlockCopy(source, 0, returnable, 0, sizeof(int) * lastWorkingLength);
            arrayPool.Return(source);

            return returnable;
            //Array.Copy()
            //Array.Resize(ref source, lastWorkingLength);
            //return source;
        }
    }
}
