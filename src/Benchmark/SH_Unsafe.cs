using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace NegativeIntersectArrays
{
    public static class SH_Unsafe
    {
        public static System.Buffers.ArrayPool<int> arrayPool = System.Buffers.ArrayPool<int>.Create();

        private static readonly int[] EmptyResult = new int[0];

        public unsafe static int[] NegativeIntersectArrays(int[] readonlySource, IReadOnlyList<int[]> exceptions)
        {
            var sourceLength = readonlySource.Length;
            if (sourceLength == 0)
                return EmptyResult;

            // Note: I have no idea what the stack size is in .net (1 mb?), but this is probably going to
            // explode the stack if source is particularly large.
            // DONE: write test to confirm the theory and find the point at which that happens ( if at all ? )
            //int* localbuffer;// = stackalloc int[sourceLength];
            //// int* localbuffer = stackalloc int[sourceLength];
            //if (sourceLength < 15000)
            //{
            //    // why does the following line not compile, having predefined localbuffer as int*?
            //    //localbuffer = stackalloc int[sourceLength];
            //    //but assigning at declaration (this following line) DOES..
            //    //int* localbuffer = stackalloc int[sourceLength];

            //    localbuffer = (int*)Marshal.AllocHGlobal(sourceLength * sizeof(int));
            //}
            //else
            //{
            //    localbuffer = (int*)Marshal.AllocHGlobal(sourceLength * sizeof(int));
            //}

            var tmpArray = arrayPool.Rent(sourceLength);
            fixed (int* localbuffer = tmpArray)
            {
                Marshal.Copy(readonlySource, 0, (IntPtr)localbuffer, sourceLength);

                int lastWorkingLength = sourceLength;
                var maxSearch = localbuffer[sourceLength - 1];

                var i = -1;
                while (++i < exceptions.Count)
                {
                    var arr2Len = exceptions[i].Length;
                    var comparedArray = exceptions[i];

                    int baseArrayIndex = 0,
                        comparedArrayIndex = 0,
                        writeIndex = 0;

                    if (comparedArray.Length == 0 || comparedArray[0] > maxSearch)
                        continue; // this entire (SORTED!) range is out of the range of the first range, nothing will be excluded..

                    fixed (int* b = comparedArray)
                    {
                        while (baseArrayIndex < lastWorkingLength && comparedArrayIndex < arr2Len)
                        {
                            var baseVal = localbuffer[baseArrayIndex];
                            var compVal = b[comparedArrayIndex];
                            if (compVal < baseVal)
                            {
                                ++comparedArrayIndex;
                            }
                            else if (baseVal == compVal)
                            {
                                ++baseArrayIndex;
                                ++comparedArrayIndex;
                            }
                            else
                            {
                                localbuffer[writeIndex++] = baseVal;
                                ++baseArrayIndex;
                            }
                        }

                        if (baseArrayIndex < lastWorkingLength)
                        {
                            var source = localbuffer + baseArrayIndex;
                            var dest = localbuffer + writeIndex;
                            writeIndex += (lastWorkingLength - baseArrayIndex);
                            var bytesToCopy =
                                (lastWorkingLength * sizeof(int)) - (baseArrayIndex * sizeof(int));
                            Buffer.MemoryCopy(source, dest, lastWorkingLength * sizeof(int), bytesToCopy);
                        }
                        //while (baseArrayIndex < lastWorkingLength) // if the baseArray is longer than the array we have to except, we need to copy in the rest of the items
                        //    localbuffer[writeIndex++] = localbuffer[baseArrayIndex++];

                        lastWorkingLength = writeIndex;
                    }
                }
                var returnable = new int[lastWorkingLength];
                Marshal.Copy((IntPtr)localbuffer, returnable, 0, lastWorkingLength);
                arrayPool.Return(tmpArray); //Marshal.FreeHGlobal((IntPtr)localbuffer);
                return returnable;
            }
        }
    }
}
