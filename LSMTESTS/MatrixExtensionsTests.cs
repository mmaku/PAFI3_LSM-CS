using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MathNet.Numerics.LinearAlgebra;

namespace LSMTESTS
{
    [TestClass]
    public class MatrixExtensionsTests
    {
        Matrix<double> a = CreateMatrix.Dense<double>(1, 1, 1);
        Matrix<double> b = CreateMatrix.Dense<double>(2, 1, 1);
        Matrix<double> c = CreateMatrix.Dense<double>(2, 1, 0);
        Matrix<double> d = CreateMatrix.Dense<double>(1, 2, 1);
        Matrix<double> e = CreateMatrix.Dense<double>(1, 2, 0);


        [TestMethod]
        public void AreEqualTest()
        {
        

            Assert.AreEqual(true, LSM_CS.MatrixExtensions.AreEqual(a, a));
            Assert.AreEqual(true, LSM_CS.MatrixExtensions.AreEqual(b, b));
            Assert.AreEqual(true, LSM_CS.MatrixExtensions.AreEqual(c, c));
            Assert.AreEqual(true, LSM_CS.MatrixExtensions.AreEqual(d, d));
            Assert.AreEqual(true, LSM_CS.MatrixExtensions.AreEqual(e, e));

            Assert.AreNotEqual(true, LSM_CS.MatrixExtensions.AreEqual(a, b));
            Assert.AreNotEqual(true, LSM_CS.MatrixExtensions.AreEqual(b, c));
            Assert.AreNotEqual(true, LSM_CS.MatrixExtensions.AreEqual(c, e));
            Assert.AreNotEqual(true, LSM_CS.MatrixExtensions.AreEqual(a, d));
            Assert.AreNotEqual(true, LSM_CS.MatrixExtensions.AreEqual(d, e));
        }
        
        [TestMethod]
        public void ColCumSumTest()
        {
            Matrix<double> bColSum = b.Clone();
            bColSum[1, 0] = 2;

            Assert.AreEqual(true, LSM_CS.MatrixExtensions.
                AreEqual(a, LSM_CS.MatrixExtensions.ColCumSum(a)));
            Assert.AreEqual(true, LSM_CS.MatrixExtensions.
                AreEqual(bColSum, LSM_CS.MatrixExtensions.ColCumSum(b)));
            Assert.AreEqual(true, LSM_CS.MatrixExtensions.
                AreEqual(c, LSM_CS.MatrixExtensions.ColCumSum(c)));
        }
        [TestMethod]
        public void ColCumSumAvgTest()
        {
            Matrix<double> bColSumAvg = b.Clone();
            bColSumAvg[1, 0] = 1;

            Assert.AreEqual(true, LSM_CS.MatrixExtensions.
                AreEqual(a, LSM_CS.MatrixExtensions.ColCumSumAvg(a)));
            Assert.AreEqual(true, LSM_CS.MatrixExtensions.
                AreEqual(b, LSM_CS.MatrixExtensions.ColCumSumAvg(b)));
            Assert.AreEqual(true, LSM_CS.MatrixExtensions.
                AreEqual(c, LSM_CS.MatrixExtensions.ColCumSumAvg(c)));
        }

    }
}
