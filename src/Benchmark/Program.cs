using BenchmarkDotNet.Running;
using System;

namespace NegativeIntersectArrays
{
    static class Program
    {
        static void Main(string[] args)
        {
            AppDomain.CurrentDomain.FirstChanceException += CurrentDomain_FirstChanceException;

#if DEBUG
            Console.WriteLine("Running Debug mode, no benchmark..");
            var c = new Algos();
            c.SH_Unsafe_A();
#else
            var summary = BenchmarkRunner.Run<Algos>();
#endif
            Console.WriteLine("Done.");
            Console.Read();

        }

        private static void CurrentDomain_FirstChanceException(object sender, System.Runtime.ExceptionServices.FirstChanceExceptionEventArgs e)
        {
            // don't know how i missed this - perhaps because we are running 1.x it wasnt available?

            Console.WriteLine("FIRSTCHANCE: " + e.Exception.ToString());
            //Console.Read();
        }
    }
}
