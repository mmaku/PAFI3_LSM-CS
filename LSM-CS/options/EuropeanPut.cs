using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MathNet.Numerics.LinearAlgebra;

namespace LSM_CS
{
    public class EuropeanPut : Option
    {
        public EuropeanPut(double expiry, double strike)
        {
            Expiry = expiry;
            Strike = strike;
        }
        public double Expiry;
        public double Strike;
        public override Matrix<double> Payoff(List<Matrix<double>> trajectories, Vector<double> times)
        {
            Matrix<double> result = CreateMatrix.Dense<double>(times.Count, trajectories[0].ColumnCount);
            for (int i = 0; i < times.Count; i++)
            {
                if (DoubleExtensions.AreEqual(times[i], Expiry))
                {
                    for (int j = 0; j < trajectories[0].ColumnCount; j++)
                    {
                        result[i, j] = Math.Max(0,  Strike - trajectories[0][i, j]);
                    }
                }
            }
            return result;
        }
    }
}
