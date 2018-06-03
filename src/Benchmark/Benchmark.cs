﻿using BenchmarkDotNet.Attributes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace NegativeIntersectArrays
{
    public class Algos
    {
        int[] baseA;
        int[] baseB;
        int[][] nega;
        const int randomSeed = 51292;

        public Algos()
        {
            Random rr = new Random(randomSeed);
            baseA = new int[rr.Next(0, 300000)];
            baseB = new int[rr.Next(0, 100000)];
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
            nega = new[] { baseB };
        }

        [Benchmark]
        public void SH_NegativeIntersection()
        {
            var k = SH_Implementation.NegativeIntersectArrays(baseA, nega);
            //Console.WriteLine($"{swT} time for {repetitions} excluding intersections. Array Size b4: {baseALength} after: {k.Length}. Per Exec: {swT / repetitions}ms");
        }
    }
}