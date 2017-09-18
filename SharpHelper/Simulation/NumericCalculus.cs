using SharpHelper.Util;

namespace SharpHelper.Simulation
{
    public static class NumericCalculus
    {
        public static double[] NuFirstOrderWithEnds(double[] xs, double[] ys) {
            var n = xs.Length;
            var returns = new double[n];
            returns[0] = (ys[1] - ys[0]).Div(xs[1] - xs[0]);
            returns[n - 1] = (ys[n - 1] - ys[n - 2]).Div(xs[n - 1] - xs[n - 2]);

            for (var i = 1; i < n - 1; i++)
                returns[i] = (ys[i + 1] - ys[i - 1]).Div(xs[i + 1] - xs[i - 1]);
            return returns;
        }

        public static double[] NuFirstOrder(double[] xs, double[] ys) {
            var n = xs.Length;
            var returns = new double[n - 2];
            for (var i = 1; i < n - 1; i++)
                returns[i - 1] = (ys[i + 1] - ys[i - 1]) .Div(xs[i + 1] - xs[i - 1]);
            return returns;
        }

        public static double[] NuFirstOrderPo(double[] xs, double[] ys)
        {
            var n = xs.Length;
            var returns = new double[n - 1];
            for (var i = 0; i < n - 1; i++)
                returns[i] = (ys[i + 1] - ys[i]).Div(xs[i + 1] - xs[i]);
            return returns;
        }

        public static double[] NuFirstOrderNe(double[] xs, double[] ys)
        {
            var n = xs.Length;
            var returns = new double[n - 1];
            for (var i = 1; i < n; i++)
                returns[i-1] = (ys[i] - ys[i - 1]) .Div (xs[i] - xs[i - 1]);
            return returns;
        }

        public static double[] NuDeltaRate(double[] xs) {
            var n = xs.Length;
            var returns = new double[n - 1];
            for (var i = 0; i < n - 1; i++)
                returns[i] = (xs[i + 1] - xs[i]).Div(xs[i]);
            return returns;
        }
    }
}
