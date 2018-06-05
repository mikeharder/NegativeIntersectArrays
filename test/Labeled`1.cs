using System;
using System.Collections;
using System.Collections.Generic;
using Xunit;
using System.Linq;

namespace Tests
{
    public class Labeled<T>
    {
        public Labeled(T data, string label)
        {
            Data = data;
            Label = label;
        }

        public T Data { get; }
        public string Label { get; }

        public override string ToString()
        {
            return Label;
        }
    }
}
//            var items = ImmutableList.Create(
//                      CreatToolPathItem(x: 0, y: 0, z: 0, c: 0, b: 0, data: "0".dumb(), railPosition: 0, curve: curve),
//                      CreatToolPathItem(x: 0, y: 10, z: 0, c: 0, b: 0, data: "3".dumb(), railPosition: 9, curve: curve)
//                 );

//    yield return  make(items, 10, "y:10" );
//}

//        {
//            var items = ImmutableList.Create(
//                      CreatToolPathItem(x: 0, y: 0, z: 0, c: 0, b: 0, data: "0".dumb(), railPosition: 0, curve: curve),
//                      CreatToolPathItem(x: 0, y: 0, z: 10, c: 0, b: 0, data: "3".dumb(), railPosition: 9, curve: curve)
//                 );

//yield return  make(items, 10, "z:10" );
//        }

//        {
//            var items = ImmutableList.Create(
//                      CreatToolPathItem(x: 10, y: 0, z: 0, c: 0, b: 0, data: "0".dumb(), railPosition: 0, curve: curve),
//                      CreatToolPathItem(x: 10, y: 0, z: 10, c: System.Math.PI * 2, b: 0, data: "3".dumb(), railPosition: 9, curve: curve)
//                 );

//yield return  make(items, 10 * 2 * Math.PI, "x:10, c:2Pi" );
//        }

//    }

