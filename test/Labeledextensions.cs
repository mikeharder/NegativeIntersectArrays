using System;
using System.Collections;
using System.Collections.Generic;
using Xunit;
using System.Linq;

namespace Tests
{

    public static class Labeledextensions
    {
        public static Labeled<T> Labeled<T>
                (this T source, string label) => new Labeled<T>(source, label);
    }
}
//    var items = ImmutableList.Create(
//              CreatToolPathItem(x: 0, y: 0, z: 0, c: 0, b: 0, data: "0".dumb(), railPosition: 0, curve: curve),
//              CreatToolPathItem(x: 0, y: 10, z: 0, c: 0, b: 0, data: "3".dumb(), railPosition: 9, curve: curve)
//         );

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

