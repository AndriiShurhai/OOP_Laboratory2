using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Laba_2
{
    public partial class MyMatrix
    {
        public static MyMatrix operator +(MyMatrix a, MyMatrix b)
        {
            ValidateSizeForAddition(a, b);
            double[][] result = new double[a.Height][];

            for (int i = 0; i < a.Height; i++) {
                double[] row = new double[a.Width];
                for (int j = 0; j < a.Height; j++)
                {
                    row[j] = a[i, j] + b[i, j];
                }
                result[i] = row;
            }
            return new MyMatrix(result);
        }

        public static MyMatrix operator *(MyMatrix a, MyMatrix b)
        {
            ValidateSizeForMultyplying(a, b);
            double[][] result = new double[a.Height][];

            for (int i = 0; i < a.Height; i++)
            {
                double[] row = new double[a.Width];
                for (int j = 0; j < a.Width; j++)
                {
                    double number = 0;
                    for (int k = 0; k < b.Width; k++)
                    {
                        number += a[i, k] * b[k, i]; 
                    }
                    row[j] = number;
                }
                result[i] = row;
            }
            return new MyMatrix(result);
        }

        private double[,] GetTransposedArray()
        {
            double[,] result = new double[Width, Height];

            for (int i = 0; i < Height; i++)
            {
                for (int j = 0; j < Width; j++)
                {
                    result[j, i] = this[i, j];
                }
            }

            return result;
        }

        public MyMatrix GetTransposedCopy() => new MyMatrix(GetTransposedArray());

        public void TransposeMe()
        {
            this.matrix = GetTransposedArray();
            cashedDeterminant = null;
        }

        public double CalculateDeterminant()
        {
            ValidateSquareSizeOfMatrix();

            if (cashedDeterminant.HasValue)
            {
                return cashedDeterminant.Value;
            }

            double[,] temp = new double[Width, Height];
            for (int i = 0; i < Height; ++i) {
                for (int j = 0; j < Width; ++j) {
                    temp[i, j] = this[i, j];
                }
            }

            double det = 1;
            for (int i = 0; i < Height; i++)
            {
                int maxRow = i;

                for (int k = i+1 ; k < Height; k++)
                {
                    if (Math.Abs(temp[k, i]) > Math.Abs(temp[maxRow, i]))
                    {
                        maxRow = k;
                    }
                }

                if (temp[maxRow, i] == 0)
                {
                    det = 0;
                    cashedDeterminant = 0;
                    return det;
                }

                if (maxRow != i)
                {
                    det *= -1;
                    for (int j = 0; j < Height; j++)
                    {
                        (temp[i, j], temp[maxRow, j]) = (temp[maxRow, j], temp[i, j]);
                    }
                }

                det *= temp[i, i];

                for (int k = i+1; k < Height; k++)
                {
                    double factor = temp[k, i] / temp[i, i];
                    for (int j = i; j < Height; j++)
                    {
                        temp[k, j] -= factor * temp[i, j];
                    }
                }
            }
            cashedDeterminant = det;
            return det;
        }

        private static void ValidateSizeForAddition(MyMatrix a, MyMatrix b)
        {
            if (a.Height != b.Height || a.Width != b.Width)
            {
                throw new ArgumentException("Must be same size to execute addition");
            }
        }

        private static void ValidateSizeForMultyplying(MyMatrix a, MyMatrix b)
        {
            if (a.Width != b.Height)
            {
                throw new ArgumentException("The numbers of rows in matrix A must be the same as the number of columns in matrix B to execute multypling");
            }
        }

        private void ValidateSquareSizeOfMatrix()
        {
            if (Height != Width)
            {
                throw new InvalidOperationException("Matrix must be squared");
            }
        }

        private void RestartDeterminant()
        {
            cashedDeterminant = null;
        }
    }
}
