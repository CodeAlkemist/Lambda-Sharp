// In short apache 2 license, check LICENSE file for the legalese

using System;
using System.Collections.Generic;

namespace Lambda
{
    public static partial class Extensions
    {
        #region Subsets

        public static List<T> Subset<T>(this List<T> l, int sindex, int eindex, int interval)
        {
            if (sindex < l.Count && eindex > l.Count) throw new IndexOutOfRangeException($"Tryed to subset {l.ToString()} out of range, tried {sindex} and {eindex}, lenght was {l.Count}");
            List<T> sub = new List<T>();
            if (eindex < 0)
                eindex = l.Count;
            for (int i = sindex; i < eindex; i++)
            {
                sub.Add(l[i]);
            }
            return sub;
        }

        public static T[] Subset<T>(this T[] a, int sindex, int eindex, int interval)
        {
            if (sindex < a.Length && eindex > a.Length) throw new IndexOutOfRangeException($"Tryed to subset {a.ToString()} out of range, tried {sindex} and {eindex}, lenght was {a.Length}");
            List<T> sub = new List<T>();
            if (eindex < 0)
                eindex = a.Length;
            for (int i = sindex; i < eindex; i += interval)
            {
                sub.Add(a[i]);
            }
            return sub.ToArray();
        }

        public static T[] Subset<T>(this T[] a, int sindex, int eindex) => Subset(a, sindex, eindex, 1);

        public static List<T> Subset<T>(this List<T> l, int sindex, int eindex) => Subset(l, sindex, eindex, 1);

        public static T[] Subset<T>(this T[] a, int interval) => Subset(a, 0, -1, interval);

        #endregion Subsets

        #region StringConv

        public static string String(this int[] i) => System.String.Join("", new List<int>(i).ConvertAll(n => n.ToString()));

        public static string String(this int[] i, string separator) => System.String.Join(separator, new List<int>(i).ConvertAll(n => n.ToString()));

        public static string String(this long[] i) => System.String.Join("", new List<long>(i).ConvertAll(n => n.ToString()));

        public static string String(this long[] i, string separator) => System.String.Join(separator, new List<long>(i).ConvertAll(n => n.ToString()));

        #endregion StringConv

        public static T[] Blend<T>(this T[] a, T[] b, int offSet, int interval)
        {
            var work = new List<T>(a.Length);
            work.AddRange(a);
            for (int i = offSet, i2 = 0; i < a.Length; i += interval, i2++)
            {
                work[i] = b[i2];
            }
            return work.ToArray();
        }
    }
}