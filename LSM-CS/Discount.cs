using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MathNet.Numerics.LinearAlgebra;

namespace LSM_CS
{
    public abstract class Discount
    {
        //public Vector <double> times;

        /// <summary>
        /// Function "Discounts" returns a matrix of doubles (dimensions same as that of payoff) 
        /// of discount factors for moments specfied in vector times 
        /// WARNING: it doesn't validate the parameters
        /// </summary>

        abstract public Matrix <double> Discounts(List<Matrix<double>> trajectories, Vector<double> times);
    }
}