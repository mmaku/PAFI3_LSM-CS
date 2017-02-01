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
    public class ModelTest
    {
        int trajectoriesNo = 3;
        int trajectoryLength = 4;
        la.Vector<double> mu = la.CreateVector.Dense<double>(1, 0);
        la.Vector<double> sigma = la.CreateVector.Dense<double>(1, 1);
        la.Vector<double> x0 = la.CreateVector.Dense<double>(1, 100); 
            la.Matrix<double> correlationMatrix = la.CreateMatrix.Dense<double>(1, 1, 1);
        [TestMethod]
        public void SimpleFixedRateTest_TimesDisplay()
        {
           ModelSimpleFixedRate model = new ModelSimpleFixedRate(
               0.1,1, trajectoriesNo, trajectoryLength, 2,
                                  mu,
                                  sigma,
                                  x0,
                                  correlationMatrix
                                  );
            la.Vector<double>ExpectedTimes_= la.CreateVector.Dense<double>(4, 0);
            for (int i = 0; i < 4; ++i)
            {
                ExpectedTimes_[i]=0.5*(i+1);
            }

            Assert.AreEqual(ExpectedTimes_, model.Times());
        }

        [TestMethod]
        public void SimpleFixedRateTest_TrajectoriesDimensions()
        {
            ModelSimpleFixedRate model = new ModelSimpleFixedRate(
                0.1, 1, trajectoriesNo, trajectoryLength, 2,
                mu,      sigma,      x0,           correlationMatrix
                                  );
            List<la.Matrix<double> > ExpectedTrajectories_ = model.Trajectories();
            Assert.AreEqual(ExpectedTrajectories_, model.Trajectories());
            for (int i = 0; i < ExpectedTrajectories_.Count; i++)
            {
                Assert.AreEqual(trajectoryLength, ExpectedTrajectories_[i].RowCount);
                Assert.AreEqual(trajectoriesNo, ExpectedTrajectories_[i].ColumnCount);
            }
        }
        [TestMethod]
        public void SimpleFixedRateTest_DiscountFactor()
        {
            
            ModelSimpleFixedRate model = new ModelSimpleFixedRate(
                0.1, 1, trajectoriesNo, trajectoryLength, 2,
                mu, sigma, x0, correlationMatrix
                                  );
            double ExpectedFactor_1 =1/ Math.Pow(1.1,0.5);
            double ExpectedFactor_2 = 1/1.1;
            for (int i = 0; i < trajectoryLength; i++)
            {
                Assert.AreEqual(ExpectedFactor_1, model.DiscountFactor(i,1,2));
                Assert.AreEqual(ExpectedFactor_1, model.DiscountFactor(i, 2, 3));
                Assert.AreEqual(ExpectedFactor_2, model.DiscountFactor(i, 0, 2));
            }
        }
        [TestMethod]
        public void SimpleFixedRateTest_DiscountToZero()
        {

            ModelSimpleFixedRate model = new ModelSimpleFixedRate(
                0.1, 1, trajectoriesNo, trajectoryLength, 2,
                mu, sigma, x0, correlationMatrix
                                  );
            la.Vector<double> ExpectedFactor_ = la.CreateVector.Dense<double>(trajectoriesNo, 0);
            List<int> StoppingTime = new List<int>();
            la.Vector<double> ExpectedTimes_ = model.Times();
            for (int i = 0; i < trajectoriesNo; i++)
            {
                StoppingTime.Add( (i+1) % trajectoryLength);
                ExpectedFactor_[i] = 1 / Math.Pow(1.1, ExpectedTimes_[StoppingTime[i]-1]);
            }
            Assert.AreEqual(ExpectedFactor_, model.DiscountToZero(StoppingTime));
        }

    }
}