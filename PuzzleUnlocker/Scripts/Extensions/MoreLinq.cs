using System;
using System.Collections.Generic;
using System.Linq;

namespace PuzzleUnlocker.Scripts.Extensions
{
    public static class MoreLinq
    {
        public static TSource MinBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> func) where TKey : IComparable
        {
            TSource minS = source.First();
            TKey min = func(minS);
            foreach (var s in source)
            {
                var result = func(s);
                if (min.CompareTo(result) > 0)
                {
                    min = result;
                    minS = s;
                }
            }

            return minS;
        }
        
        public static TSource MaxBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> func) where TKey : IComparable
        {
            TSource maxS = source.First();
            TKey max = func(maxS);
            foreach (var s in source)
            {
                var result = func(s);
                if (max.CompareTo(result) < 0)
                {
                    max = result;
                    maxS = s;
                }
            }

            return maxS;
        }
        
        public static int MinByIndex<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> func) where TKey : IComparable
        {
            TKey min = func(source.First());
            var ind = 0;
            var minInd = 0;
            foreach (var s in source)
            {
                var result = func(s);
                if (min.CompareTo(result) > 0)
                {
                    min = result;
                    minInd = ind;
                }

                ind++;
            }

            return minInd;
        }

        public static int[] GetIndexes<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> func)
        {
            List<int> list = new List<int>();
            var ind = 0;
            foreach (var s in source)
            {
                if (func(s))
                {
                    list.Add(ind);
                }

                ind++;
            }

            return list.ToArray();
        }
    }
}