using System;
using System.Collections.Generic;
using Xunit;

namespace Tests
{

    //using NegativeIntersectArrays = NegativeIntersectArrays.SH_Implementation.NegativeIntersectArrays;
    public class NegativeIntersectionTests
    {
        Func<int[], IReadOnlyList<int[]>, int[]> NegativeIntersectArrays;
        public NegativeIntersectionTests()
        {
            //TODO: Verify other implementations by pointing to the static function here:
            this.NegativeIntersectArrays = new Func<int[], IReadOnlyList<int[]>, int[]>(
                global::NegativeIntersectArrays.SH_Implementation.NegativeIntersectArrays);
        }

        [Fact]
        public void FullNegativeIntersectionRemovesAll()
        {
            var baseA = new[] { 1, 2, 3 };
            var nega = new[] { new[] { 1, 2, 3 } };
            var result = NegativeIntersectArrays(baseA, nega);
            Assert.Empty(result);
        }

        [Fact]
        public void ExtendedRangeDoesNothing()
        {
            var baseA = new[] { 1, 2, 3 };
            var nega = new[] { new[] { 4, 5, 6 } };
            var result = NegativeIntersectArrays(baseA, nega);
            Assert.Equal(3, result.Length);
            Assert.Equal(3, result[2]);
        }


        [Fact]
        public void MultipleRangesDontCauseExplosions()
        {
            var baseA = new[] { 4, 5, 6, 9, 10, 14 };
            var nega = new[] { new[] { 5, 6 }, new[] { 8, 14 }, new[] { 10 }, new[] { 8, 10, 14 }, new[] { 4 } };
            var result = NegativeIntersectArrays(baseA, nega);
            Assert.Single(result);
            Assert.Equal(9, result[0]);
        }

        [Fact]
        public void PeopleAreDumb()
        {
            var baseA = new int[0];
            var nega = new[] { new[] { 4, 5, 6, 8, 10, 14 } };
            var result = NegativeIntersectArrays(baseA, nega);
            Assert.Empty(result);

            baseA = new[] { 4, 5, 6, 9, 10, 14 };
            nega = new[] { new int[0] };
            result = NegativeIntersectArrays(baseA, nega);
            Assert.Equal(baseA.Length, result.Length);
        }

        [Fact]
        public void OneExpectedOutput()
        {
            var baseA = new[] { 4, 5, 6, 9, 10, 14 };
            var nega = new[] { new[] { 4, 5, 6, 8, 10, 14 } };
            var result = NegativeIntersectArrays(baseA, nega);
            Assert.Single(result);
            Assert.Equal(9, result[0]);
        }

        [Fact]
        public void FirstAndLastSourceIncluded()
        {
            var baseA = new[] { 4, 5, 6, 9 };
            var nega = new[] { new[] { 4, 9, 12, 14 } };
            var result = NegativeIntersectArrays(baseA, nega);
            Assert.Equal(2, result.Length);
            Assert.Equal(5, result[0]);

            baseA = new[] { 3, 5, 6, 9 };
            nega = new[] { new[] { 4, 5, 6, 9 } };
            result = NegativeIntersectArrays(baseA, nega);
            Assert.Single(result);
            Assert.Equal(3, result[0]);

            baseA = new[] { 1, 9 };
            nega = new[] { new[] { 1, 10 } };
            result = NegativeIntersectArrays(baseA, nega);
            Assert.Single(result);
            Assert.Equal(9, result[0]);
        }

        [Fact]
        public void ExtendedRangesWorkCorrectly()
        {
            var baseA = new[] { 1, 9, 101 };
            var nega = new[] { new[] { 1, 10, 11, 14, 16, 17, 122 } };
            var result = NegativeIntersectArrays(baseA, nega);
            Assert.Equal(2, result.Length);
            Assert.Equal(101, result[1]);

            baseA = new[] { 4, 5, 6, 9, 12, 14, 16, 19, 22, 24, 27, 30, 31, 35, 37, 201 };
            nega = new[] { new[] { 4, 9, 12, 14 } };
            result = NegativeIntersectArrays(baseA, nega);
            Assert.Equal(baseA.Length - 4, result.Length);
            Assert.Equal(5, result[0]);

            baseA = new[] { 4, 5, 6, 9 };
            nega = new[] { new[] { 4, 9, 12, 14, 16, 19, 20, 22, 240, 241, 242 } };
            result = NegativeIntersectArrays(baseA, nega);
            Assert.Equal(2, result.Length);
            Assert.Equal(5, result[0]);
        }

        [Fact]
        public void NegativeIntersectionWorks()
        {

            int[] baseA;
            int[][] nega;
            int[] result;

            baseA = new[] { 4, 5, 6, 7, 8 };
            nega = new[] { new[] { 1, 2, 3, 5, 6, 7 } };
            result = NegativeIntersectArrays(baseA, nega);
            Assert.Equal(2, result.Length); // 4,8
            Assert.Equal(4, result[0]);
        }
    }
}

