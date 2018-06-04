using System;
using System.Collections.Generic;

namespace NegativeIntersectArrays
{
    public static class Mike_Implementation
    {
        public static System.Buffers.ArrayPool<int> arrayPool = System.Buffers.ArrayPool<int>.Create();

        public static int[] NegativeIntersectArrays(int[] readonlySource, IReadOnlyList<int[]> exceptions)
        {
            var minExceptionIndexes = new int[exceptions.Count];
            var minException = int.MaxValue;
            var minExceptionIndex = -1;
            GetMinException(exceptions, minExceptionIndexes, ref minException, ref minExceptionIndex);

            var result = arrayPool.Rent(readonlySource.Length);
            var resultIndex = 0;

            var sourceIndex = 0;

            while (sourceIndex < readonlySource.Length)
            {
                var current = readonlySource[sourceIndex];
                if (current < minException)
                {
                    result[resultIndex] = current;
                    resultIndex++;
                    sourceIndex++;
                }
                else if (current > minException)
                {
                    minExceptionIndexes[minExceptionIndex]++;
                    GetMinException(exceptions, minExceptionIndexes, ref minException, ref minExceptionIndex);
                }
                else
                {
                    // current == minException, so skip
                    sourceIndex++;
                }
            }

            var finalResult = new int[resultIndex];
            Array.Copy(result, finalResult, resultIndex);
            arrayPool.Return(result);

            return finalResult;
        }

        private static void GetMinException(IReadOnlyList<int[]> exceptions, int[] minExceptionIndexes, ref int minException, ref int minExceptionIndex)
        {
            var min = int.MaxValue;
            var minIndex = -1;

            for (var i = 0; i < minExceptionIndexes.Length; i++)
            {
                var currentException = exceptions[i];
                var currentIndex = minExceptionIndexes[i];

                var current = (currentIndex < currentException.Length) ? currentException[currentIndex] : int.MaxValue;

                if (current < min)
                {
                    min = current;
                    minIndex = i;
                }
            }

            minException = min;
            minExceptionIndex = minIndex;
        }
    }
}
