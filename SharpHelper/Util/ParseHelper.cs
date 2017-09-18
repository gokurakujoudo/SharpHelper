using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace SharpHelper.Util
{
    public static class ParseHelper
    {
        public static T To<T>(this object obj) => (T)obj;
        public static bool IsDefault<T>(this T value) => EqualityComparer<T>.Default.Equals(value, default(T));

        public static double ToDouble(this int num) => num + 0.0;
        public static IEnumerable<T> ToEnumerable<T>(this object o) => o.To<IEnumerable>().Cast<T>();
        public static IEnumerable<string> ToStrings(this object o) => o.ToEnumerable<object>().Select(obj => obj.ToString());

    }
}
