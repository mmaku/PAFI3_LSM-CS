using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using LSM_CS;
using MathNet.Numerics.LinearAlgebra;
using System.Collections.Generic;

namespace LSMTESTS
{
    [TestClass]
    public class TrajectoryGeneratorTests
    {
        //TrajectoryGeneratoring pre calculation validation
        [TestClass]
        public class ValidationMethodsTests
        {
            [TestMethod]
            [ExpectedException(typeof(ArgumentException),
                "Number of trajectories needs to be a positive integer")]

            public void TrajValidationTest()
            {
                Matrix<double> cor1 = CreateMatrix.Dense<double>(1, 1, 1);
                Vector<double> v1 = CreateVector.Dense<double>(1, 1);
                
                List<Matrix<double>> res = new List<Matrix<double>>();
                res = TrajectoryGenerator.Sample(1, -1, -1, -1, v1, v1, v1, cor1);
            }

            [TestMethod]
            [ExpectedException(typeof(ArgumentException),
                "Length of trajectory needs to be a positive integer")]
            public void TrajNoValidationTest()
            {
                Matrix<double> cor1 = CreateMatrix.Dense<double>(1, 1, 1);
                Vector<double> v1 = CreateVector.Dense<double>(1, 1);
                
                List<Matrix<double>> res = new List<Matrix<double>>();
                res = TrajectoryGenerator.Sample(1, 1, -1, -1, v1, v1, v1, cor1);
            }

            [TestMethod]
            [ExpectedException(typeof(ArgumentException),
                "endTime needs to be a positive")]
            public void EndTimeTest()
            {
                Matrix<double> cor1 = CreateMatrix.Dense<double>(1, 1, 1);
                Vector<double> v1 = CreateVector.Dense<double>(1, 1);
                
                List<Matrix<double>> res = new List<Matrix<double>>();
                res = TrajectoryGenerator.Sample(1, 1, 1, -1, v1, v1, v1, cor1);
            }

            [TestMethod]
            [ExpectedException(typeof(ArgumentException),
                "Dimensions don't match")]
            public void WrongDimTest1()
            {
                Matrix<double> cor1 = CreateMatrix.Dense<double>(2, 1, 1);
                Vector<double> v1 = CreateVector.Dense<double>(1, 1);
                
                List<Matrix<double>> res = new List<Matrix<double>>();
                res = TrajectoryGenerator.Sample(1, 1, 1, 1, v1, v1, v1, cor1);
            }

            [TestMethod]
            [ExpectedException(typeof(ArgumentException),
                "Dimensions don't match")]
            public void WrongDimTest2()
            {
                Matrix<double> cor1 = CreateMatrix.Dense<double>(1, 2, 1);
                Vector<double> v1 = CreateVector.Dense<double>(1, 1);
                
                List<Matrix<double>> res = new List<Matrix<double>>();
                res = TrajectoryGenerator.Sample(1, 1, 1, 1, v1, v1, v1, cor1);
            }

            [TestMethod]
            [ExpectedException(typeof(ArgumentException),
                "Dimensions don't match")]
            public void WrongDimTest3()
            {
                Matrix<double> cor1 = CreateMatrix.Dense<double>(1, 1, 1);
                Vector<double> v1 = CreateVector.Dense<double>(2, 1);
                
                List<Matrix<double>> res = new List<Matrix<double>>();
                res = TrajectoryGenerator.Sample(1, 1, 1, 1, v1, v1, v1, cor1);
            }

            [TestMethod]
            [ExpectedException(typeof(ArgumentException),
                "Correlation matrix needs to have 1s on diagonal")]
            public void DiagTest1()
            {
                Matrix<double> cor1 = CreateMatrix.Dense<double>(2, 2, 1);
                cor1[0, 0] = 0.1;
                Vector<double> v1 = CreateVector.Dense<double>(2, 1);
                
                List<Matrix<double>> res = new List<Matrix<double>>();
                res = TrajectoryGenerator.Sample(1, 1, 1, 1, v1, v1, v1, cor1);
            }

            [TestMethod]
            [ExpectedException(typeof(ArgumentException),
                "Correlation matrix needs to have 1s on diagonal")]
            public void DiagTest2()
            {
                Matrix<double> cor1 = CreateMatrix.Dense<double>(2, 2, 1);
                cor1[0, 0] = 2;
                Vector<double> v1 = CreateVector.Dense<double>(2, 1);
                
                List<Matrix<double>> res = new List<Matrix<double>>();
                res = TrajectoryGenerator.Sample(1, 1, 1, 1, v1, v1, v1, cor1);
            }
            [TestMethod]
            [ExpectedException(typeof(ArgumentException),
                "Correlation matrix needs to have 1s on diagonal")]
            public void DiagTest3()
            {
                Matrix<double> cor1 = CreateMatrix.Dense<double>(2, 2, 1);
                cor1[1, 1] = 0.1;
                Vector<double> v1 = CreateVector.Dense<double>(2, 1);
                
                List<Matrix<double>> res = new List<Matrix<double>>();
                res = TrajectoryGenerator.Sample(1, 1, 1, 1, v1, v1, v1, cor1);
            }
            [TestMethod]
            [ExpectedException(typeof(ArgumentException),
                "Correlations need to be between -1 and 1 (exclusive outside of diagonal)")]
            public void TooHighCorTest1()
            {
                Matrix<double> cor1 = CreateMatrix.Dense<double>(2, 2, 1);
                cor1[0, 1] = 2;
                Vector<double> v1 = CreateVector.Dense<double>(2, 1);
                
                List<Matrix<double>> res = new List<Matrix<double>>();
                res = TrajectoryGenerator.Sample(1, 1, 1, 1, v1, v1, v1, cor1);
            }

            [TestMethod]
            [ExpectedException(typeof(ArgumentException),
                "Correlations need to be between -1 and 1 (exclusive outside of diagonal)")]
            public void TooHighCorTest2()
            {
                Matrix<double> cor1 = CreateMatrix.Dense<double>(2, 2, 1);
                cor1[1, 0] = 2;
                Vector<double> v1 = CreateVector.Dense<double>(2, 1);
                
                List<Matrix<double>> res = new List<Matrix<double>>();
                res = TrajectoryGenerator.Sample(1, 1, 1, 1, v1, v1, v1, cor1);
            }

            [TestMethod]
            [ExpectedException(typeof(ArgumentException),
                "Correlations need to be between -1 and 1 (exclusive outside of diagonal)")]
            public void TooLowCorTest1()
            {
                Matrix<double> cor1 = CreateMatrix.Dense<double>(2, 2, 1);
                cor1[0, 1] = -2;
                Vector<double> v1 = CreateVector.Dense<double>(2, 1);
                
                List<Matrix<double>> res = new List<Matrix<double>>();
                res = TrajectoryGenerator.Sample(1, 1, 1, 1, v1, v1, v1, cor1);
            }

            [TestMethod]
            [ExpectedException(typeof(ArgumentException),
                "Correlations need to be between -1 and 1 (exclusive outside of diagonal)")]
            public void TooLowCorTest2()
            {
                Matrix<double> cor1 = CreateMatrix.Dense<double>(2, 2, 1);
                cor1[1, 0] = -2;
                Vector<double> v1 = CreateVector.Dense<double>(2, 1);
                
                List<Matrix<double>> res = new List<Matrix<double>>();
                res = TrajectoryGenerator.Sample(1, 1, 1, 1, v1, v1, v1, cor1);
            }
            [TestMethod]
            [ExpectedException(typeof(ArgumentException),
                "Correlation matrix needs to bo symmetric and positive definite")]
            public void BadMatrixTest1()
            {
                Matrix<double> cor1 = CreateMatrix.Dense<double>(2, 2, 1);
                cor1[1, 0] = 0.5;
                Vector<double> v1 = CreateVector.Dense<double>(2, 1);
                
                List<Matrix<double>> res = new List<Matrix<double>>();
                res = TrajectoryGenerator.Sample(1, 1, 1, 1, v1, v1, v1, cor1);
            }
            [TestMethod]
            [ExpectedException(typeof(ArgumentException),
                "Correlation matrix needs to bo symmetric and positive definite")]
            public void BadMatrixTest2()
            {
                Matrix<double> cor1 = CreateMatrix.Dense<double>(2, 2, 1);
                cor1[1, 0] = -0.5;
                Vector<double> v1 = CreateVector.Dense<double>(2, 1);
                
                List<Matrix<double>> res = new List<Matrix<double>>();
                res = TrajectoryGenerator.Sample(1, 1, 1, 1, v1, v1, v1, cor1);
            }
            [TestMethod]
            [ExpectedException(typeof(ArgumentException),
                "Correlation matrix needs to bo symmetric and positive definite")]
            public void BadMatrixTest3()
            {
                Matrix<double> cor1 = CreateMatrix.Dense<double>(3, 3, 1);
                cor1[2, 0] = -0.5;
                Vector<double> v1 = CreateVector.Dense<double>(3, 1);
                
                List<Matrix<double>> res = new List<Matrix<double>>();
                res = TrajectoryGenerator.Sample(1, 1, 1, 1, v1, v1, v1, cor1);
            }
        }

        //TrajectoryGeneratoring results dimensions
        [TestClass]
        public class DimensionsAndCalculationTests
        {
            [TestMethod]
            public void MatrixCountTest1()
            {
                Matrix<double> cor1 = CreateMatrix.Dense<double>(1, 1, 1);
                Vector<double> v1 = CreateVector.Dense<double>(1, 1);
                
                List<Matrix<double>> res = new List<Matrix<double>>();
                res = TrajectoryGenerator.Sample(1, 2, 2, 1, v1, v1, v1, cor1);

                Assert.AreEqual(1, res.Count);
            }

            [TestMethod]
            public void MatrixCountTest2()
            {
                Matrix<double> cor1 = CreateMatrix.Dense<double>(3, 3, 0);
                for (int i = 0; i<3; i++)
                {
                    cor1[i, i] = 1;
                }
 
                Vector<double> v1 = CreateVector.Dense<double>(3, 1);
                
                List<Matrix<double>> res = TrajectoryGenerator.Sample(1, 2, 2, 1, v1, v1, v1, cor1);

                Assert.AreEqual(3, res.Count);
            }
            [TestMethod]
            public void PositiveDefTest1()
            {
                Matrix<double> cor1 = CreateMatrix.Dense<double>(3, 3, 0);
                for (int i = 0; i < 3; i++)
                {
                    cor1[i, i] = 1;
                }
                cor1[1, 2] = 0.5;
                cor1[2, 1] = 0.5;
                Vector<double> v1 = CreateVector.Dense<double>(3, 1);
                
                List<Matrix<double>> res = TrajectoryGenerator.Sample(1, 2, 2, 1, v1, v1, v1, cor1);

                Assert.AreEqual(3, res.Count);
            }
            [TestMethod]
            public void MatrixDimTest1()
            {
                Matrix<double> cor1 = CreateMatrix.Dense<double>(3, 3, 0);
                for (int i = 0; i < 3; i++)
                {
                    cor1[i, i] = 1;
                }
                cor1[1, 2] = 0.5;
                cor1[2, 1] = 0.5;
                Vector<double> v1 = CreateVector.Dense<double>(3, 1);
                Vector<double> x0 = CreateVector.Dense<double>(3, 1);
                x0[1] = 2;
                x0[2] = 3;
                
                int trajectoryLength = 3;
                int trajectoriesNo = 20;
                List<Matrix<double>> res = TrajectoryGenerator.Sample(1, trajectoriesNo, trajectoryLength, 1, v1, v1, v1, cor1);
                
                for (int i = 0; i<res.Count; i++)
                {
                    Assert.AreEqual(trajectoryLength, res[i].RowCount);
                    Assert.AreEqual(trajectoriesNo, res[i].ColumnCount);
                }
            }
            [TestMethod]
            public void properCalcTest1()
            {
                Matrix<double> cor1 = CreateMatrix.Dense<double>(3, 3, 0);
                for (int i = 0; i < 3; i++)
                {
                    cor1[i, i] = 1;
                }
                cor1[1, 2] = 0.5;
                cor1[2, 1] = 0.5;
                Vector<double> v1 = CreateVector.Dense<double>(3, 1);
                Vector<double> x0 = CreateVector.Dense<double>(3, 1);
                x0[1] = 2;
                x0[2] = 3;
                
                int trajectoryLength = 3;
                int trajectoriesNo = 20;
                List<Matrix<double>> res = TrajectoryGenerator.Sample(1, trajectoriesNo, trajectoryLength, 1, v1, v1, v1, cor1);

                for (int i = 0; i < res.Count; i++)
                {
                    for (int j = 0; j < trajectoriesNo; j ++)
                    {
                        for (int k = 0; k < trajectoryLength; k++)
                        {
                            Assert.AreNotEqual(x0[i], res[i][k, j]);
                        }
                        
                    }
                }
            }

        }
    }
}
