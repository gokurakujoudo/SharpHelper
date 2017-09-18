using System;
using System.Collections.Generic;
using System.Linq;
using SharpHelper.Util;

namespace SharpHelper.Simulation
{
    public static class RandomHelper
    {
        private static readonly Random R = new Random();
        public static T RndItem<T>(this IEnumerable<T> ie) {
            var array = ie as T[] ?? ie.ToArray();
            return array[R.Next(array.Length)];
        }
        public static IEnumerable<T> RndItems<T>(this IEnumerable<T> ie, int num, bool canRepeat = true) {
            var array = ie as T[] ?? ie.ToArray();
            return array.Slice(num.RndInts(array.Length, canRepeat));
        }
        public static IEnumerable<int> RndInts(this int num, int max = 100, bool canRepeat = true) {
            if (canRepeat) return new bool[num].Select(x => R.Next(max));
            if (num > max) throw new ArgumentOutOfRangeException();
            var sequence = Enumerable.Range(0, max).ToArray();
            var output = new int[max];
            var end = max - 1;
            for (var i = 0; i < num; i++)
            {
                var x = R.Next(0, end + 1);
                output[i] = sequence[x];
                sequence[x] = sequence[end];
                end--;
            }
            return output;
        }
        public static IEnumerable<bool> RndBools(this int num) => new bool[num].Select(x => R.NextDouble() > 0.5);
        public static IEnumerable<T> Times<T>(this int num, Func<T> f) => new bool[num].Select(x => f());
        public static bool NextBool(this Random r) => r.NextDouble() > 0.5;
    }
}
