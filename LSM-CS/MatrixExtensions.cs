using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MathNet.Numerics.LinearAlgebra;

namespace LSM_CS
{

    public static class MatrixExtensions
    {
        public static Matrix<double> ColCumSum (Matrix<double> a)
        {
            Matrix<double> b = CreateMatrix.Dense<double>(a.RowCount, a.ColumnCount);
            
            for (int j = 0; j <a.ColumnCount; j++)
            {
                b[0, j] = a[0, j];
            }

            for (int i = 1; i < a.RowCount; i++)
            {
                for (int j = 0; j < a.ColumnCount; j++)
                {
                    b[i, j] = a[i, j] + b[i - 1, j];
                }
            }
            return b;
        }
        public static Matrix<double> ColCumSumAvg (Matrix<double> a)
        {
            Matrix < double > b = ColCumSum(a);
            for (int i = 0; i < a.RowCount; i++)
            {
                for (int j = 0; j <a.ColumnCount; j++)
                {
                    b[i, j] = b[i, j] / (i + 1);
                }
            }
            return b;
        }
        public static bool AreEqual (Matrix<double> a, Matrix<double> b)
        {
            if ( Math.Pow(a.RowCount - b.RowCount,2 ) + Math.Pow(a.ColumnCount - b.ColumnCount, 2) > 0)
            {
                return false;
            }
            for (int i = 0; i < a.RowCount; i++)
            {
                for (int j = 0; j < a.ColumnCount; j++)
                {
                    if (a[i, j] != b[i, j])
                    //a nie chcemy tu DoubleExtentions? (sprawdzanie, ze nie roznia sie o wiecej niz epsilon)
                    //if(Math.Abs(a[i,j] - b[i,j]) > 1E-19)
                    {
                        return false;
                    }
                }
                
            }
            return true;
        }
    }
}
