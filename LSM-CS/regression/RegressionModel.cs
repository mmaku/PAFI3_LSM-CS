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
    ///  Class for regresion creation and calculation
    /// </summary>
    public abstract class RegressionModel
    {
        protected Matrix<double> data_;
        protected Vector<double> coefficients_;
        public int CoefficientsCount { get; protected set; }

        public RegressionModel()
        {

        }

        public Vector<double> Coefficients()
        {
            return coefficients_;
        }

        public Matrix<double> Data()
        {
            return data_;
        }


        public void BuildModel(List<Vector<double>> regressors, Vector<double> regressand) // być może do wywalenia z klasy bazowej
        {
            Debug.Assert(regressors.Count > 0);
            for (int i = 0; i < regressors.Count; i++)
            {
                Debug.Assert(regressors[i].Count == regressand.Count);
            }

            data_ = PrepareData(regressors);

            coefficients_ = MultipleRegression.NormalEquations(data_, regressand);
        }

        public void BuildModel(Vector<double> regressors, Vector<double> regressand)
        {
            Debug.Assert(regressors.Count == regressand.Count);

            List<Vector<double>> regressorsList = new List<Vector<double>>();
            regressorsList.Add(regressors);
            data_ = PrepareData(regressorsList);

            coefficients_ = MultipleRegression.NormalEquations(data_, regressand);
        }

        abstract public Matrix<double> PrepareData(List<Vector<double>> regressors);

        public Vector<double> Predict(List<Vector<double>> regressors)
        {
            return PrepareData(regressors).Multiply(coefficients_);
        }


        public Vector<double> Predict()
        {
            return data_.Multiply(coefficients_);
        }

    }
}