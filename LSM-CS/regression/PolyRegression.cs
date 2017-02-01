using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearRegression;
using System.Diagnostics;

namespace LSM_CS
{
    public class PolyRegression : RegressionModel
    {
        private int degree_;

        public PolyRegression(int degree = 2)
        {
            Debug.Assert(degree > 0);
            degree_ = degree;
            CoefficientsCount = degree + 1;

        }

        public int Degree()
        {
            return degree_;
        }

        public override Matrix<double> PrepareData(List<Vector<double>> regressors)
        {
            Debug.Assert(regressors.Count > 0);

            Matrix<double> data = CreateMatrix.DenseOfColumnVectors<double>(regressors[0]); // do pokminienia - czy w data trzymamy tylko to, co zostało użte do budowy modelu? Czy wszystko?
            for(int i = 2; i <= degree_; i++)
            {
                data = data.Append(CreateMatrix.DenseOfColumnVectors<double>(regressors[0].PointwisePower(i)));
            }

            data = data.Append(CreateMatrix.Dense<double>(regressors[0].Count, 1, 1.0)); // tak średnio bym powiedział, może by to przeżucić na początek? Albo wielomian stopniować od największego do najmniejszego?

            return data;
        }
    }
}
