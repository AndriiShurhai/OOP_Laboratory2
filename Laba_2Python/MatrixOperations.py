from MatrixData import MyMatrix
import numpy as np

class MyMatrix(MyMatrix):
    def __add__(self, other):
        self._validate_size_for_addition()
        return MyMatrix(self.matrix + other.matrix)
    
    def __mul__(self, other):
        self._validate_size_for_multiplication()
        if isinstance(other, MyMatrix):
            return MyMatrix(np.dot(self.matrix, other.matrix))
        elif isinstance(other, (int, float)):
            return MyMatrix(self.matrix * other)
        else:
            pass

    def get_transposed_array(self):
        return self.matrix.T
    
    def get_transposed_copy(self):
        return MyMatrix(self.get_transposed_array())
    
    def transpose_me(self):
        self.matrix = self.get_transposed_array()
        self._cached_determinant = None

    def calculate_determinant(self):
        self._validate_square_matrix()

        if (self._cached_determinant is not None):
            return self._cached_determinant
        
        det = self._cached_determinant = np.linalg.det(self.matrix)

        return det
    
    def _validate_size_for_addition(self, other):
        if self.height != other.height or self.width != other.width:
            raise ValueError("Matrix dimensions must match for addition")

    def _validate_size_for_multiplication(self, other):
        if self.width != other.height:
            raise ValueError("Matrix A columns must match Matrix B rows for multiplication")

    def _validate_square_matrix(self):
        if self.height != self.width:
            raise ValueError("Matrix must be square")

