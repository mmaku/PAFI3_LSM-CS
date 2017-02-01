using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.Random;
using MathNet.Numerics.Distributions;


namespace LSM_CS
{
    
    
    /// <summary>
    ///  methods for trajectory generation
    /// </summary>
    public static class TrajectoryGenerator
    {

        
        
        /// <summary>
        /// Returns a matrix of cummulatively summed columns of a matrix
        /// </summary>
        /// <param name="a">Matrix to be summed</param>
        /// <returns>Matrix of same dimensions as a</returns>
        



        /// <summary>
        /// Return list of matrices of length correlationMatrix.RowCount,
        /// vector of [i,j]th elements of result comes from multivariate normal dist,
        /// with mean = 0, and sigma = correlationMatrix
        /// </summary>
        /// <param name="seedValue">Seed value</param>
        /// <param name="trajectoriesNo">Number of columns of result matrices</param>
        /// <param name="trajectoryLength">Number of rows of result matrices</param>
        /// <param name="correlationMatrix">Positive definite symmetric matrix, with 1s on diagonal</param>
        /// <returns></returns>
        private static List<Matrix<double>> NormalTrajectories (int seedValue,
                                                                int trajectoriesNo,
                                                                int trajectoryLength,
                                                                Matrix<double> correlationMatrix)
        {
            Random generator = new MersenneTwister(seedValue);
            Matrix<double> sample = CreateMatrix.Dense<double>(correlationMatrix.RowCount, 1);
            Matrix<double> mu = CreateMatrix.Dense<double>(correlationMatrix.RowCount,1, 0);
            Matrix<double> x = CreateMatrix.Dense<double>(1,1,1);
            MatrixNormal distribution = new MatrixNormal(mu, correlationMatrix, x, generator);

            List<Matrix<double>> result = new List<Matrix<double>>();

            for (int i = 0; i < correlationMatrix.RowCount; i++)
            {
                result.Add (CreateMatrix.Dense<double>(trajectoryLength, trajectoriesNo, 0));
            }



            for (int i = 0; i < trajectoryLength; i++)
            {
                for (int j = 0; j < trajectoriesNo; j++)
                {
                    sample = distribution.Sample();
                    for (int k = 0; k < correlationMatrix.RowCount; k++)
                    {
                        result[k][i, j] = sample[k,0];
                    }
                }
            }

            return result;
        }



        /// <summary>
        /// Converts matrix containing matrix of iid samples of normal distribution to 
        /// a matrix of GBMs (i-th trajectory of GBM in i-th column)
        /// </summary>
        /// <param name="mu">Drift</param>
        /// <param name="sigma">Volatility</param>
        /// <param name="times">Sorted vector of times of evaluation</param>
        /// <param name="x0">initial value of GBM</param>
        /// <param name="NormalMatrix"></param>
        /// <returns></returns>
        private static Matrix<double> NormalToGBM (double mu,
                                                    double sigma,
                                                    Vector<double> times,
                                                    double x0,
                                                    Matrix<double> NormalMatrix)
        {
            NormalMatrix = MatrixExtensions.ColCumSum(NormalMatrix);
            double dt = times[0];
            for (int i = 0; i < NormalMatrix.RowCount; i++)
            {
                for (int j = 0; j < NormalMatrix.ColumnCount; j++)
                {
                    NormalMatrix[i, j] = x0 * Math.Exp((mu - Math.Pow(sigma, 2) / 2) * times[i] + sigma * NormalMatrix[i, j] * Math.Sqrt(dt));
                }
            }
            return NormalMatrix;
        }



        /// <summary>
        /// Returns a list of matrices (length same as length of mu), containing correlated GBMs with specified parameters. WARNING:
        /// it doesn't validate the parameters
        /// </summary>
        /// <param name="seedValue">Seed value for generator</param>
        /// <param name="trajectoriesNo">Number of trajectories to generate</param>
        /// <param name="beginTime">Starting time (usually 0)</param>
        /// <param name="endTime">End time</param>
        /// <param name="mu">Vector of drifts</param>
        /// <param name="sigma">Vector of volatilites</param>
        /// <param name="x0">Vector of initial values</param>
        /// <param name="correlationMatrix">Correlation matrix</param>
        /// <returns></returns>
        public static List<Matrix<double>> Sample (int seedValue,
                                                   int trajectoriesNo,
                                                   int trajectoryLength,
                                                   double endTime,
                                                   Vector<double> mu,
                                                   Vector<double> sigma,
                                                   Vector<double> x0,
                                                   Matrix<double> correlationMatrix
                                                   )
        {
            try
            {
                ValidateNumeric(trajectoriesNo, trajectoryLength, endTime);
                ValidateDimensions(mu, sigma, x0, correlationMatrix);
                ValidateCorrrelationMatrix(correlationMatrix);
            }
            catch (Exception e)
            {
                throw e;
            }
            Vector<double> times = Times(endTime, trajectoryLength);
            List<Matrix<double>> result = NormalTrajectories(seedValue, trajectoriesNo, trajectoryLength, correlationMatrix);
            
            for (int i = 0; i < result.Count; i++)
            {
                result[i] = NormalToGBM(mu[i], sigma[i], times, x0[i], result[i]);
            }
            return result;

        }

        private static void ValidateNumeric (int trajectoriesNo, int trajectoryLength, double endTime)
        {
            if (trajectoriesNo <= 0)
            {
                throw new ArgumentException("Number of trajectories needs to be a positive integer");
            }
            if (trajectoryLength <= 0)
            {
                throw new ArgumentException("Length of trajectory needs to be a positive integer");
            }
            if (endTime <= 0 )
            {
                throw new ArgumentException("endTime needs to be a positive");
            }
        }

        private static void ValidateDimensions (Vector<double> mu,
                                                   Vector<double> sigma,
                                                   Vector<double> x0,
                                                   Matrix<double> correlationMatrix)
        {
            int d = mu.Count;
            if (Math.Pow(sigma.Count-d, 2) + Math.Pow(x0.Count - d, 2) +
                Math.Pow(correlationMatrix.RowCount - d, 2) + Math.Pow(correlationMatrix.ColumnCount - d, 2) > 0)
            {
                throw new ArgumentException("Dimensions don't match");
            }

        }

        private static void ValidateCorrrelationMatrix (Matrix<double> correlationMatrix)
        {
            Vector < double > diag = correlationMatrix.Diagonal();
            for (int i = 0; i < diag.Count; i++)
            {
                if (diag[i] != 1)
                {
                    throw new ArgumentException("Correlation matrix needs to have 1s on diagonal");
                }
            }
            
            for (int i = 0; i < correlationMatrix.RowCount; i++)
            {
                for (int j = 0; j < correlationMatrix.ColumnCount; j ++)
                {
                    if (i != j)
                    {
                        if (!(correlationMatrix[i, j] < 1 && correlationMatrix[i, j] > -1))
                        {
                            throw new ArgumentException("Correlations need to be between -1 and 1 (exclusive outside of diagonal)");
                        }
                    }
                    
                }
            }

            Matrix<double> mu_test = CreateMatrix.Dense<double>(correlationMatrix.RowCount, 1, 0);
            Matrix<double> x = CreateMatrix.Dense<double>(1, 1, 1);

            if (!MatrixNormal.IsValidParameterSet(mu_test, correlationMatrix, x))
            {
                throw new ArgumentException("Correlation matrix needs to bo symmetric and positive definite");
            }



            
        }
        public static Vector<double> Times (double endTime, int trajectoryLength)
        {
            double dt = endTime / trajectoryLength;
            Vector<double> times = CreateVector.Dense<double>(trajectoryLength, dt);


            for (int i = 1; i < trajectoryLength; i++)
            {
                times[i] = times[i - 1] + dt;
            }
            return times;
        }




        
        
    }
}
