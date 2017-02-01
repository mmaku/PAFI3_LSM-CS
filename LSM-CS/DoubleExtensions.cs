using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LSM_CS
{
    public static class DoubleExtensions
    {
        public static bool AreEqual(double a, double b, double tol = 1E-9)
        {
            return (Math.Abs(a - b) <= tol);
        }
    }
}
