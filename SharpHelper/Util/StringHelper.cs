using System;
using System.Linq;

namespace SharpHelper.Util
{
    public static class StringHelper
    {
        public static int Count(this string str, char ch) => str.Count(c => c == ch);

        public static string[] CutString(this string full, int cutIndex, int cutLen = 0)
        {
            if (cutIndex < 0) throw new ArgumentOutOfRangeException();
            var len = full.Length;
            if (cutIndex >= len) return new[]{full, string.Empty, string.Empty};
            var before = full.Substring(0, cutIndex);
            if (cutLen < 0) cutLen = len;
            return cutIndex + cutLen >= len
                ? new[] {before, full.Substring(cutIndex), string.Empty}
                : new[] {before, full.Substring(cutIndex, cutLen), full.Substring(cutIndex + cutLen)};
        }
    }
}
