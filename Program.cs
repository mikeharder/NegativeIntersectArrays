using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace NegativeIntersectArrays
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
        }

        public void NegativeIntersectionPerformance()
        {
            Random rr = new Random(51292);

            int[] baseA = new int[rr.Next(0, 300000)];
            int[] baseB = new int[rr.Next(0, 100000)];
            var fill = new Func<int[], bool>((int[] x) =>
            {
                for (int i = 0; i < x.Length; i++)
                {
                    x[i] = rr.Next(500, 3000000);
                }
                Array.Sort(x);
                return true;
            });

            fill(baseA);
            fill(baseB);
            var baseALength = baseA.Length;
            //Assert.True(baseALength > 20000);

            var nega = new[] { baseB };

            Stopwatch sw = Stopwatch.StartNew();

            const int repetitions = 1000;
            int[] k = null;
            for (int i = 0; i < repetitions; i++)
            {
                k = NegativeIntersectArrays(baseA, nega);
            }

            var swT = sw.Elapsed.TotalMilliseconds;
            Console.WriteLine($"{swT} time for {repetitions} excluding intersections. Array Size b4: {baseALength} after: {k.Length}. Per Exec: {swT / repetitions}ms");

        }


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


        //[Test]
//         public void NegativeIntersectionWorks()
//         {

// #pragma warning disable HeapAnalyzerBoxingRule // Value type to reference type conversion causing boxing allocation


//             int[] baseA;
//             int[][] nega;
//             int[] result;

//             baseA = new[] { 1, 2, 3 };
//             nega = new[] { new[] { 1, 2, 3 } };
//             result = IndexSearch.NegativeIntersectArrays(baseA, nega);
//             Assert.AreEqual(0, result.Length);

//             baseA = new[] { 1, 2, 3 };
//             nega = new[] { new[] { 4, 5, 6 } };
//             result = IndexSearch.NegativeIntersectArrays(baseA, nega);
//             Assert.AreEqual(3, result.Length);
//             Assert.AreEqual(3, result[2]);


//             #region idiot proofing
//             baseA = new int[0];
//             nega = new[] { new[] { 4, 5, 6, 8, 10, 14 } };
//             result = IndexSearch.NegativeIntersectArrays(baseA, nega);
//             Assert.AreEqual(0, result.Length);

//             baseA = new[] { 4, 5, 6, 9, 10, 14 };
//             nega = new[] { new int[0] };
//             result = IndexSearch.NegativeIntersectArrays(baseA, nega);
//             Assert.AreEqual(baseA.Length, result.Length);

//             #endregion

//             baseA = new[] { 4, 5, 6, 9, 10, 14 };
//             nega = new[] { new[] { 4, 5, 6, 8, 10, 14 } };
//             result = IndexSearch.NegativeIntersectArrays(baseA, nega);
//             Assert.AreEqual(1, result.Length);
//             Assert.AreEqual(9, result[0]);

//             baseA = new[] { 4, 5, 6, 9 };
//             nega = new[] { new[] { 4, 9, 12, 14 } };
//             result = IndexSearch.NegativeIntersectArrays(baseA, nega);
//             Assert.AreEqual(2, result.Length);
//             Assert.AreEqual(5, result[0]);

//             #region firstLast
//             baseA = new[] { 3, 5, 6, 9 };
//             nega = new[] { new[] { 4, 5, 6, 9 } };
//             result = IndexSearch.NegativeIntersectArrays(baseA, nega);
//             Assert.AreEqual(1, result.Length);
//             Assert.AreEqual(3, result[0]);

//             baseA = new[] { 1, 9 };
//             nega = new[] { new[] { 1, 10 } };
//             result = IndexSearch.NegativeIntersectArrays(baseA, nega);
//             Assert.AreEqual(1, result.Length);
//             Assert.AreEqual(9, result[0]);
//             #endregion


//             baseA = new[] { 1, 9, 101 };
//             nega = new[] { new[] { 1, 10, 11, 14, 16, 17, 122 } };
//             result = IndexSearch.NegativeIntersectArrays(baseA, nega);
//             Assert.AreEqual(2, result.Length);
//             Assert.AreEqual(101, result[1]);

//             baseA = new[] { 4, 5, 6, 7, 8 };
//             nega = new[] { new[] { 1, 2, 3, 5, 6, 7 } };
//             result = IndexSearch.NegativeIntersectArrays(baseA, nega);
//             Assert.AreEqual(2, result.Length); // 4,8
//             Assert.AreEqual(4, result[0]);

//             #region Extended range shortcuts
//             baseA = new[] { 4, 5, 6, 9, 12, 14, 16, 19, 22, 24, 27, 30, 31, 35, 37, 201 };
//             nega = new[] { new[] { 4, 9, 12, 14 } };
//             result = IndexSearch.NegativeIntersectArrays(baseA, nega);
//             Assert.AreEqual(baseA.Length - 4, result.Length);
//             Assert.AreEqual(5, result[0]);

//             baseA = new[] { 4, 5, 6, 9 };
//             nega = new[] { new[] { 4, 9, 12, 14, 16, 19, 20, 22, 240, 241, 242 } };
//             result = IndexSearch.NegativeIntersectArrays(baseA, nega);
//             Assert.AreEqual(2, result.Length);
//             Assert.AreEqual(5, result[0]);
//             #endregion

// #pragma warning restore HeapAnalyzerBoxingRule // Value type to reference type conversion causing boxing allocation
//         }
     }
}
