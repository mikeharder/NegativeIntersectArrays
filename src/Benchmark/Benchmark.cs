using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Attributes.Exporters;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace NegativeIntersectArrays
{
    [HtmlExporter]
    [MarkdownExporter]
    public class Algos
    {
        int[] baseA;
        int[] baseB;
        int[] baseC;
        int[] baseD;
        int[][] nega;
        int[][] negb;
        const int randomSeed = 51292;

        public Algos()
        {
            Random rr = new Random(randomSeed);
            baseA = new int[rr.Next(0, 300000)];
            baseB = new int[rr.Next(0, 100000)];
            baseC = new int[rr.Next(0, 100000)];
            baseD = new int[rr.Next(0, 100000)];
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
            fill(baseC);
            fill(baseD);
            nega = new[] { baseB };
            negb = new[] { baseB, baseC, baseD };
        }

        [Benchmark]
        public void SH_NegativeIntersection()
        {
            var k = SH_Implementation.NegativeIntersectArrays(baseA, nega);
        }

        [Benchmark]
        public void Mike_NegativeIntersection()
        {
            var k = Mike_Implementation.NegativeIntersectArrays(baseA, nega);
        }

        //[Benchmark]
        //public void SH_NegativeIntersectionB()
        //{
        //    var k = SH_Implementation.NegativeIntersectArrays(baseA, negb);
        //}

        //[Benchmark]
        //public void Mike_NegativeIntersectionB()
        //{
        //    var k = Mike_Implementation.NegativeIntersectArrays(baseA, negb);
        //}
    }
}
