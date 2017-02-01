using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MathNet.Numerics.LinearAlgebra;

namespace LSM_CS
{
    public class AmericanPut : Option
    {
        public double Expiry;
        public double Strike;
        public AmericanPut(double expiry, double strike)
        {
            Expiry = expiry;
            Strike = strike;
        }
        public override Matrix<double> Payoff(List<Matrix<double>> trajectories, Vector<double> times)
        {
            Matrix<double> result = CreateMatrix.Dense<double>(times.Count, trajectories[0].ColumnCount);
            int i = 0;
            while (i < times.Count && times[i] <= Expiry)
            {
                for (int j = 0; j < trajectories[0].ColumnCount; j++)
                {
                    result[i, j] = Math.Max(0,  Strike - trajectories[0][i, j]);
                }
                i++;
            }
            return result;
        }
    }
}
