using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MathNet.Numerics.LinearAlgebra;

namespace LSM_CS
{
    /// <summary>
    /// Model will remember all the entry parameters for simulating trajectories (using TrajectoryGenerator),
    /// and computing discount factors, and hold it all together, to ensure consistency.
    /// It has methods to return Trajectories(), Times() or Discounts().
    /// Repective underlying objects will only be computed if the method is called out, then stored in Model,
    /// to be able to get them 
    /// for passing these as arguments to other objects.
    /// /// </summary>
    public abstract class Model
    {
        protected int SeedValue;
        public int TrajectoriesNo { get; protected set; }
        public int TrajectoryLength { get; protected set; }
        protected double EndTime;
        protected Vector<double> Mu;
        protected Vector<double> Sigma;
        public Vector<double> X0 { get; protected set; }
        protected Matrix<double> CorrelationMatrix;

        
        /// </summary>
        /// Function "DiscountFactor" returns a double discount factor from moment Times[TimeFrom] to Times[TimeTo] 
        /// for Trajectory[TrajectoryNumber] (as numeraire may be correlated with active X).
        /// It accesses discount factors stored in DiscountVector
        /// WARNING: it doesn't validate the parameters
        /// </summary>

        abstract public double DiscountFactor(int TrajectoryNumber, int TimeTo, int TimeFrom);
        /// </summary>
        /// Function "DiscountToZero" returns a vector of doubles - discount factor from moments StoppingTime to 0
        /// for Trajectory[TrajectoryNumber].   It accesses discount factors stored in DiscountVector
        /// WARNING: it doesn't validate the parameters
        /// </summary>
        abstract public Vector<double> DiscountToZero(List<int> StoppingTime);

        /// </summary>
        /// Function "Sample" returns a list of matrix of doubles (dimensions
        /// length_of_mu*[trajectoriesLength*trajectoriesNo]) of trajectories for moments 0:endTime
        /// It uses method Sample() from TrajectoryGenerator, and stores the result for later access.
        /// 
        /// WARNING: it doesn't validate the parameters
        /// </summary>
        abstract public List<Matrix<double>> Trajectories();
        /// </summary>
        /// Function "Times" returns a vector of moments 0:endTime (dimensions trajectoriesLength*1) 
        /// It uses method Times() from TrajectoryGenerator, and stores the result for later access.
        /// 
        /// WARNING: it doesn't validate the parameters
        /// </summary>
        abstract public Vector<double> Times();


    }
}