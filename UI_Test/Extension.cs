using System;
using System.Linq;

namespace UI_Test {
    static class Extension {
        public static T[] Populate<T>(this T[] array, T value){
            return array.Select(x => value).ToArray();
        }
    }
}
