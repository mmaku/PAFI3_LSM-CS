using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MathNet.Numerics.LinearAlgebra;

namespace LSM_CS
{
    public class FixedDiscount : Discount
    {
        public FixedDiscount(double rate)
        {
            Rate = rate;
        }
        public double Rate;
        /// <summary>
        /// Function "Discounts" returns a matrix of doubles (dimensions same as that of payoff) 
        /// of discount factors for moments specfied in vector times 
        /// WARNING: it doesn't validate the parameters
        /// </summary>

        public override Matrix<double> Discounts(List<Matrix<double>> trajectories, Vector<double> times)
        {
            Matrix<double> result = CreateMatrix.Dense<double>(times.Count, trajectories[0].ColumnCount);
            for (int i = 0; i < times.Count; i++)
            {
                for (int j = 0; j < trajectories[0].ColumnCount; j++)
                {
                    result[i, j] = 1 / Math.Pow(1 + Rate, times[i]); 
                }
            }
            return result;
        }

        ///point for discusion : Should it return just a vector of factors for times and one rate, or matrix
        ///with second dimension being rates (fixed) passed as argument to function?
        
    }
}
