using System;
using System.Collections;
using System.Collections.Generic;
using Xunit;
using System.Linq;

namespace Tests
{

    public class NegativeIntersectionTests
    {
        public delegate int[] Implementation(int[] source, IReadOnlyList<int[]> exceptions);
        public static IEnumerable<object[]> Data()
        {
            object[] make(string label, Implementation implementation) => new object[] { (implementation).Labeled(label) };

            yield return make("Mike", NegativeIntersectArrays.Mike_Implementation.NegativeIntersectArrays);
            yield return make("Joe", NegativeIntersectArrays.SH_Implementation.NegativeIntersectArrays);
            yield return make("Joe Unsafe", NegativeIntersectArrays.SH_Unsafe.NegativeIntersectArrays);
            //yield return make("Joe SIMD", NegativeIntersectArrays.SH_SIMD.NegativeIntersectArrays);
        }


        [Theory, MemberData(nameof(Data))]
        public void FullNegativeIntersectionRemovesAll(Labeled<Implementation> NegativeIntersectArrays)
        {
            var baseA = new[] { 1, 2, 3 };
            var nega = new[] { new[] { 1, 2, 3 } };
            var result = NegativeIntersectArrays.Data(baseA, nega);
            Assert.Empty(result);
        }

        [Theory, MemberData(nameof(Data))]
        public void ExtendedRangeDoesNothing(Labeled<Implementation> NegativeIntersectArrays)
        {
            var baseA = new[] { 1, 2, 3 };
            var nega = new[] { new[] { 4, 5, 6 } };
            var result = NegativeIntersectArrays.Data(baseA, nega);
            Assert.Equal(3, result.Length);
            Assert.Equal(3, result[2]);
        }


        [Theory, MemberData(nameof(Data))]
        public void MultipleRangesDontCauseExplosions(Labeled<Implementation> NegativeIntersectArrays)
        {
            var baseA = new[] { 4, 5, 6, 9, 10, 14 };
            var nega = new[] { new[] { 5, 6 }, new[] { 8, 14 }, new[] { 10 }, new[] { 8, 10, 14 }, new[] { 4 } };

            var result = NegativeIntersectArrays.Data(baseA, nega);
            Assert.Single(result);
            Assert.Equal(9, result[0]);
        }

        [Theory, MemberData(nameof(Data))]
        public void PeopleAreDumb(Labeled<Implementation> NegativeIntersectArrays)
        {
            var baseA = new int[0];
            var nega = new[] { new[] { 4, 5, 6, 8, 10, 14 } };
            var result = NegativeIntersectArrays.Data(baseA, nega);
            Assert.Empty(result);

            baseA = new[] { 4, 5, 6, 9, 10, 14 };
            nega = new[] { new int[0] };
            result = NegativeIntersectArrays.Data(baseA, nega);
            Assert.Equal(baseA.Length, result.Length);
        }

        [Theory, MemberData(nameof(Data))]
        public void OneExpectedOutput(Labeled<Implementation> NegativeIntersectArrays)
        {
            var baseA = new[] { 4, 5, 6, 9, 10, 14 };
            var nega = new[] { new[] { 4, 5, 6, 8, 10, 14 } };
            var result = NegativeIntersectArrays.Data(baseA, nega);
            Assert.Single(result);
            Assert.Equal(9, result[0]);
        }

        [Theory, MemberData(nameof(Data))]
        public void FirstAndLastSourceIncluded(Labeled<Implementation> NegativeIntersectArrays)
        {
            var baseA = new[] { 4, 5, 6, 9 };
            var nega = new[] { new[] { 4, 9, 12, 14 } };
            var result = NegativeIntersectArrays.Data(baseA, nega);
            Assert.Equal(2, result.Length);
            Assert.Equal(5, result[0]);

            baseA = new[] { 3, 5, 6, 9 };
            nega = new[] { new[] { 4, 5, 6, 9 } };
            result = NegativeIntersectArrays.Data(baseA, nega);
            Assert.Single(result);
            Assert.Equal(3, result[0]);

            baseA = new[] { 1, 9 };
            nega = new[] { new[] { 1, 10 } };
            result = NegativeIntersectArrays.Data(baseA, nega);
            Assert.Single(result);
            Assert.Equal(9, result[0]);
        }

        [Theory, MemberData(nameof(Data))]
        public void ExtendedRangesWorkCorrectly(Labeled<Implementation> NegativeIntersectArrays)
        {
            var baseA = new[] { 1, 9, 101 };
            var nega = new[] { new[] { 1, 10, 11, 14, 16, 17, 122 } };
            var result = NegativeIntersectArrays.Data(baseA, nega);
            Assert.Equal(2, result.Length);
            Assert.Equal(101, result[1]);

            baseA = new[] { 4, 5, 6, 9, 12, 14, 16, 19, 22, 24, 27, 30, 31, 35, 37, 201 };
            nega = new[] { new[] { 4, 9, 12, 14 } };
            result = NegativeIntersectArrays.Data(baseA, nega);
            Assert.Equal(baseA.Length - 4, result.Length);
            Assert.Equal(5, result[0]);

            baseA = new[] { 4, 5, 6, 9 };
            nega = new[] { new[] { 4, 9, 12, 14, 16, 19, 20, 22, 240, 241, 242 } };
            result = NegativeIntersectArrays.Data(baseA, nega);
            Assert.Equal(2, result.Length);
            Assert.Equal(5, result[0]);
        }

        [Theory, MemberData(nameof(Data))]
        public void NegativeIntersectionWorks(Labeled<Implementation> NegativeIntersectArrays)
        {

            int[] baseA;
            int[][] nega;
            int[] result;

            baseA = new[] { 4, 5, 6, 7, 8 };
            nega = new[] { new[] { 1, 2, 3, 5, 6, 7 } };
            result = NegativeIntersectArrays.Data(baseA, nega);
            Assert.Equal(2, result.Length); // 4,8
            Assert.Equal(4, result[0]);
        }

        [Theory, MemberData(nameof(Data))]
        public void Huge_FirstArray_DoesntExplodeStack(Labeled<Implementation> NegativeIntersectArrays)
        {

            int[] baseA;
            int[][] nega;
            int[] result;

            // at 1_000_000_000, all the joe implementations fail 
            // due to overflow in the calculation passed to Buffer.Blockcopy :p
            // But it chows over 10 gigs of ram to run the test so.. meh.

            baseA = new int[100_000]; // at 1M, the unsafe explodes with a stackoverflow
            Array.Copy(new[] { 4, 5, 6, 7, 8, 9 }, baseA, 5);

            nega = new[] { new[] { 0, 1, 2, 3, 5, 6, 7 } };
            result = NegativeIntersectArrays.Data(baseA, nega);
            Assert.Equal(99_997, result.Length); // 4,8
            Assert.Equal(4, result[0]);
        }
    }
}

