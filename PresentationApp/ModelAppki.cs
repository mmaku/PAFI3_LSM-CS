using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LSM_CS;
using MathNet.Numerics.LinearAlgebra;

namespace PresentationApp
{
    public static class ModelAppki
    {
        public static Model market = new ModelSimpleFixedRate(0.05, 34, 100000, 50, 1, CreateVector.Dense<double>(1, 0.05),
            CreateVector.Dense<double>(1, 0.2), CreateVector.Dense<double>(1, 100), CreateMatrix.Dense<double>(1, 1, 1));
        public static Option c;
        public static RegressionModel formula = new PolyRegression();
        public static Pricer p;
    }
}
