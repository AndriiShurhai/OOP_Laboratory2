using Xunit;


namespace Laba_2
{
    public class MyMatrixTests
    {
        [Fact]
        public void Constructor_From2DArray_CreatesCorrectMatrix()
        {
            double[,] array2D = {
            {1.0, 2.0, 3.0},
            {4.0, 5.0, 6.0}
        };

            var matrix = new MyMatrix(array2D);

            Assert.Equal(2, matrix.Height);
            Assert.Equal(3, matrix.Width);
            Assert.Equal(1.0, matrix[0, 0]);
            Assert.Equal(6.0, matrix[1, 2]);
        }

        [Fact]
        public void Constructor_FromJaggedArray_CreatesCorrectMatrix()
        {
            double[][] jaggedArray = new double[][] {
            new double[] {1.1, 1.2, 1.3},
            new double[] {2.1, 2.2, 2.3}
        };

            var matrix = new MyMatrix(jaggedArray);

            Assert.Equal(2, matrix.Height);
            Assert.Equal(3, matrix.Width);
            Assert.Equal(1.1, matrix[0, 0]);
            Assert.Equal(2.3, matrix[1, 2]);
        }

        [Fact]
        public void Constructor_FromNonRectangularJaggedArray_ThrowsArgumentException()
        {
            double[][] nonRectangularArray = new double[][] {
            new double[] {1.0, 2.0},
            new double[] {3.0, 4.0, 5.0}
        };

            Assert.Throws<ArgumentException>(() => new MyMatrix(nonRectangularArray));
        }

        [Theory]
        [InlineData("1.5 2.5          3.5", "               4.5  5.5 6.5")]
        [InlineData("1.5\t2.5\t3.5", "4.5\t5.5\t6.5")]
        public void Constructor_FromStringArray_CreatesCorrectMatrix(string row1, string row2)
        {
            string[] stringArray = { row1, row2 };

            var matrix = new MyMatrix(stringArray);

            Assert.Equal(2, matrix.Height);
            Assert.Equal(3, matrix.Width);
            Assert.Equal(1.5, matrix[0, 0]);
            Assert.Equal(6.5, matrix[1, 2]);
        }

        [Theory]
        [InlineData("7.0\t8.0\t9.0\n10.0\t11.0\t12.0")]
        [InlineData("7.0 8.0 9.0\n10.0 11.0 12.0")]
        public void Constructor_FromSingleString_CreatesCorrectMatrix(string matrixString)
        {
            var matrix = new MyMatrix(matrixString);

            Assert.Equal(2, matrix.Height);
            Assert.Equal(3, matrix.Width);
            Assert.Equal(7.0, matrix[0, 0]);
            Assert.Equal(12.0, matrix[1, 2]);
        }

        [Fact]
        public void CopyConstructor_CreatesIndependentCopy()
        {
            var original = new MyMatrix(new double[,] { { 1.0, 2.0 }, { 3.0, 4.0 } });

            var copy = new MyMatrix(original);
            copy[0, 0] = 99.9;

            Assert.NotEqual(copy[0, 0], original[0, 0]);
            Assert.Equal(1.0, original[0, 0]);
            Assert.Equal(99.9, copy[0, 0]);
        }

        [Fact]
        public void Properties_ReturnCorrectDimensions()
        {
            var matrix = new MyMatrix(new double[,] { { 1.0, 2.0, 3.0 }, { 4.0, 5.0, 6.0 } });

            Assert.Equal(2, matrix.Height);
            Assert.Equal(3, matrix.Width);
            Assert.Equal(2, matrix.getHeight());
            Assert.Equal(3, matrix.getWidth());
        }

        [Theory]
        [InlineData(0, 0, 10.0)]
        [InlineData(1, 1, 20.0)]
        public void Indexer_GetAndSetElements_WorksCorrectly(int row, int col, double value)
        {
            var matrix = new MyMatrix(new double[,] { { 1.0, 2.0 }, { 3.0, 4.0 } });

            matrix[row, col] = value;
            double result = matrix[row, col];

            Assert.Equal(value, result);
        }

        [Theory]
        [InlineData(0, 0, 15.0)]
        [InlineData(1, 1, 25.0)]
        public void JavaStyleAccessors_GetAndSetElements_WorksCorrectly(int row, int col, double value)
        {
            var matrix = new MyMatrix(new double[,] { { 1.0, 2.0 }, { 3.0, 4.0 } });

            matrix.setElement(row, col, value);
            double result = matrix.getElement(row, col);

            Assert.Equal(value, result);
        }

        [Theory]
        [InlineData(5, 5)]
        [InlineData(-1, 0)]
        [InlineData(0, -1)]
        public void Indexer_AccessOutOfRange_ThrowsException(int row, int col)
        {
            var matrix = new MyMatrix(new double[,] { { 1.0, 2.0 }, { 3.0, 4.0 } });

            Assert.Throws<IndexOutOfRangeException>(() => matrix[row, col]);
        }

        [Fact]
        public void ToString_FormatsMatrixCorrectly()
        {
            var matrix = new MyMatrix(new double[,] { { 1.0, 2.0 }, { 3.0, 4.0 } });
            string expected = "1\t2\r\n3\t4";

            string result = matrix.ToString();

            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData("1.0 2.0\n3.0")]
        [InlineData("")]
        [InlineData("abc def\nghi jkl")]
        public void Constructor_FromInvalidInput_ThrowsArgumentException(string invalidInput)
        {
            Assert.Throws<ArgumentException>(() => new MyMatrix(invalidInput));
        }

        [Fact]
        public void TransposeMe_WorksCorrectly()
        {
            // 2 5
            // 4 6


            var matrix = new MyMatrix(new double[,] { {2, 5}, {4, 6}});
            matrix.TransposeMe();


            Assert.Equal(2, matrix.Width);
            Assert.Equal(2, matrix.Height);
            Assert.Equal(2, matrix[0, 0]);
            Assert.Equal(4, matrix[0, 1]);
            Assert.Equal(5, matrix[1, 0]);
            Assert.Equal(6, matrix[1, 1]);
        }

        [Fact]
        public void DeterminantCalculatesCorrectlyForSquareMatrix()
        {
            var matrix = new MyMatrix(new double[,] { { 1.0, 2.0 }, { 3.0, 4.0 } });
            double determinant = matrix.CalculateDeterminant();

            Assert.Equal(-2.0, determinant, 5);
        }
    }
}
