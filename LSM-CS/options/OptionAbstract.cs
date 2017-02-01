using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MathNet.Numerics.LinearAlgebra;

namespace LSM_CS
{
    public abstract class Option
    {
        abstract public Matrix<double> Payoff(List<Matrix<double>> trajectories, Vector<double> times);
    }
}
