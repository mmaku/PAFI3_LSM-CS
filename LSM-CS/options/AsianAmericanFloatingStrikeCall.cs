using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MathNet.Numerics.LinearAlgebra;

namespace LSM_CS
{
    public class AsianAmericanFloatingStrikeCall : Option
    {
        public AsianAmericanFloatingStrikeCall(double expiry)
        {
            Expiry = expiry;
        }
        public double Expiry;
        public override Matrix<double> Payoff(List<Matrix<double>> trajectories, Vector<double> times)
        {
            Matrix<double> result = CreateMatrix.Dense<double>(times.Count, trajectories[0].ColumnCount);
            Matrix<double> average = MatrixExtensions.ColCumSumAvg(trajectories[0]);
            int i = 0;
            while (i < times.Count && times[i] <= Expiry)
            {
                for (int j = 0; j < average.ColumnCount; j++)
                {
                    result[i, j] = Math.Max(trajectories[0][i, j] - average[i, j], 0);
                }
                i++;
            }
            return result;
        }
    }
}
