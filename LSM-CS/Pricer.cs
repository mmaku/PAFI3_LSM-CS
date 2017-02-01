using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MathNet.Numerics.LinearAlgebra;

namespace LSM_CS
{
    public class Pricer
    {
        public Matrix<double> RegressionCoefficients { get; private set; }
        public List<int> stoppingTime { private set; get; }
        public double Price { private set; get; }

        public Pricer (RegressionModel formula, Model market, Option c)
        {
            Matrix<double> payoff = c.Payoff(market.Trajectories(), market.Times());
            RegressionCoefficients = CreateMatrix.Dense<double>(market.TrajectoryLength-1, formula.CoefficientsCount);
            stoppingTime = new List<int>(market.TrajectoriesNo);

            Vector<double> chosenPayoff = CreateVector.Dense<double>(market.TrajectoriesNo, 0);


            for (int i = 0; i < market.TrajectoriesNo; i ++)
            {
                stoppingTime.Add (market.TrajectoryLength - 1);
            }

            for (int i = market.TrajectoryLength-2; i >= 0; i --)
            {
                List<int> positiveIndexes = new List<int>();
                List<double> discountedPayoffs = new List<double>();
                for (int j = 0; j < market.TrajectoriesNo; j++)
                {
                    if (payoff[i,j] > 0)
                    {
                        positiveIndexes.Add(j);
                        discountedPayoffs.Add(payoff[stoppingTime[j], j] * market.DiscountFactor(j, i, stoppingTime[j]));
                    }
                }
                if (positiveIndexes.Count>2)
                {
                    List<Vector<double>> regressors = PositiveTrajectoriesValues(positiveIndexes, market, i);
                    formula.BuildModel(regressors, CreateVector.DenseOfEnumerable(discountedPayoffs));
                    RegressionCoefficients.SetRow(i, formula.Coefficients());
                    Vector<double> continuationValue = formula.Predict();

                    for (int k = 0; k < positiveIndexes.Count; k++)
                    {
                        if (continuationValue[k] < payoff[i, positiveIndexes[k]])
                        {
                            stoppingTime[positiveIndexes[k]] = i;
                        }
                    }
                }
                

            }


            for (int i = 0; i < market.TrajectoriesNo; i ++)
            {
                chosenPayoff[i] = payoff[stoppingTime[i], i];
            }

            Price = market.DiscountToZero(stoppingTime).DotProduct(chosenPayoff) / market.TrajectoriesNo;
        }

        private List<Vector<double>> PositiveTrajectoriesValues (List<int> positiveIndexes, Model market, int time)
        {
            List<Matrix<double>> fullTrajectories = market.Trajectories();
            List<Vector<double>> result = new List<Vector<double>>(fullTrajectories.Count);


            for (int i = 0; i < fullTrajectories.Count; i++)
            {
                result.Add(CreateVector.Dense<double>(positiveIndexes.Count));
                for (int j = 0; j < positiveIndexes.Count; j++)
                {
                    result[i][j] = fullTrajectories[i][time, positiveIndexes[j]];
                }
            }
            

            return result;
        }

    }
}
