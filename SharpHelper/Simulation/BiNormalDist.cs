using System;

namespace SharpHelper.Simulation {
    public static class BiNormalDist {

        public static double BiNormDist(double a, double b, double rho) {
            var x = new[] {0.24840615, 0.39233107, 0.21141819, 0.03324666, 0.00082485334};
            var y = new[] {0.10024215, 0.48281397, 1.0609498, 1.7797294, 2.6697604};
            var a1 = a / Math.Sqrt(2 * (1 - Math.Pow(rho, 2)));
            var b1 = b / Math.Sqrt(2 * (1 - Math.Pow(rho, 2)));
            if (a <= 0 && b <= 0 && rho <= 0) {
                var sum = 0D;
                for (var i = 0; i <= 4; i++) {
                    for (var j = 0; j <= 4; j++) {
                        var z1 = a1 * (2 * y[i] - a1);
                        var z2 = b1 * (2 * y[j] - b1);
                        var z3 = 2 * rho * (y[i] - a1) * (y[j] - b1);
                        sum += x[i] * x[j] * Math.Exp(z1 + z2 + z3);
                    }
                }
                return sum * Math.Sqrt(1 - Math.Pow(rho, 2)) / Math.PI;
            }
            if (a <= 0 && b >= 0 && rho >= 0)
                return NormalDist.NormDist(a) - BiNormDist(a, -b, -rho);
            if (a >= 0 && b <= 0 && rho >= 0)
                return NormalDist.NormDist(b) - BiNormDist(-a, b, -rho);
            if (a >= 0 && b >= 0 && rho <= 0) {
                var sum = NormalDist.NormDist(a) + NormalDist.NormDist(b);
                return sum - 1 + BiNormDist(-a, -b, rho);
            }
            if (a * b * rho > 0) {
                var rho1 = (rho * a - b) * Math.Sign(a) / Math.Sqrt(Math.Pow(a, 2) - 2 * rho * a * b + Math.Pow(b, 2));
                var rho2 = (rho * b - a) * Math.Sign(b) / Math.Sqrt(Math.Pow(a, 2) - 2 * rho * a * b + Math.Pow(b, 2));
                var delta = (1 - Math.Sign(a) * Math.Sign(b)) / 4D;
                return BiNormDist(a, 0, rho1) + BiNormDist(b, 0, rho2) - delta;
            }
            throw new ArgumentException(nameof(BiNormDist));
        }
    }
}
