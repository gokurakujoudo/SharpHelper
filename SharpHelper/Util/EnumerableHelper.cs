using System;
using System.Collections.Generic;
using System.Linq;

namespace SharpHelper.Util
{
    public static class EnumerableHelper
    {
        public static IOrderedEnumerable<T> Ascending<T>(this IEnumerable<T> list)
            where T : IComparable<T> => list.OrderBy(t => t);

        public static IOrderedEnumerable<T> Descending<T>(this IEnumerable<T> list)
            where T : IComparable<T> => list.OrderByDescending(t => t);

        public static bool ContainsAll<T>(this IEnumerable<T> container, IEnumerable<T> check) =>
            check.All(container.Contains);

        public static bool UnorderEqual<T>(this IEnumerable<T> ie1, IEnumerable<T> ie2)
        {
            var enum1 = ie1 as IList<T> ?? ie1.ToList();
            var enum2 = ie2 as IList<T> ?? ie2.ToList();
            return enum1.ContainsAll(enum2) && enum2.ContainsAll(enum1);
        }

        public static string JoinStr<T>(this IEnumerable<T> ie, string seprator = ", ") => string.Join(seprator, ie);

        public static IEnumerable<T> SubByIndex<T>(this IEnumerable<T> ie, int start = 0, int end = -1)
        {
            var array = ie as T[] ?? ie.ToArray();
            var len = array.Length;
            if (start < 0) start += len;
            if (end < 0) end += len;
            return array.Where((t, i) => i >= start && i <= end).ToArray();
        }

        public static IEnumerable<T> SubByLen<T>(this IEnumerable<T> ie, int start = 0, int num = -1)
        {
            var array = ie as T[] ?? ie.ToArray();
            if (start < 0) start += array.Length;
            return num < 0
                ? array.Where((t, i) => i >= start).ToArray()
                : array.Where((t, i) => i >= start && i < start + num);
        }

        public static IEnumerable<T> Slice<T>(this IEnumerable<T> ie, IEnumerable<int> index) => ie.Where((t, i) => index.Contains(i));

        public static Dictionary<T, int> ValueCounts<T>(this IEnumerable<T> ie) {
            var dict = new Dictionary<T, int>();
            foreach (var t in ie)
                if (dict.ContainsKey(t)) dict[t] += 1;
                else dict[t] = 1;
            return dict;
        }

        public static T MaxItem<T, TComp>(this IEnumerable<T> ie, Func<T, TComp> func)where TComp:IComparable<TComp> {
            var list = ie as IList<T> ?? ie.ToList();
            if (!list.Any()) throw new ArgumentException("IE is empty");
            if (list.Count == 1) return list.First();
            var idx = 0;
            var v = func(list.First());
            foreach (var item in list) {
                var nv = func(item);
                if (nv.CompareTo(v) <= 0) continue;
                v = nv;
                idx++;
            }
            return list[idx];
        }

        public static T MinItem<T, TComp>(this IEnumerable<T> ie, Func<T, TComp> func) where TComp : IComparable<TComp>
        {
            var list = ie as IList<T> ?? ie.ToList();
            if (!list.Any()) throw new ArgumentException("IE is empty");
            if (list.Count == 1) return list.First();
            var idx = 0;
            var v = func(list.First());
            foreach (var item in list)
            {
                var nv = func(item);
                if (nv.CompareTo(v) >= 0) continue;
                v = nv;
                idx++;
            }
            return list[idx];
        }
    }
}
