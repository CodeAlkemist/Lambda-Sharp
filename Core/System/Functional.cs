using System.Collections.Generic;
using static Lambda.Utilities;

namespace System
{
    public static class Functional
    {
        public static T[] Fold<T>(this T[] array, Func<T, T, T> callback, int start)
        {
            var tmp = new List<T>(array.Length);
            tmp.Add(array[0]);
            foreach (var item in Range(start + 1, array.Length - 1, 1))
            {
                tmp.Add(callback(tmp[item - 1], array[item]));
            }
            return tmp.ToArray();
        }
    }
}