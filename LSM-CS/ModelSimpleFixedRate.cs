using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MathNet.Numerics.LinearAlgebra;

namespace LSM_CS
{
    public class ModelSimpleFixedRate : Model
    {
        protected double Rate;
        protected Vector<double> Times_;
        protected List<Matrix<double>> Sample_;
        protected Vector<double> DiscountVector_;

        protected bool up_to_date_time_ = false;
        protected bool up_to_date_discounts_ = false;
        protected bool up_to_date_trajectories_ = false;

        public ModelSimpleFixedRate(double rate,
                                  int seedValue,
                                  int trajectoriesNo,
                                  int trajectoryLength,
                                  double endTime,
                                  Vector<double> mu,
                                  Vector<double> sigma,
                                  Vector<double> x0,
                                  Matrix<double> correlationMatrix
                                  )
        {
            Rate = rate;//initialize all arguments
            SeedValue = seedValue;
            TrajectoriesNo = trajectoriesNo;
            TrajectoryLength = trajectoryLength;//==endTime/dt
            EndTime = endTime;
            Mu = mu;
            Sigma = sigma;
            X0 = x0;
            CorrelationMatrix = correlationMatrix;
            up_to_date_time_ = false;
            up_to_date_discounts_ = false;
            up_to_date_trajectories_ = false;
            //end of initialization
        }

        public override double DiscountFactor(int TrajectoryNumber, int TimeTo, int TimeFrom)
        {
            if (!up_to_date_discounts_)                DiscountVector();
            if (TimeFrom - TimeTo >= 0) return DiscountVector_[TimeFrom - TimeTo];
            else return (1 / Math.Pow(1 + Rate, Times_[TimeFrom] - Times_[TimeTo]));
        }
        private void DiscountVector()
        {
            if (!up_to_date_discounts_)
            {
                if (!up_to_date_time_) Times();
                DiscountVector_ = CreateVector.Dense<double>(TrajectoryLength+1, 1);
                for (int i = 1; i < TrajectoryLength+1; ++i)
                {
                    DiscountVector_[i] = 1 / Math.Pow(1 + Rate, Times_[i-1]);
                }
                up_to_date_discounts_ = true;
            }  
        }
        public override Vector<double> DiscountToZero(List<int> StoppingTime)
        {
            if (!up_to_date_discounts_) DiscountVector();
            Vector<double> DiscountToZeroVector_ = CreateVector.Dense<double>(TrajectoriesNo);
            for (int i = 0; i < TrajectoriesNo; ++i)
            {
                DiscountToZeroVector_[i] = DiscountVector_[StoppingTime[i]];
            }

            return DiscountToZeroVector_;
        }

        public override Vector<double> Times()
        {
            if (!up_to_date_time_)
            {
                Times_ = TrajectoryGenerator.Times(EndTime, TrajectoryLength);
                up_to_date_time_ = true;
            }
            return (Times_);

        }
        public override List<Matrix<double>> Trajectories()
        {
            if (!up_to_date_trajectories_)
            {
                Sample_ = TrajectoryGenerator.Sample(SeedValue,TrajectoriesNo,TrajectoryLength,EndTime,Mu,Sigma,X0,CorrelationMatrix);
                up_to_date_trajectories_ = true;
            }
            return Sample_;
        }
    }
}
