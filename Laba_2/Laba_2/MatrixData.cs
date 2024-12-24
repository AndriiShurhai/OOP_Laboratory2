using System.Globalization;


namespace Laba_2
{
    public partial class MyMatrix
    {
        private double[,] matrix;
        private double? cashedDeterminant;

        public MyMatrix(MyMatrix other)
        {
            matrix = new double[other.Height, other.Width];
            for (int i = 0; i < Height; i++)
                for (int j = 0; j < Width; j++)
                    matrix[i, j] = other[i, j];
        }

        public MyMatrix(double[,] array)
        {
            matrix = new double[array.GetLength(0), array.GetLength(1)];
            for (int i = 0; i < Height; i++)
                for (int j = 0; j < Width; j++)
                    matrix[i, j] = array[i, j];
        }

        public MyMatrix(double[][] jaggedArray)
        {
            ValidateJaggedArray(jaggedArray);
            int width = jaggedArray[0].Length;
            matrix = new double[jaggedArray.Length, width];
            for (int i = 0; i < Height; i++)
                for (int j = 0; j < Width; j++)
                    matrix[i, j] = jaggedArray[i][j];
        }

        public MyMatrix(string[] rows)
        {
            if (rows == null || rows.Length == 0)
                throw new ArgumentException("Array cannot be empty");

            var firstRow = rows[0].Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
            matrix = new double[rows.Length, firstRow.Length];

            for (int i = 0; i < rows.Length; i++)
            {
                var numbers = rows[i].Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
                if (numbers.Length != firstRow.Length)
                    throw new ArgumentException("All rows must have the same number of elements");

                for (int j = 0; j < numbers.Length; j++)
                    if (!double.TryParse(numbers[j], out matrix[i, j]))
                        throw new ArgumentException($"Invalid number format at row {i}, column {j}");
            }
        }

        public MyMatrix(string matrixString)
        {
            var rows = matrixString.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
            if (rows.Length == 0)
                throw new ArgumentException("String cannot be empty");

            var firstRow = rows[0].Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
            matrix = new double[rows.Length, firstRow.Length];

            for (int i = 0; i < rows.Length; i++)
            {
                var numbers = rows[i].Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
                if (numbers.Length != firstRow.Length)
                    throw new ArgumentException("All rows must have the same number of elements");

                for (int j = 0; j < numbers.Length; j++)
                    if (!double.TryParse(numbers[j], out matrix[i, j]))
                        throw new ArgumentException($"Invalid number format at row {i}, column {j}");
            }
        }

        public int Height => matrix.GetLength(0);
        public int Width => matrix.GetLength(1);

        public int getHeight() => Height;
        public int getWidth() => Width;

        public double this[int i, int j]
        {
            get
            {
                ValidateIndexes(i, j);
                return matrix[i, j];
            }
            set
            {
                ValidateIndexes(i, j);
                cashedDeterminant = null;
                matrix[i, j] = value;
            }
        }

        public double getElement(int i, int j)
        {
            ValidateIndexes(i, j);
            return matrix[i, j];
        }

        public void setElement(int i, int j, double value)
        {
            ValidateIndexes(i, j);
            cashedDeterminant = null;
            matrix[i, j] = value;
        }

        private void ValidateIndexes(int i, int j)
        {
            if (i < 0 || i >= Height)
                throw new IndexOutOfRangeException($"Row index {i} is out of range [0, {Height})");
            if (j < 0 || j >= Width)
                throw new IndexOutOfRangeException($"Column index {j} is out of range [0, {Width})");
        }

        private void ValidateJaggedArray(double[][] jaggedArray)
        {
            if (jaggedArray == null || jaggedArray.Length == 0)
                throw new ArgumentException("Matrix cannot be empty");

            int width = jaggedArray[0].Length;

            for (int i = 1; i < jaggedArray.Length; i++)
                if (jaggedArray[i].Length != width)
                    throw new ArgumentException("Matrix must be rectangular");
        }

        public override string ToString()
        {
            var result = new System.Text.StringBuilder();
            for (int i = 0; i < Height; i++)
            {
                for (int j = 0; j < Width; j++)
                {
                    result.Append(matrix[i, j]);
                    if (j < Width - 1)
                        result.Append('\t');
                }
                if (i < Height - 1)
                    result.AppendLine();
            }
            return result.ToString();
        }
    }
}
