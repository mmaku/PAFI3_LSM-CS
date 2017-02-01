using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearRegression;
using System.Diagnostics;

namespace LSM_CS
{
    public class BivariableRegression : RegressionModel
    {
        private int degree1_;
        private int degree2_;
        private int crosprodDegree_;

        public BivariableRegression(int degree1 = 2, int degree2 = 2, int crosprodDegree = 0)
        {
            Debug.Assert(degree1 > 0);
            Debug.Assert(degree2 > 0);
            Debug.Assert(crosprodDegree >= 0);
            degree1_ = degree1;
            degree2_ = degree2;
            crosprodDegree_ = crosprodDegree;
            CoefficientsCount = degree1 + degree2 + crosprodDegree + 1;
        }

        public int[] Degrees()
        {
            return new int[3] {degree1_, degree2_, crosprodDegree_};
        }

        public override Matrix<double> PrepareData(List<Vector<double>> regressors)
        {
            Debug.Assert(regressors.Count > 1);

            Matrix<double> data = CreateMatrix.DenseOfColumnVectors<double>(regressors[0]); 
            for (int i = 2; i <= degree1_; i++)
            {
                data = data.Append(CreateMatrix.DenseOfColumnVectors<double>(regressors[0].PointwisePower(i)));
            }

            data.Append(CreateMatrix.DenseOfColumnVectors<double>(regressors[1])); 
            for (int i = 2; i <= degree2_; i++)
            {
                 data = data.Append(CreateMatrix.DenseOfColumnVectors<double>(regressors[1].PointwisePower(i)));
            }

            for (int i = 1; i <= crosprodDegree_; i++)
            {
                data.Append(CreateMatrix.DenseOfColumnVectors<double>(regressors[0].PointwisePower(i).PointwiseMultiply(regressors[0].PointwisePower(i))));
            }

            data = data.Append(CreateMatrix.Dense<double>(regressors[0].Count, 1, 1.0)); 

            return data;
        }
    }
}
