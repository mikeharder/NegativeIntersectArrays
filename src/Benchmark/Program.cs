using BenchmarkDotNet.Running;
using System;

namespace NegativeIntersectArrays
{
    class Program
    {
        static void Main(string[] args)
        {
#if DEBUG
            Console.WriteLine("Running Debug mode, no benchmark..");
            var c = new Algos();
            c.SH_NegativeIntersection();
#else
            var summary = BenchmarkRunner.Run<Algos>();
#endif
            Console.WriteLine("Done.");
            Console.Read();
        }
     }
}
