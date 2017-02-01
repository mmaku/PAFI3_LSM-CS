using LSM_CS;
using MathNet.Numerics;
using MathNet.Numerics.LinearAlgebra;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LSMTESTS
{
    [TestClass]
    public class RegressionTests
    {

        [TestClass]
        public class LinearRegressionTests
        {
            Vector<double> a = CreateVector.Dense<double>(new double[3] { 4.0, 6.0, 8.0 });
            Vector<double> b = CreateVector.Dense<double>(new double[3] { 1.0, 2.0, 3.0 });

            LinearRegression testLR1 = new LinearRegression();
            LinearRegression testLR2 = new LinearRegression(false);

            [TestMethod]
            public void BuildModelTest()
            {
                testLR1.BuildModel(a, b);
                testLR2.BuildModel(a, b);

                Vector<double> testCoef1 = testLR1.Coefficients();
                Vector<double> testCoef2 = testLR2.Coefficients();

                Assert.IsTrue(DoubleExtensions.AreEqual(testCoef1[0], 0.5));
                Assert.IsTrue(DoubleExtensions.AreEqual(testCoef1[1], -1.0));
                Console.Write(testCoef2[0]);
                Assert.IsTrue(DoubleExtensions.AreEqual(testCoef2[0], 0.3448275862));
            }

            [TestMethod]
            public void IsInterceptTest()
            {
                Assert.IsTrue(testLR1.IsIntercept());
                Assert.IsFalse(testLR2.IsIntercept());
            }

            [TestMethod]
            public void CoeficientCountTest()
            {
                Assert.AreEqual(testLR1.CoefficientsCount, 2);
                Assert.AreEqual(testLR2.CoefficientsCount, 1);
            }

            [TestMethod]
            public void DataTest()
            {
                testLR1.BuildModel(a, b);
                testLR2.BuildModel(a, b);

                Assert.AreEqual(testLR1.Data(), CreateMatrix.DenseOfColumnVectors<double>(new Vector<double>[2] {a, CreateVector.Dense<double>(3, 1.0)}));
                Assert.AreEqual(testLR2.Data(), CreateMatrix.DenseOfColumnVectors<double>(a));
            }
        }



    }
}
