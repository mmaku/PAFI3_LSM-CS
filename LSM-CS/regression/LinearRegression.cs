using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearRegression;
using System.Diagnostics;

namespace LSM_CS
{
    /// <summary>
    ///  Class for linear regresion with intercept creation and calculation
    /// </summary>
    public class LinearRegression : RegressionModel
    {
        private bool isIntercept_;

        public LinearRegression(bool isIntercept = true, int assetCount = 1)
        {
            Debug.Assert(assetCount > 0);
            isIntercept_ = isIntercept;
            CoefficientsCount = assetCount;

            if (isIntercept_)
            {
                CoefficientsCount++;
            }
        }

        public bool IsIntercept()
        {
            return isIntercept_;
        }


        public override Matrix<double> PrepareData(List<Vector<double>> regressors)
        {
            Debug.Assert(regressors.Count > 0);
            for (int i = 1; i < regressors.Count; i++)
            {
                Debug.Assert(regressors[i].Count == regressors[0].Count);
            }

            int foo = CoefficientsCount;
            if (isIntercept_)
            {
                foo--;
            }
                                  
            Matrix<double> bar = CreateMatrix.DenseOfColumnVectors<double>(regressors);
            Matrix<double> data = bar.SubMatrix(0, bar.RowCount, 0, foo);

            if (isIntercept_)
            {
                data = data.Append(CreateMatrix.Dense<double>(bar.RowCount, 1, 1.0));
            }

            return data;
        }
    }
}