using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpHelper.Util {
    public static class MathHelper {
        public static double Pow(this double x, double y = 2) => Math.Pow(x, y);
        internal static double Div(this double d1, double d2) => d2 == 0 ? 0 : d1 / d2;

        internal static T[,] ToColumn<T>(this T[] input)
        {
            var l = input.Length;
            var output = new T[l, 1];
            for (var i = 0; i < l; i++)
                output[i, 0] = input[i];
            return output;
        }

        public const double TOL = 1E-8;

        public static double Percentile(double[] sequence, double percentile)
        {
            Array.Sort(sequence);
            var length = sequence.Length;
            var n = (length - 1) * percentile + 1;
            if (n == 1d) return sequence[0];
            if (n == length) return sequence[length - 1];
            var k = (int)n;
            var d = n - k;
            return sequence[k - 1] + d * (sequence[k] - sequence[k - 1]);
        }
    }
}
