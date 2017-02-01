using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using la = MathNet.Numerics.LinearAlgebra;
using LSM_CS;


namespace LSMTESTS
{
    [TestClass]
    public class PayoffTest
    {



        [TestMethod]
        public void EuropeanCallTest()
        {
            la.Vector<double> times = la.CreateVector.Dense<double>(3, 0);
            la.Matrix<double> trajectories = la.CreateMatrix.Dense<double>(3, 3, 0);
            la.Matrix<double> expectedPayoff = la.CreateMatrix.Dense<double>(3, 3, 0);

            List<la.Matrix<double>> traj = new List<la.Matrix<double>>();

            expectedPayoff[0, 0] = 0;
            expectedPayoff[0, 1] = 0;
            expectedPayoff[0, 2] = 0;
            expectedPayoff[1, 0] = 0;
            expectedPayoff[1, 1] = 0;
            expectedPayoff[1, 2] = 0;
            expectedPayoff[2, 0] = 0;
            expectedPayoff[2, 1] = 1;
            expectedPayoff[2, 2] = 2;

            EuropeanCall option = new EuropeanCall(1, 5);
            times[1] = 0.5;
            times[2] = 1;

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    trajectories[i, j] = 3 + i + j;
                }
            }

            traj.Add(trajectories);

            Assert.IsTrue(MatrixExtensions.AreEqual(expectedPayoff, option.Payoff(traj, times)));


        }

        [TestMethod]
        public void AmericanCallTest()
        {
            la.Vector<double> times = la.CreateVector.Dense<double>(3, 0);
            la.Matrix<double> trajectories = la.CreateMatrix.Dense<double>(3, 3, 0);
            la.Matrix<double> expectedPayoff = la.CreateMatrix.Dense<double>(3, 3, 0);

            List<la.Matrix<double>> traj = new List<la.Matrix<double>>();

            expectedPayoff[0, 0] = 0;
            expectedPayoff[0, 1] = 0.5;
            expectedPayoff[0, 2] = 1.5;
            expectedPayoff[1, 0] = 0.5;
            expectedPayoff[1, 1] = 1.5;
            expectedPayoff[1, 2] = 2.5;
            expectedPayoff[2, 0] = 1.5;
            expectedPayoff[2, 1] = 2.5;
            expectedPayoff[2, 2] = 3.5;

            AmericanCall option = new AmericanCall(1, 5);
            times[1] = 0.5;
            times[2] = 1;

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    trajectories[i, j] = 4.5 + i + j;
                }
            }

            traj.Add(trajectories);

            Assert.IsTrue(MatrixExtensions.AreEqual(expectedPayoff, option.Payoff(traj, times)));


        }

        [TestMethod]
        public void AsianAmericanFloatingStrikeCallTest()
        {
            la.Vector<double> times = la.CreateVector.Dense<double>(3, 0);
            la.Matrix<double> trajectories = la.CreateMatrix.Dense<double>(3, 3, 0);
            la.Matrix<double> expectedPayoff = la.CreateMatrix.Dense<double>(3, 3, 0);

            List<la.Matrix<double>> traj = new List<la.Matrix<double>>();

            expectedPayoff[0, 0] = 0;
            expectedPayoff[0, 1] = 0;
            expectedPayoff[0, 2] = 0;
            expectedPayoff[1, 0] = 0.5;
            expectedPayoff[1, 1] = 0.5;
            expectedPayoff[1, 2] = 0.5;
            expectedPayoff[2, 0] = 1;
            expectedPayoff[2, 1] = 1;
            expectedPayoff[2, 2] = 1;

            AsianAmericanFloatingStrikeCall option = new AsianAmericanFloatingStrikeCall(1);
            times[1] = 0.5;
            times[2] = 1;

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    trajectories[i, j] = 4 + i + j;
                }
            }

            traj.Add(trajectories);

            Assert.IsTrue(MatrixExtensions.AreEqual(expectedPayoff, option.Payoff(traj, times)));


        }

    }
}
