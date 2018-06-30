using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MathNet.Numerics.LinearAlgebra;

namespace LSM_CS
{
    public class ModelAsianAvgFixedRate: ModelSimpleFixedRate
    {
        public ModelAsianAvgFixedRate(double rate,
                                  int seedValue,
                                  int trajectoriesNo,
                                  int trajectoryLength,
                                  double endTime,
                                  Vector<double> mu,
                                  Vector<double> sigma,
                                  Vector<double> x0,
                                  Matrix<double> correlationMatrix
                                  ):            
            base(rate, seedValue, trajectoriesNo, trajectoryLength, 
                endTime, mu, sigma, x0, correlationMatrix)    {}


        public override List<Matrix<double>> Trajectories()
        {
            if (!up_to_date_trajectories_)
            {
                Sample_ = TrajectoryGenerator.Sample(SeedValue,TrajectoriesNo,TrajectoryLength,EndTime,Mu,Sigma,X0,CorrelationMatrix);
                int roz = Sample_.Count;
                for ( int i=0; i < roz; ++i)
                {
                    Sample_.Add(MatrixExtensions.ColCumSumAvg(Sample_[i]));
                }
                
                up_to_date_trajectories_ = true;
            }
            return Sample_;
        }
    }
}
