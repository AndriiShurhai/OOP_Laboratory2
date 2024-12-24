import unittest
import numpy as np
from MatrixOperations import MyMatrix


class TestMyMatrix(unittest.TestCase):
    def test_constructor_from_2d_array_creates_correct_matrix(self):
        array_2d = [[1.0, 2.0, 3.0], [4.0, 5.0, 6.0]]
        matrix = MyMatrix(array_2d)

        self.assertEqual(matrix.height, 2)
        self.assertEqual(matrix.width, 3)
        self.assertEqual(matrix[0, 0], 1.0)
        self.assertEqual(matrix[1, 2], 6.0)

    def test_constructor_from_numpy_array_creates_correct_matrix(self):
        numpy_array = np.array([[1.1, 1.2, 1.3], [2.1, 2.2, 2.3]])
        matrix = MyMatrix(numpy_array)

        self.assertEqual(matrix.height, 2)
        self.assertEqual(matrix.width, 3)
        self.assertEqual(matrix[0, 0], 1.1)
        self.assertEqual(matrix[1, 2], 2.3)

    def test_constructor_from_non_rectangular_array_raises_exception(self):
        non_rectangular_array = [[1.0, 2.0], [3.0, 4.0, 5.0]]

        with self.assertRaises(ValueError):
            MyMatrix(non_rectangular_array)

    def test_transpose_creates_correct_transpose(self):
        matrix = MyMatrix([[1.0, 2.0], [3.0, 4.0]])
        transposed = matrix.get_transposed_array()

        self.assertEqual(transposed[0, 0], 1.0)
        self.assertEqual(transposed[1, 0], 2.0)
        self.assertEqual(transposed[0, 1], 3.0)
        self.assertEqual(transposed[1, 1], 4.0)

    def test_transpose_me_modifies_matrix_correctly(self):
        matrix = MyMatrix([[1.0, 2.0], [3.0, 4.0]])
        matrix.transpose_me()

        self.assertEqual(matrix[0, 0], 1.0)
        self.assertEqual(matrix[1, 0], 2.0)
        self.assertEqual(matrix[0, 1], 3.0)
        self.assertEqual(matrix[1, 1], 4.0)

    def test_determinant_calculates_correctly(self):
        matrix = MyMatrix([[1.0, 2.0], [3.0, 4.0]])
        det = matrix.calculate_determinant()

        self.assertAlmostEqual(det, -2.0)

    def test_addition_works_correctly(self):
        matrix_a = MyMatrix([[1.0, 2.0], [3.0, 4.0]])
        matrix_b = MyMatrix([[5.0, 6.0], [7.0, 8.0]])
        result = matrix_a + matrix_b

        self.assertEqual(result[0, 0], 6.0)
        self.assertEqual(result[1, 1], 12.0)

    def test_multiplication_works_correctly(self):
        matrix_a = MyMatrix([[1.0, 2.0], [3.0, 4.0]])
        matrix_b = MyMatrix([[2.0, 0.0], [1.0, 2.0]])
        result = matrix_a * matrix_b

        self.assertEqual(result[0, 0], 4.0)
        self.assertEqual(result[1, 1], 8.0)

    def test_scalar_multiplication_works_correctly(self):
        matrix = MyMatrix([[1.0, 2.0], [3.0, 4.0]])
        result = matrix * 2.0

        self.assertEqual(result[0, 0], 2.0)
        self.assertEqual(result[1, 1], 8.0)

    def test_invalid_multiplication_raises_exception(self):
        matrix_a = MyMatrix([[1.0, 2.0]])
        matrix_b = MyMatrix([[1.0, 2.0]])

        with self.assertRaises(ValueError):
            _ = matrix_a * matrix_b

    def test_indexer_get_and_set_elements(self):
        matrix = MyMatrix([[1.0, 2.0], [3.0, 4.0]])
        matrix[0, 0] = 10.0

        self.assertEqual(matrix[0, 0], 10.0)

    def test_invalid_index_access_raises_exception(self):
        matrix = MyMatrix([[1.0, 2.0], [3.0, 4.0]])

        with self.assertRaises(IndexError):
            _ = matrix[10, 10]

    def test_to_string_formats_matrix_correctly(self):
        matrix = MyMatrix([[1.0, 2.0], [3.0, 4.0]])
        result = str(matrix)
        expected = "1.0\t2.0\n3.0\t4.0"

        self.assertEqual(result, expected)

    def test_constructor_invalid_input_raises_exception(self):
        with self.assertRaises(ValueError):
            MyMatrix("invalid input")

    def test_copy_constructor_creates_independent_copy(self):
        original = MyMatrix([[1.0, 2.0], [3.0, 4.0]])
        copy = MyMatrix(original.matrix.copy())

        copy[0, 0] = 99.9

        self.assertNotEqual(copy[0, 0], original[0, 0])
        self.assertEqual(original[0, 0], 1.0)
        self.assertEqual(copy[0, 0], 99.9)


if __name__ == "__main__":
    unittest.main()
